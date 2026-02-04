using System.Net;
using System.Net.Mail;
using FurryFriends.Web.Models;
using FurryFriends.Web.ViewModels;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace FurryFriends.Web.Services
{
    public interface IEmailNotificationService
    {
        Task SendOrderNotificationToAdminAsync(ThanhToanResultViewModel hoaDon);
        Task SendStatusChangeNotificationToCustomerAsync(object hoaDon, string oldStatus, string newStatus);
    }

    public class EmailNotificationService : IEmailNotificationService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailNotificationService> _logger;

        public EmailNotificationService(IConfiguration configuration, ILogger<EmailNotificationService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendOrderNotificationToAdminAsync(ThanhToanResultViewModel hoaDon)
        {
            try
            {
                // Lấy thông tin hóa đơn từ ThanhToanResultViewModel
                var hoaDonId = hoaDon.HoaDonId.ToString();
                var tenKhachHang = hoaDon.TenCuaKhachHang ?? "Khách hàng";
                var soDienThoai = hoaDon.SdtCuaKhachHang ?? "";
                var emailKhachHang = hoaDon.EmailCuaKhachHang ?? "";
                var tongTien = hoaDon.TongTienSauKhiGiam;
                var ngayTao = hoaDon.NgayTao; // ✅ Sử dụng ngày từ API
                
                // Lấy chi tiết sản phẩm
                var chiTiets = hoaDon.ChiTietSanPham ?? new List<HoaDonChiTietViewModel>();

                // ✅ Debug logging
                _logger.LogInformation($"🔍 Admin Debug - HoaDonId: {hoaDonId}");
                _logger.LogInformation($"🔍 Admin Debug - TenKhachHang: {tenKhachHang}");
                _logger.LogInformation($"🔍 Admin Debug - SoDienThoai: {soDienThoai}");
                _logger.LogInformation($"🔍 Admin Debug - EmailKhachHang: {emailKhachHang}");
                _logger.LogInformation($"🔍 Admin Debug - TongTien: {tongTien}");
                _logger.LogInformation($"🔍 Admin Debug - NgayTao: {ngayTao}");
                _logger.LogInformation($"🔍 Admin Debug - ChiTiets Count: {chiTiets.Count}");
                
                // ✅ Log chi tiết sản phẩm
                foreach (var chiTiet in chiTiets)
                {
                    _logger.LogInformation($"🔍 Admin Debug - ChiTiet: {chiTiet.TenSanPhamLucMua}, SL: {chiTiet.SoLuong}, Gia: {chiTiet.GiaLucMua}");
                }

                var adminEmail = _configuration["EmailSettings:AdminEmail"] ?? "admin@furryfriends.vn";
                var subject = $"🆕 Đơn hàng mới #{hoaDonId.Substring(0, 8).ToUpper()} - FurryFriends Store";
                
                var emailBody = CreateAdminOrderNotificationBody(hoaDonId, tenKhachHang, soDienThoai, emailKhachHang, tongTien, ngayTao, chiTiets);
                
                await SendEmailAsync(adminEmail, subject, emailBody);
                
                _logger.LogInformation($"✅ Admin notification sent for order {hoaDonId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"❌ Error sending admin notification: {ex.Message}");
                throw;
            }
        }

        public async Task SendStatusChangeNotificationToCustomerAsync(object hoaDon, string oldStatus, string newStatus)
        {
            try
            {
                var type = hoaDon.GetType();
                
                // Lấy thông tin hóa đơn
                var hoaDonId = type.GetProperty("HoaDonId")?.GetValue(hoaDon)?.ToString() ?? "";
                var tenKhachHang = type.GetProperty("TenCuaKhachHang")?.GetValue(hoaDon)?.ToString() ?? "Khách hàng";
                
                // ✅ Thử lấy email từ nhiều thuộc tính khác nhau
                var emailKhachHang = type.GetProperty("EmailCuaKhachHang")?.GetValue(hoaDon)?.ToString() ?? 
                                    type.GetProperty("Email")?.GetValue(hoaDon)?.ToString() ?? 
                                    type.GetProperty("KhachHang")?.GetValue(hoaDon)?.GetType().GetProperty("Email")?.GetValue(type.GetProperty("KhachHang")?.GetValue(hoaDon))?.ToString() ?? "";
                
                var tongTien = type.GetProperty("TongTienSauKhiGiam")?.GetValue(hoaDon);
                var ngayTao = type.GetProperty("NgayTao")?.GetValue(hoaDon);
                
                // ✅ Debug logging
                _logger.LogInformation($"🔍 Debug - HoaDonId: {hoaDonId}");
                _logger.LogInformation($"🔍 Debug - TenKhachHang: {tenKhachHang}");
                _logger.LogInformation($"🔍 Debug - EmailKhachHang: {emailKhachHang}");
                _logger.LogInformation($"🔍 Debug - TongTien: {tongTien}");
                _logger.LogInformation($"🔍 Debug - NgayTao: {ngayTao}");
                
                // ✅ Log tất cả properties của hoaDon object để debug
                var properties = type.GetProperties();
                foreach (var prop in properties)
                {
                    var value = prop.GetValue(hoaDon);
                    _logger.LogInformation($"🔍 Debug - Property: {prop.Name} = {value}");
                }
                
                // ✅ Nếu email vẫn null, thử lấy từ KhachHang navigation property
                if (string.IsNullOrEmpty(emailKhachHang))
                {
                    _logger.LogInformation($"🔍 Debug - Email is empty, trying KhachHang navigation property");
                    var khachHangProperty = type.GetProperty("KhachHang");
                    if (khachHangProperty != null)
                    {
                        var khachHang = khachHangProperty.GetValue(hoaDon);
                        _logger.LogInformation($"🔍 Debug - KhachHang object: {khachHang}");
                        if (khachHang != null)
                        {
                            var khachHangType = khachHang.GetType();
                            // ✅ Sửa lại để lấy EmailCuaKhachHang thay vì Email
                            var emailProperty = khachHangType.GetProperty("EmailCuaKhachHang");
                            if (emailProperty != null)
                            {
                                emailKhachHang = emailProperty.GetValue(khachHang)?.ToString() ?? "";
                                _logger.LogInformation($"🔍 Debug - Email from KhachHang.EmailCuaKhachHang: {emailKhachHang}");
                                
                                // ✅ Nếu EmailCuaKhachHang vẫn null, thử lấy từ TaiKhoan
                                if (string.IsNullOrEmpty(emailKhachHang))
                                {
                                    _logger.LogInformation($"🔍 Debug - EmailCuaKhachHang is empty, trying TaiKhoan");
                                    var taiKhoanProperty = khachHangType.GetProperty("TaiKhoans");
                                    if (taiKhoanProperty != null)
                                    {
                                        var taiKhoans = taiKhoanProperty.GetValue(khachHang) as IEnumerable<object>;
                                        if (taiKhoans != null && taiKhoans.Any())
                                        {
                                            var firstTaiKhoan = taiKhoans.First();
                                            var taiKhoanType = firstTaiKhoan.GetType();
                                            var taiKhoanEmailProperty = taiKhoanType.GetProperty("Email");
                                            if (taiKhoanEmailProperty != null)
                                            {
                                                emailKhachHang = taiKhoanEmailProperty.GetValue(firstTaiKhoan)?.ToString() ?? "";
                                                _logger.LogInformation($"🔍 Debug - Email from TaiKhoan.Email: {emailKhachHang}");
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                _logger.LogWarning($"🔍 Debug - EmailCuaKhachHang property not found in KhachHang type: {khachHangType.Name}");
                            }
                        }
                        else
                        {
                            _logger.LogWarning($"🔍 Debug - KhachHang object is null");
                        }
                    }
                    else
                    {
                        _logger.LogWarning($"🔍 Debug - KhachHang property not found in HoaDon type: {type.Name}");
                    }
                }
                
                if (string.IsNullOrEmpty(emailKhachHang))
                {
                    _logger.LogWarning($"❌ Customer email is empty for order {hoaDonId}");
                    return;
                }

                var subject = $"📦 Cập nhật trạng thái đơn hàng #{hoaDonId.Substring(0, 8).ToUpper()} - FurryFriends Store";
                
                var emailBody = CreateCustomerStatusNotificationBody(hoaDonId, tenKhachHang, oldStatus, newStatus, tongTien, ngayTao);
                
                await SendEmailAsync(emailKhachHang, subject, emailBody);
                
                _logger.LogInformation($"✅ Customer notification sent for order {hoaDonId} - Status: {oldStatus} → {newStatus}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"❌ Error sending customer notification: {ex.Message}");
                throw;
            }
        }

        private async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"] ?? "smtp.gmail.com";
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
            var senderEmail = _configuration["EmailSettings:SenderEmail"] ?? "your-email@gmail.com";
            var senderPassword = _configuration["EmailSettings:SenderPassword"] ?? "your-app-password";
            var senderName = _configuration["EmailSettings:SenderName"] ?? "FurryFriends Store";

            using (var smtpClient = new SmtpClient(smtpServer))
            {
                smtpClient.Port = smtpPort;
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;

                using (var mailMessage = new MailMessage(senderEmail, toEmail))
                {
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Priority = MailPriority.Normal;

                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
        }

        private string CreateAdminOrderNotificationBody(string hoaDonId, string tenKhachHang, string soDienThoai, string emailKhachHang, decimal tongTien, DateTime ngayTao, IEnumerable<HoaDonChiTietViewModel> chiTiets)
        {
            var shortId = hoaDonId.Substring(0, 8).ToUpper();
            var tongTienStr = tongTien.ToString("N0");
            var ngayTaoStr = ngayTao.ToString("dd/MM/yyyy HH:mm");

            var productList = "";
            int stt = 1;
            foreach (var chiTiet in chiTiets)
            {
                var tenSanPham = chiTiet.TenSanPhamLucMua ?? "";
                var soLuong = chiTiet.SoLuong?.ToString() ?? "0";
                var gia = chiTiet.GiaLucMua?.ToString("N0") ?? "0";
                var thanhTien = (chiTiet.SoLuong ?? 0) * (chiTiet.GiaLucMua ?? 0);
                
                productList += $@"
                    <tr>
                        <td>{stt}</td>
                        <td>{tenSanPham}</td>
                        <td>{soLuong}</td>
                        <td>{gia} VNĐ</td>
                    </tr>";
                stt++;
            }

            return $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Thông báo đơn hàng mới</title>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; margin: 0; padding: 20px; background-color: #f4f4f4; }}
                        .container {{ max-width: 600px; margin: 0 auto; background: white; border-radius: 10px; overflow: hidden; box-shadow: 0 0 20px rgba(0,0,0,0.1); }}
                        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; }}
                        .header h1 {{ margin: 0; font-size: 24px; }}
                        .warning-banner {{ background: #fff3cd; border: 1px solid #ffeaa7; color: #856404; padding: 15px; margin: 20px; border-radius: 8px; display: flex; align-items: center; gap: 10px; }}
                        .warning-icon {{ font-size: 20px; }}
                        .content {{ padding: 20px; }}
                        .section {{ margin: 20px 0; padding: 20px; background: #f8f9fa; border-radius: 8px; border-left: 4px solid #667eea; }}
                        .section h3 {{ margin-top: 0; color: #667eea; display: flex; align-items: center; gap: 10px; }}
                        .section-icon {{ font-size: 18px; }}
                        .info-row {{ display: flex; justify-content: space-between; margin: 10px 0; }}
                        .info-label {{ font-weight: bold; color: #555; }}
                        .info-value {{ color: #333; }}
                        .total-amount {{ font-size: 18px; font-weight: bold; color: #667eea; }}
                        .product-table {{ width: 100%; border-collapse: collapse; margin: 15px 0; }}
                        .product-table th, .product-table td {{ padding: 10px; text-align: left; border-bottom: 1px solid #ddd; }}
                        .product-table th {{ background-color: #667eea; color: white; font-weight: bold; }}
                        .product-table tr:nth-child(even) {{ background-color: #f8f9fa; }}
                        .action-link {{ display: inline-block; background: #667eea; color: white; padding: 12px 24px; text-decoration: none; border-radius: 6px; margin: 20px 0; }}
                        .footer {{ background: #f8f9fa; padding: 20px; text-align: center; color: #666; border-top: 1px solid #ddd; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>🐾 FurryFriends Store</h1>
                            <p>Hệ thống quản lý đơn hàng</p>
                        </div>
                        
                        <div class='warning-banner'>
                            <span class='warning-icon'>⚠️</span>
                            <strong>Thông báo: Có đơn hàng mới cần xử lý!</strong>
                        </div>
                        
                        <div class='content'>
                            <div class='section'>
                                <h3><span class='section-icon'>📋</span>Thông tin đơn hàng</h3>
                                <div class='info-row'>
                                    <span class='info-label'>Mã đơn hàng:</span>
                                    <span class='info-value'>#{shortId}</span>
                                </div>
                                <div class='info-row'>
                                    <span class='info-label'>Ngày đặt:</span>
                                    <span class='info-value'>{ngayTaoStr}</span>
                                </div>
                                <div class='info-row'>
                                    <span class='info-label'>Tổng tiền:</span>
                                    <span class='info-value total-amount'>{tongTienStr} VNĐ</span>
                                </div>
                            </div>
                            
                            <div class='section'>
                                <h3><span class='section-icon'>👤</span>Thông tin khách hàng</h3>
                                <div class='info-row'>
                                    <span class='info-label'>Họ tên:</span>
                                    <span class='info-value'>{tenKhachHang}</span>
                                </div>
                                <div class='info-row'>
                                    <span class='info-label'>Số điện thoại:</span>
                                    <span class='info-value'>{soDienThoai}</span>
                                </div>
                                <div class='info-row'>
                                    <span class='info-label'>Email:</span>
                                    <span class='info-value'>{emailKhachHang}</span>
                                </div>
                            </div>
                            
                            <div class='section'>
                                <h3><span class='section-icon'>🛍️</span>Sản phẩm đã đặt</h3>
                                <table class='product-table'>
                                    <thead>
                                        <tr>
                                            <th>STT</th>
                                            <th>Sản phẩm</th>
                                            <th>Số lượng</th>
                                            <th>Đơn giá</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {productList}
                                    </tbody>
                                </table>
                            </div>
                            
                            <div class='info-row'>
                                <span class='info-label'>Link quản lý:</span>
                                <a href='#' class='action-link'>🔗 Xem chi tiết đơn hàng</a>
                            </div>
                            
                            <p style='text-align: center; color: #856404; font-weight: bold;'>
                                Vui lòng xử lý đơn hàng này sớm nhất có thể!
                            </p>
                        </div>
                        
                        <div class='footer'>
                            <p>Trân trọng,</p>
                            <p><strong>Hệ thống FurryFriends Store</strong> 🐾</p>
                        </div>
                    </div>
                </body>
                </html>";
        }

        private string CreateCustomerStatusNotificationBody(string hoaDonId, string tenKhachHang, string oldStatus, string newStatus, object tongTien, object ngayTao)
        {
            var shortId = hoaDonId.Substring(0, 8).ToUpper();
            var tongTienStr = tongTien is decimal tt ? tt.ToString("N0") : "0";
            var ngayTaoStr = ngayTao is DateTime date ? date.ToString("dd/MM/yyyy HH:mm") : "N/A";

            var statusIcon = newStatus switch
            {
                "Đã duyệt" => "✅",
                "Đang vận chuyển" => "🚚",
                "Đã giao hàng" => "🎉",
                "Đã hủy" => "❌",
                _ => "📋"
            };

            var statusColor = newStatus switch
            {
                "Đã duyệt" => "#28a745",
                "Đang vận chuyển" => "#17a2b8",
                "Đã giao hàng" => "#28a745",
                "Đã hủy" => "#dc3545",
                _ => "#6c757d"
            };

            return $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8'>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 800px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
                        .content {{ background: #f8f9fa; padding: 30px; border-radius: 0 0 10px 10px; }}
                        .status-badge {{ display: inline-block; padding: 8px 16px; border-radius: 20px; font-size: 14px; font-weight: bold; text-transform: uppercase; }}
                        .highlight {{ color: #667eea; font-weight: bold; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h2>📦 Cập Nhật Đơn Hàng</h2>
                            <p>Mã đơn hàng: #{shortId}</p>
                        </div>
                        <div class='content'>
                            <p>Xin chào <strong>{tenKhachHang}</strong>,</p>
                            
                            <p>Đơn hàng của bạn đã được cập nhật trạng thái:</p>
                            
                            <div style='text-align: center; margin: 30px 0;'>
                                <div class='status-badge' style='background-color: {statusColor}; color: white;'>
                                    {statusIcon} {newStatus}
                                </div>
                            </div>
                            
                            <h3 style='color: #667eea;'>📋 Thông tin đơn hàng</h3>
                            <p><strong>Mã đơn hàng:</strong> #{shortId}</p>
                            <p><strong>Ngày đặt:</strong> {ngayTaoStr}</p>
                            <p><strong>Tổng tiền:</strong> <span class='highlight'>{tongTienStr} VNĐ</span></p>
                            <p><strong>Trạng thái cũ:</strong> {oldStatus}</p>
                            <p><strong>Trạng thái mới:</strong> <span style='color: {statusColor}; font-weight: bold;'>{newStatus}</span></p>
                            
                            {GetStatusSpecificMessage(newStatus)}
                            
                            <p style='margin-top: 30px;'>
                                <strong>🔗 Theo dõi đơn hàng:</strong> 
                                <a href='https://localhost:7102/DonHang' style='color: #667eea;'>Xem chi tiết đơn hàng</a>
                            </p>
                            
                            <p>Nếu bạn có bất kỳ câu hỏi nào, vui lòng liên hệ với chúng tôi:</p>
                            <ul>
                                <li>📞 Hotline: <strong>0968596808</strong></li>
                                <li>📧 Email: <strong>info@furryfriends.vn</strong></li>
                            </ul>
                            
                            <p>Cảm ơn bạn đã tin tưởng FurryFriends!</p>
                            
                            <p>Trân trọng,<br>
                            <strong>Đội ngũ FurryFriends Store</strong> 🐾</p>
                        </div>
                    </div>
                </body>
                </html>
            ";
        }

        private string GetStatusSpecificMessage(string status)
        {
            return status switch
            {
                "Đã duyệt" => @"
                    <div style='background: #d4edda; border: 1px solid #c3e6cb; color: #155724; padding: 15px; border-radius: 8px; margin: 20px 0;'>
                        <strong>✅ Đơn hàng đã được duyệt!</strong><br>
                        Chúng tôi đã xác nhận đơn hàng của bạn và sẽ chuẩn bị giao hàng sớm nhất.
                    </div>",
                "Đang vận chuyển" => @"
                    <div style='background: #d1ecf1; border: 1px solid #bee5eb; color: #0c5460; padding: 15px; border-radius: 8px; margin: 20px 0;'>
                        <strong>🚚 Đơn hàng đang được vận chuyển!</strong><br>
                        Đơn hàng của bạn đã được giao cho đơn vị vận chuyển và sẽ đến trong thời gian sớm nhất.
                    </div>",
                "Đã giao hàng" => @"
                    <div style='background: #d4edda; border: 1px solid #c3e6cb; color: #155724; padding: 15px; border-radius: 8px; margin: 20px 0;'>
                        <strong>🎉 Đơn hàng đã được giao thành công!</strong><br>
                        Cảm ơn bạn đã mua sắm tại FurryFriends Store. Chúng tôi mong được phục vụ bạn lần sau!
                    </div>",
                "Đã hủy" => @"
                    <div style='background: #f8d7da; border: 1px solid #f5c6cb; color: #721c24; padding: 15px; border-radius: 8px; margin: 20px 0;'>
                        <strong>❌ Đơn hàng đã được hủy!</strong><br>
                        Đơn hàng của bạn đã được hủy theo yêu cầu. Nếu có thắc mắc, vui lòng liên hệ với chúng tôi.
                    </div>",
                _ => @"
                    <div style='background: #fff3cd; border: 1px solid #ffeaa7; color: #856404; padding: 15px; border-radius: 8px; margin: 20px 0;'>
                        <strong>📋 Trạng thái đơn hàng đã được cập nhật!</strong><br>
                        Đơn hàng của bạn đã được cập nhật trạng thái mới.
                    </div>"
            };
        }
    }
}
