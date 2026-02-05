using FurryFriends.API.Models.DTO;
using FurryFriends.Web.Services.IService;
using FurryFriends.Web.Services;
using FurryFriends.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using FurryFriends.Web.Filter;

namespace FurryFriends.Web.Controllers
{
    public class SanPhamKhachHangController : Controller
    {
        private readonly ISanPhamService _sanPhamService;
        private readonly ISanPhamChiTietService _sanPhamChiTietService;
        private readonly DiscountCalculationService _discountCalculationService;

        public SanPhamKhachHangController(
            ISanPhamService sanPhamService, 
            ISanPhamChiTietService sanPhamChiTietService,
            DiscountCalculationService discountCalculationService)
        {
            _sanPhamService = sanPhamService;
            _sanPhamChiTietService = sanPhamChiTietService;
            _discountCalculationService = discountCalculationService;
        }

        // Hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index(string? tuKhoa, Guid? thuongHieuId, string? khoangGia, string? sapXep)
        {
            var danhSachSanPhamDTO = await _sanPhamService.GetAllAsync();
            var viewModelList = new List<SanPhamViewModel>();

            // Lấy toàn bộ chi tiết một lần
            var allChiTietListDTO = await _sanPhamChiTietService.GetAllAsync();

            foreach (var sp in danhSachSanPhamDTO)
            {
                // Bỏ qua sản phẩm không hoạt động
                if (!sp.TrangThai) continue;

                // loc theo tu khoa
                if (!string.IsNullOrEmpty(tuKhoa) && !sp.TenSanPham.Contains(tuKhoa, StringComparison.OrdinalIgnoreCase))
                    continue;

                // loc theo thuong hieu
                if (thuongHieuId.HasValue && sp.ThuongHieuId != thuongHieuId.Value)
                    continue;

                // Lọc chi tiết theo sản phẩm
                var chiTietListDTO = allChiTietListDTO
                    .Where(ct => ct.SanPhamId == sp.SanPhamId
                              && ct.SoLuong > 0
                              && ct.TrangThai == 1) // ✅ chỉ lấy chi tiết còn hàng & hoạt động
                    .ToList();

                if (!chiTietListDTO.Any())
                    continue; // ✅ nếu không có chi tiết hợp lệ thì bỏ luôn sp

                // loc theo gia
                decimal minPrice = 0;
                decimal maxPrice = decimal.MaxValue;
                if (!string.IsNullOrEmpty(khoangGia))
                {
                    switch (khoangGia)
                    {
                        case "duoi-1-trieu":
                            maxPrice = 1000000;
                            break;
                        case "1-2-trieu":
                            minPrice = 1000000;
                            maxPrice = 2000000;
                            break;
                        case "tren-2-trieu":
                            minPrice = 2000000;
                            break;
                    }
                }

                // Check if any variant is within price range (can be optimized but checking all variants is safer for display)
                bool isInPriceRange = chiTietListDTO.Any(ct => ct.Gia >= minPrice && ct.Gia <= maxPrice);
                if (!isInPriceRange) continue;


                string? anhDaiDien = chiTietListDTO
                    .FirstOrDefault(ct => !string.IsNullOrEmpty(ct.DuongDan))
                    ?.DuongDan;
                
                // Fix image path if it's relative
                if (!string.IsNullOrEmpty(anhDaiDien) && !anhDaiDien.StartsWith("http"))
                {
                    anhDaiDien = $"https://localhost:7289{anhDaiDien}";
                }

                // Chuyển sang ViewModel chi tiết
                var chiTietVMs = chiTietListDTO.Select(ct => {
                    var imgs = ct.DuongDan != null ? new List<string> { ct.DuongDan } : new List<string>();
                    // Fix image paths in detail list
                    imgs = imgs.Select(img => !img.StartsWith("http") ? $"https://localhost:7289{img}" : img).ToList();

                    return new SanPhamChiTietViewModel
                    {
                        SanPhamChiTietId = ct.SanPhamChiTietId,
                        MauSac = ct.TenMau ?? "",
                        KichCo = ct.TenKichCo ?? "",
                        SoLuongTon = ct.SoLuong,
                        GiaBan = ct.Gia,
                        DanhSachAnh = imgs,

                        // Thông tin giảm giá sẽ cập nhật sau
                        CoGiamGia = false,
                        PhanTramGiamGia = null,
                        GiaSauGiam = null
                    };
                }).ToList();

                var sanPhamVM = new SanPhamViewModel
                {
                    SanPhamId = sp.SanPhamId,
                    TenSanPham = sp.TenSanPham,
                    MoTa = sp.TenThuongHieu, 
                    TrangThai = sp.TrangThai,
                    AnhDaiDienUrl = anhDaiDien,
                    SanPhamChiTietId = chiTietListDTO.First().SanPhamChiTietId,
                    GiaBan = chiTietListDTO.Min(ct => ct.Gia), 
                    SoLuongTon = chiTietListDTO.Sum(ct => ct.SoLuong), 
                    TenThuongHieu = sp.TenThuongHieu,
                    ThuongHieuId = sp.ThuongHieuId,
                    ChiTietList = chiTietVMs
                };

                // Áp dụng logic giảm giá
                sanPhamVM = await _discountCalculationService.UpdateProductDiscount(sanPhamVM);

                viewModelList.Add(sanPhamVM);
            }

            // Sap xep
            if (!string.IsNullOrEmpty(sapXep))
            {
                switch (sapXep)
                {
                    case "gia-tang":
                        viewModelList = viewModelList.OrderBy(x => x.GiaBan).ToList();
                        break;
                    case "gia-giam":
                        viewModelList = viewModelList.OrderByDescending(x => x.GiaBan).ToList();
                        break;
                    // case "moi-nhat": // Cần thêm field NgayTao vào ViewModel nếu muốn sort
                    //    viewModelList = viewModelList.OrderByDescending(x => x.NgayTao).ToList();
                    //    break;
                }
            }

            var khachHangId = HttpContext.Session.GetString("KhachHangId");
            ViewBag.KhachHangId = khachHangId;
            
            // Pass filter values back to view
            ViewBag.TuKhoa = tuKhoa;
            ViewBag.ThuongHieuId = thuongHieuId;
            ViewBag.KhoangGia = khoangGia;
            ViewBag.SapXep = sapXep;

            return View(viewModelList);
        }


        // Hiển thị chi tiết sản phẩm
        public async Task<IActionResult> ChiTiet(Guid id)
        {
            var sp = await _sanPhamService.GetByIdAsync(id);
            if (sp == null) return NotFound();

            var chiTietListDTO = (await _sanPhamChiTietService.GetAllAsync())
                                    .Where(ct => ct.SanPhamId == id)
                                    .ToList();

            var chiTietViewModels = chiTietListDTO.Select(ct => new SanPhamChiTietViewModel
            {
                SanPhamChiTietId = ct.SanPhamChiTietId,
                MauSac = ct.TenMau ?? "",
                KichCo = ct.TenKichCo ?? "",
                SoLuongTon = ct.SoLuong,
                GiaBan = ct.Gia,
                DanhSachAnh = ct.DuongDan != null ? new List<string> { ct.DuongDan } : new List<string>(),
                
                // Thông tin giảm giá sẽ được tính toán sau
                CoGiamGia = false,
                PhanTramGiamGia = null,
                GiaSauGiam = null
            }).ToList();

            var vm = new SanPhamViewModel
            {
                SanPhamId = sp.SanPhamId,
                TenSanPham = sp.TenSanPham,
                MoTa = sp.TenThuongHieu ?? "", // Đây là mô tả, không phải tên thương hiệu
                TrangThai = sp.TrangThai,
                AnhDaiDienUrl = chiTietListDTO.FirstOrDefault()?.DuongDan,
                GiaBan = chiTietListDTO.FirstOrDefault()?.Gia ?? 0,
                SoLuongTon = chiTietListDTO.FirstOrDefault()?.SoLuong ?? 0,
                
                // Thông tin thương hiệu
                TenThuongHieu = sp.TenThuongHieu, // Tên thương hiệu
                ThuongHieuId = sp.ThuongHieuId,
                
                ChiTietList = chiTietViewModels
            };

            // Áp dụng logic giảm giá với % cao nhất
            vm = await _discountCalculationService.UpdateProductDiscount(vm);

            return View("Details", vm);
        }
    }
}
