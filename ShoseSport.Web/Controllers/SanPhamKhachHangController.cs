using FurryFriends.Web.Services.IService;
using FurryFriends.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FurryFriends.Web.Controllers
{
    public class SanPhamKhachHangController : Controller
    {
        private readonly ISanPhamService _sanPhamService;

        public SanPhamKhachHangController(ISanPhamService sanPhamService)
        {
            _sanPhamService = sanPhamService;
        }

        public async Task<IActionResult> Index(string? tuKhoa, Guid? thuongHieuId, string? khoangGia, string? sapXep)
        {
            var danhSachSanPham = await _sanPhamService.GetAllAsync();

            var viewModelList = danhSachSanPham.Select(sp => new SanPhamViewModel
            {
                SanPhamId = sp.SanPhamId,
                TenSanPham = sp.TenSanPham,
                TenThuongHieu = sp.TenThuongHieu,
                ThuongHieuId = sp.ThuongHieuId,
                TrangThai = sp.TrangThai,
                GiaBan = sp.SanPhamChiTiets?.FirstOrDefault()?.Gia ?? 0,
                SoLuongTon = sp.SanPhamChiTiets?.FirstOrDefault()?.SoLuong ?? 0,
                AnhDaiDienUrl = sp.SanPhamChiTiets?.FirstOrDefault()?.DuongDan
                                ?? "https://via.placeholder.com/400x300",
                CoGiamGia = false
            }).ToList();

            if (!string.IsNullOrEmpty(tuKhoa))
                viewModelList = viewModelList
                    .Where(x => x.TenSanPham.Contains(tuKhoa, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            if (thuongHieuId.HasValue)
                viewModelList = viewModelList
                    .Where(x => x.ThuongHieuId == thuongHieuId)
                    .ToList();

            ViewBag.TuKhoa = tuKhoa;
            ViewBag.ThuongHieuId = thuongHieuId;
            ViewBag.KhoangGia = khoangGia;
            ViewBag.SapXep = sapXep;

            return View(viewModelList);
        }

        public async Task<IActionResult> ChiTiet(Guid id)
        {
            var sanPham = await _sanPhamService.GetByIdAsync(id);
            if (sanPham == null) return NotFound();

            var viewModel = new SanPhamViewModel
            {
                SanPhamId = sanPham.SanPhamId,
                TenSanPham = sanPham.TenSanPham,
                TenThuongHieu = sanPham.TenThuongHieu,
                ThuongHieuId = sanPham.ThuongHieuId,
                TrangThai = sanPham.TrangThai,
                GiaBan = sanPham.SanPhamChiTiets?.FirstOrDefault()?.Gia ?? 0,
                SoLuongTon = sanPham.SanPhamChiTiets?.FirstOrDefault()?.SoLuong ?? 0,
                AnhDaiDienUrl = "https://via.placeholder.com/400x300",
                CoGiamGia = false
            };

            return View("Detail", viewModel);
        }
    }
}