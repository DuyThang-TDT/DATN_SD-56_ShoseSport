using FurryFriends.Web.Services.IService;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Threading;

namespace FurryFriends.Web.Controllers
{
    public class HoaDonController : Controller
    {
        private readonly IHoaDonService _hoaDonService;
        private readonly IConfiguration _configuration;

        public HoaDonController(IHoaDonService hoaDonService, IConfiguration configuration)
        {
            _hoaDonService = hoaDonService;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> SendEmailInvoice(Guid id)
        {
            try
            {
                // Lấy thông tin hóa đơn
                var hoaDon = await _hoaDonService.GetHoaDonByIdAsync(id);
                if (hoaDon == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy hóa đơn" });
                }

                // Kiểm tra email khách hàng
                if (string.IsNullOrEmpty(hoaDon.EmailCuaKhachHang))
                {
                    return Json(new { success = false, message = "Khách hàng chưa cung cấp email" });
                }

                // ✅ Sửa: Gửi email ngay lập tức không có timeout
                try
                {
                    // Gửi email với nội dung chi tiết
                    await SendInvoiceEmailAsync(hoaDon, id.ToString().Substring(0, 8).ToUpper());

                    // ✅ Log thành công
                    Console.WriteLine($"✅ Email sent successfully to: {hoaDon.EmailCuaKhachHang}");

                    return Json(new { success = true, message = "Hóa đơn đã được gửi thành công!" });
                }
                catch (Exception emailEx)
                {
                    // ✅ Log lỗi chi tiết
                    Console.WriteLine($"❌ Email error: {emailEx.Message}");
                    Console.WriteLine($"❌ Email error details: {emailEx.StackTrace}");

                    return Json(new { success = false, message = $"Lỗi gửi email: {emailEx.Message}" });
                }
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Error in SendEmailInvoice: {ex.Message}");
                return Json(new { success = false, message = "Có lỗi xảy ra khi gửi email" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> TestEmail()
        {
            try
            {
                var smtpServer = _configuration["EmailSettings:SmtpServer"] ?? "smtp.gmail.com";
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
                var senderEmail = _configuration["EmailSettings:SenderEmail"] ?? "your-email@gmail.com";
                var senderPassword = _configuration["EmailSettings:SenderPassword"] ?? "your-app-password";

                Console.WriteLine($"🧪 Testing email configuration:");
                Console.WriteLine($"🧪 SMTP: {smtpServer}:{smtpPort}");
                Console.WriteLine($"🧪 From: {senderEmail}");

                using (var smtpClient = new SmtpClient(smtpServer))
                {
                    smtpClient.Port = smtpPort;
                    smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                    smtpClient.EnableSsl = true;
                    smtpClient.Timeout = 10000;

                    using (var mailMessage = new MailMessage(senderEmail, senderEmail)) // Gửi cho chính mình
                    {
                        mailMessage.Subject = "Test Email - FurryFriends";
                        mailMessage.Body = "Đây là email test từ FurryFriends. Nếu bạn nhận được email này, cấu hình email đã hoạt động!";
                        mailMessage.IsBodyHtml = false;

                        await smtpClient.SendMailAsync(mailMessage);
                    }
                }

                return Json(new { success = true, message = "Test email sent successfully!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Test email failed: {ex.Message}");
                return Json(new { success = false, message = $"Test email failed: {ex.Message}" });
            }
        }

        [HttpPost]
        private async Task SendInvoiceEmailAsync(object hoaDon, string invoiceNumber)
        {
            try
            {
                // Email configuration - you should store these in appsettings.json
                var smtpServer = _configuration["EmailSettings:SmtpServer"] ?? "smtp.gmail.com";
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
                var senderEmail = _configuration["EmailSettings:SenderEmail"] ?? "your-email@gmail.com";
                var senderPassword = _configuration["EmailSettings:SenderPassword"] ?? "your-app-password";
                var senderName = _configuration["EmailSettings:SenderName"] ?? "FurryFriends Store";

                // ✅ Lấy thông tin email từ object
                var emailProperty = hoaDon.GetType().GetProperty("EmailCuaKhachHang");
                var toEmail = emailProperty?.GetValue(hoaDon)?.ToString();

                if (string.IsNullOrEmpty(toEmail))
                {
                    throw new Exception("Email khách hàng không hợp lệ");
                }

                // ✅ Log thông tin email
                Console.WriteLine($"📧 Sending email to: {toEmail}");
                Console.WriteLine($"📧 SMTP Server: {smtpServer}:{smtpPort}");
                Console.WriteLine($"📧 From: {senderEmail}");

                // ✅ Tối ưu hóa SMTP client không có timeout
                using (var smtpClient = new SmtpClient(smtpServer))
                {
                    smtpClient.Port = smtpPort;
                    smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                    smtpClient.EnableSsl = true;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = false;

                    using (var mailMessage = new MailMessage(senderEmail, toEmail))
                    {
                        mailMessage.Subject = $"Hóa đơn #{invoiceNumber} - FurryFriends Store";
                        mailMessage.Body = CreateDetailedEmailBody(hoaDon, invoiceNumber);
                        mailMessage.IsBodyHtml = true;
                        mailMessage.Priority = MailPriority.Normal;

                        // ✅ Gửi email không có timeout
                        Console.WriteLine($"📧 Attempting to send email...");
                        await smtpClient.SendMailAsync(mailMessage);
                        Console.WriteLine($"✅ Email sent successfully!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Email sending failed: {ex.Message}");
                Console.WriteLine($"❌ Exception type: {ex.GetType().Name}");

                // ✅ Thêm thông tin chi tiết về lỗi
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"❌ Inner exception: {ex.InnerException.Message}");
                }

                throw new Exception($"Failed to send email: {ex.Message}", ex);
            }
        }

        private string CreateEmailBody(string customerName, string invoiceNumber)
        {
            return $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8'>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
                        .content {{ background: #f8f9fa; padding: 30px; border-radius: 0 0 10px 10px; }}
                        .logo {{ font-size: 24px; font-weight: bold; margin-bottom: 10px; }}
                        .footer {{ margin-top: 30px; text-align: center; color: #666; font-size: 14px; }}
                        .highlight {{ color: #667eea; font-weight: bold; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <div class='logo'>🐾 FurryFriends</div>
                            <h2>Cảm ơn bạn đã đặt hàng!</h2>
                        </div>
                        <div class='content'>
                            <p>Xin chào <strong>{customerName}</strong>,</p>
                            
                            <p>Cảm ơn bạn đã tin tưởng và mua sắm tại <span class='highlight'>FurryFriends Store</span>!</p>
                            
                            <p>Hóa đơn điện tử cho đơn hàng <strong>#{invoiceNumber}</strong> của bạn đã được đính kèm trong email này.</p>
                            
                            <div style='background: white; padding: 20px; border-radius: 8px; margin: 20px 0; border-left: 4px solid #667eea;'>
                                <h3 style='margin-top: 0; color: #667eea;'>📋 Thông tin đơn hàng</h3>
                                <p><strong>Mã đơn hàng:</strong> #{invoiceNumber}</p>
                                <p><strong>Ngày đặt:</strong> {DateTime.Now:dd/MM/yyyy HH:mm}</p>
                                <p><strong>Trạng thái:</strong> Đang xử lý</p>
                            </div>
                            
                            <p>Chúng tôi sẽ liên hệ với bạn sớm nhất để xác nhận và giao hàng.</p>
                            
                            <p>Nếu bạn có bất kỳ câu hỏi nào, vui lòng liên hệ với chúng tôi:</p>
                            <ul>
                                <li>📞 Hotline: <strong>0968596808</strong></li>
                                <li>📧 Email: <strong>info@furryfriends.vn</strong></li>
                                <li>🏪 Địa chỉ: <strong>142 Nguyễn Đổng Chi, Nam Từ Liêm, TP. Hà Nội</strong></li>
                            </ul>
                            
                            <p>Một lần nữa, cảm ơn bạn đã lựa chọn FurryFriends!</p>
                            
                            <p>Trân trọng,<br>
                            <strong>Đội ngũ FurryFriends Store</strong> 🐾</p>
                        </div>
                        <div class='footer'>
                            <p>© 2024 FurryFriends Store. All rights reserved.</p>
                            <p>www.furryfriends.vn | Powered by FurryFriends System</p>
                        </div>
                    </div>
                </body>
                </html>
            ";
        }

        private string CreateDetailedEmailBody(object hoaDon, string invoiceNumber)
        {
            // ✅ Lấy dữ liệu thật từ HoaDon object với các trường LucMua
            var type = hoaDon.GetType();

            // ✅ Lấy thông tin khách hàng từ hóa đơn
            var tenKhachHang = type.GetProperty("TenCuaKhachHang")?.GetValue(hoaDon)?.ToString() ?? "Khách hàng";
            var emailKhachHang = type.GetProperty("EmailCuaKhachHang")?.GetValue(hoaDon)?.ToString() ?? "";
            var soDienThoai = type.GetProperty("SdtCuaKhachHang")?.GetValue(hoaDon)?.ToString() ?? "";

            // ✅ Lấy thông tin địa chỉ giao hàng lúc mua
            var diaChiGiaoHangLucMua = type.GetProperty("DiaChiGiaoHangLucMua")?.GetValue(hoaDon)?.ToString() ?? "";

            // ✅ Lấy thông tin hình thức thanh toán
            var hinhThucProperty = type.GetProperty("HinhThucThanhToan");
            var hinhThuc = hinhThucProperty?.GetValue(hoaDon);
            var hinhThucThanhToan = "";

            if (hinhThuc != null)
            {
                var hinhThucType = hinhThuc.GetType();
                hinhThucThanhToan = hinhThucType.GetProperty("TenHinhThuc")?.GetValue(hinhThuc)?.ToString() ?? "Thanh toán khi nhận hàng";
            }

            // ✅ Lấy thông tin voucher
            var thongTinVoucherLucMua = type.GetProperty("ThongTinVoucherLucMua")?.GetValue(hoaDon)?.ToString() ?? "";

            // ✅ Lấy thông tin hóa đơn
            var ngayTao = type.GetProperty("NgayTao")?.GetValue(hoaDon);
            var trangThai = type.GetProperty("TrangThai")?.GetValue(hoaDon);
            var tongTien = type.GetProperty("TongTien")?.GetValue(hoaDon);
            var tongTienSauKhiGiam = type.GetProperty("TongTienSauKhiGiam")?.GetValue(hoaDon);

            // ✅ Tính toán giảm giá và phí vận chuyển
            var tienGiam = 0m;
            var phiVanChuyen = 0m;

            if (tongTien is decimal tongTienCalc && tongTienSauKhiGiam is decimal tongTienSauKhiGiamCalc)
            {
                tienGiam = tongTienCalc - tongTienSauKhiGiamCalc;
                // Tính phí vận chuyển (miễn phí nếu >= 500k, ngược lại 30k)
                if (tongTienSauKhiGiamCalc < 500000)
                {
                    phiVanChuyen = 30000;
                }
            }

            // ✅ Lấy chi tiết sản phẩm
            var chiTietsProperty = type.GetProperty("HoaDonChiTiets");
            var chiTiets = chiTietsProperty?.GetValue(hoaDon) as IEnumerable<object> ?? new List<object>();

            // ✅ Tạo nội dung email chi tiết với dữ liệu thật
            var emailBody = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8'>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 800px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
                        .content {{ background: #f8f9fa; padding: 30px; border-radius: 0 0 10px 10px; }}
                        .logo {{ font-size: 24px; font-weight: bold; margin-bottom: 10px; }}
                        .footer {{ margin-top: 30px; text-align: center; color: #666; font-size: 14px; }}
                        .highlight {{ color: #667eea; font-weight: bold; }}
                        .invoice-details {{ background: white; padding: 20px; border-radius: 8px; margin: 20px 0; border-left: 4px solid #667eea; }}
                        .product-table {{ width: 100%; border-collapse: collapse; margin: 20px 0; }}
                        .product-table th, .product-table td {{ padding: 12px; text-align: left; border-bottom: 1px solid #ddd; }}
                        .product-table th {{ background-color: #f8f9fa; font-weight: bold; }}
                        .total-section {{ background: #e8f5e8; padding: 20px; border-radius: 8px; margin: 20px 0; }}
                        .status-badge {{ display: inline-block; padding: 8px 16px; border-radius: 20px; font-size: 12px; font-weight: bold; text-transform: uppercase; }}
                        .status-pending {{ background: #fff3cd; color: #856404; }}
                        .status-approved {{ background: #d1ecf1; color: #0c5460; }}
                        .status-shipping {{ background: #d4edda; color: #155724; }}
                        .status-delivered {{ background: #d1e7dd; color: #0f5132; }}
                        .status-cancelled {{ background: #f8d7da; color: #721c24; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <div class='logo'>🐾 FurryFriends</div>
                            <h2>Hóa Đơn Điện Tử</h2>
                            <p>Mã hóa đơn: #{invoiceNumber}</p>
                        </div>
                        <div class='content'>
                            <p>Xin chào <strong>{tenKhachHang}</strong>,</p>
                            
                            <p>Cảm ơn bạn đã tin tưởng và mua sắm tại <span class='highlight'>FurryFriends Store</span>!</p>
                            
                            <div class='invoice-details'>
                                <h3 style='margin-top: 0; color: #667eea;'>📋 Thông tin đơn hàng</h3>
                                <p><strong>Mã đơn hàng:</strong> #{invoiceNumber}</p>
                                <p><strong>Ngày đặt:</strong> {(ngayTao is DateTime date ? date.ToString("dd/MM/yyyy HH:mm") : "N/A")}</p>
                                <p><strong>Trạng thái:</strong> 
                                    <span class='status-badge status-{GetStatusClass(trangThai is int status ? status : 0)}'>{GetStatusText(trangThai is int st ? st : 0)}</span>
                                </p>
                                <p><strong>Hình thức thanh toán:</strong> {hinhThucThanhToan}</p>
                                {(string.IsNullOrEmpty(thongTinVoucherLucMua) ? "" : $"<p><strong>Voucher áp dụng:</strong> {thongTinVoucherLucMua}</p>")}
                            </div>
                            
                            <div class='invoice-details'>
                                <h3 style='margin-top: 0; color: #667eea;'>👤 Thông tin khách hàng</h3>
                                <p><strong>Họ tên:</strong> {tenKhachHang}</p>
                                <p><strong>Số điện thoại:</strong> {soDienThoai}</p>
                                <p><strong>Email:</strong> {emailKhachHang}</p>
                                <p><strong>Địa chỉ:</strong> {diaChiGiaoHangLucMua}</p>
                            </div>
                            
                            <div class='invoice-details'>
                                <h3 style='margin-top: 0; color: #667eea;'>🛍️ Chi tiết sản phẩm</h3>
                                <table class='product-table'>
                                    <thead>
                                        <tr>
                                            <th>STT</th>
                                            <th>Sản phẩm</th>
                                            <th>Màu sắc</th>
                                            <th>Kích cỡ</th>
                                            <th>Số lượng</th>
                                            <th>Đơn giá</th>
                                            <th>Thành tiền</th>
                                        </tr>
                                    </thead>
                                    <tbody>";

            // ✅ Thêm chi tiết sản phẩm với dữ liệu LucMua
            int stt = 1;
            foreach (var chiTiet in chiTiets)
            {
                var chiTietType = chiTiet.GetType();

                // ✅ Lấy dữ liệu từ các trường LucMua
                var tenSanPhamLucMua = chiTietType.GetProperty("TenSanPhamLucMua")?.GetValue(chiTiet)?.ToString() ?? "";
                var mauSacLucMua = chiTietType.GetProperty("MauSacLucMua")?.GetValue(chiTiet)?.ToString() ?? "";
                var kichCoLucMua = chiTietType.GetProperty("KichCoLucMua")?.GetValue(chiTiet)?.ToString() ?? "";
                var soLuongSanPham = chiTietType.GetProperty("SoLuongSanPham")?.GetValue(chiTiet);
                var giaLucMua = chiTietType.GetProperty("GiaLucMua")?.GetValue(chiTiet);

                // ✅ Tính thành tiền từ số lượng và giá lúc mua
                decimal thanhTienTinh = 0;
                if (soLuongSanPham is int sl && giaLucMua is decimal giaValue)
                {
                    thanhTienTinh = sl * giaValue;
                }

                emailBody += $@"
                                        <tr>
                                            <td>{stt}</td>
                                            <td>{tenSanPhamLucMua}</td>
                                            <td>{mauSacLucMua}</td>
                                            <td>{kichCoLucMua}</td>
                                            <td>{soLuongSanPham}</td>
                                            <td>{(giaLucMua is decimal giaDisplay ? giaDisplay.ToString("N0") : "0")} VNĐ</td>
                                            <td>{thanhTienTinh.ToString("N0")} VNĐ</td>
                                        </tr>";
                stt++;
            }

            emailBody += $@"
                                    </tbody>
                                </table>
                            </div>
                            
                            <div class='total-section'>
                                <h3 style='margin-top: 0; color: #155724;'>💰 Tổng thanh toán</h3>
                                <p><strong>Tổng tiền hàng:</strong> {(tongTien is decimal tongTienDisplay ? tongTienDisplay.ToString("N0") : "0")} VNĐ</p>
                                {(tienGiam > 0 ? $"<p><strong>Giảm giá:</strong> - {tienGiam.ToString("N0")} VNĐ</p>" : "")}
                                <p><strong>Phí vận chuyển:</strong> {phiVanChuyen.ToString("N0")} VNĐ</p>
                                <hr style='border: none; border-top: 2px solid #155724; margin: 15px 0;'>
                                <p style='font-size: 18px; font-weight: bold; color: #155724;'>
                                    <strong>Tổng cộng:</strong> {(tongTienSauKhiGiam is decimal tongTienSauKhiGiamDisplay ? tongTienSauKhiGiamDisplay.ToString("N0") : "0")} VNĐ
                                </p>
                            </div>
                            
                            <p>Chúng tôi sẽ liên hệ với bạn sớm nhất để xác nhận và giao hàng.</p>
                            
                            <p>Nếu bạn có bất kỳ câu hỏi nào, vui lòng liên hệ với chúng tôi:</p>
                            <ul>
                                <li>📞 Hotline: <strong>0968596808</strong></li>
                                <li>📧 Email: <strong>info@furryfriends.vn</strong></li>
                                <li>🏪 Địa chỉ: <strong>142 Nguyễn Đổng Chi, Nam Từ Liêm, TP. Hà Nội</strong></li>
                            </ul>
                            
                            <p>Một lần nữa, cảm ơn bạn đã lựa chọn FurryFriends!</p>
                            
                            <p>Trân trọng,<br>
                            <strong>Đội ngũ FurryFriends Store</strong> 🐾</p>
                        </div>
                        <div class='footer'>
                            <p>© 2024 FurryFriends Store. All rights reserved.</p>
                            <p>www.furryfriends.vn | Powered by FurryFriends System</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

            return emailBody;
        }

        private string GetStatusClass(int status)
        {
            return status switch
            {
                0 => "pending",
                1 => "approved",
                2 => "shipping",
                3 => "delivered",
                4 => "cancelled",
                _ => "pending"
            };
        }

        private string GetStatusText(int status)
        {
            return status switch
            {
                0 => "Đang xử lý",
                1 => "Đã duyệt",
                2 => "Đang vận chuyển",
                3 => "Đã giao hàng",
                4 => "Đã hủy",
                _ => "Đang xử lý"
            };
        }
    }
}
