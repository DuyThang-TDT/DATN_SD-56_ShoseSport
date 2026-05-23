using ShoseSport.API.Models.DTO;
using ShoseSport.Web.Services.IService;
using ShoseSport.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoseSport.Web.Filter;
using ShoseSport.API.Models;
using Microsoft.AspNetCore.Http;

namespace ShoseSport.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthorizeEmployee]
    public class SanPhamController : Controller
    {
        private readonly ISanPhamService _sanPhamService;
        private readonly ISanPhamChiTietService _chiTietService;
        private readonly IAnhService _anhService;
        private readonly IThuongHieuService _thuongHieuService;
        private readonly IKichCoService _kichCoService;
        private readonly IMauSacService _mauSacService;
        private readonly IThanhPhanService _thanhPhanService;
        private readonly IChatLieuService _chatLieuService;
        private readonly IThongBaoService _thongBaoService;

        public SanPhamController(
            ISanPhamService sanPhamService,
            ISanPhamChiTietService chiTietService,
            IAnhService anhService,
            IThuongHieuService thuongHieuService,
            IKichCoService kichCoService,
            IMauSacService mauSacService,
            IThanhPhanService thanhPhanService,
            IChatLieuService chatLieuService,
            IThongBaoService thongBaoService)
        {
            _sanPhamService = sanPhamService;
            _chiTietService = chiTietService;
            _anhService = anhService;
            _thuongHieuService = thuongHieuService;
            _kichCoService = kichCoService;
            _mauSacService = mauSacService;
            _thanhPhanService = thanhPhanService;
            _chatLieuService = chatLieuService;
            _thongBaoService = thongBaoService;
        }

        // ================== INDEX ==================
        public async Task<IActionResult> Index()
        {
            try
            {
                var allSanPhams = await _sanPhamService.GetAllAsync();
                await LoadDropdownData();
                return View(allSanPhams);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi tải dữ liệu: {ex.Message}";
                return View(new List<SanPhamDTO>());
            }
        }

        // ================== CREATE GET ==================
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadDropdownData(isCreateMode: true);

            return View(new SanPhamFullCreateViewModel
            {
                SanPham = new SanPhamDTO { TrangThai = true },
                ChiTietList = new List<SanPhamChiTietCreateViewModel>() // vẫn giữ để tránh lỗi View
            });
        }

        // ================== CREATE POST ==================
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ReadOnly]
        public async Task<IActionResult> Create(SanPhamFullCreateViewModel model)
        {
            // ❌ BỎ ValidateChiTietList

            if (!ModelState.IsValid)
            {
                await LoadDropdownData(isCreateMode: true);
                return View(model);
            }

            // ✅ Chỉ tạo sản phẩm (API đã auto gen variant)
            var createResult = await _sanPhamService.CreateAsync(model.SanPham);

            if (!createResult.Success)
            {
                AddApiErrorsToModelState(createResult);
                await LoadDropdownData(isCreateMode: true);
                return View(model);
            }

            var createdSanPham = createResult.Data;

            TempData["Success"] = "Tạo sản phẩm thành công!";

            // 🔔 Thông báo
            await _thongBaoService.CreateAsync(new ThongBaoDTO
            {
                TieuDe = $"Thêm sản phẩm: {createdSanPham.TenSanPham}",
                NoiDung = $"Sản phẩm '{createdSanPham.TenSanPham}' đã được thêm vào hệ thống.",
                Loai = "SanPham",
                UserName = "admin",
                NgayTao = DateTime.Now,
                DaDoc = false
            });

            return RedirectToAction("Index");
        }

        // ================== EDIT GET ==================
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var sanPham = await _sanPhamService.GetByIdAsync(id);
            if (sanPham == null) return NotFound();

            var allChiTiet = await _chiTietService.GetAllAsync();

            var chiTietList = allChiTiet?
                .Where(x => x.SanPhamId == id)
                .Select(x => new SanPhamChiTietCreateViewModel
                {
                    SanPhamChiTietId = x.SanPhamChiTietId,
                    MauSacId = x.MauSacId,
                    KichCoId = x.KichCoId,
                    SoLuongTon = x.SoLuong,
                    GiaBan = x.Gia,
                    GiaNhap = x.GiaNhap,
                    MoTa = x.MoTa,
                    AnhId = x.AnhId,
                    DuongDan = x.DuongDan
                }).ToList() ?? new List<SanPhamChiTietCreateViewModel>();

            await LoadDropdownData(sanPham);

            return View(new SanPhamFullCreateViewModel
            {
                SanPham = sanPham,
                ChiTietList = chiTietList
            });
        }

        // ================== EDIT POST ==================
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ReadOnly]
        public async Task<IActionResult> Edit(SanPhamFullCreateViewModel model)
        {
            var oldSanPham = await _sanPhamService.GetByIdAsync(model.SanPham.SanPhamId);
            if (oldSanPham == null) return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadDropdownData(model.SanPham);
                return View(model);
            }

            var updateResult = await _sanPhamService.UpdateAsync(model.SanPham.SanPhamId, model.SanPham);

            if (!updateResult.Success)
            {
                AddApiErrorsToModelState(updateResult);
                await LoadDropdownData(model.SanPham);
                return View(model);
            }

            // ✔ Vẫn giữ xử lý variant ở EDIT
            await ProcessVariants(model.SanPham.SanPhamId, model.ChiTietList);

            TempData["Success"] = "Cập nhật sản phẩm thành công!";

            return RedirectToAction("Index");
        }

        // ================== DELETE ==================
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var model = await _sanPhamService.GetByIdAsync(id);
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [ReadOnly]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                var sanPham = await _sanPhamService.GetByIdAsync(id);
                if (sanPham == null)
                {
                    TempData["Error"] = "Không tìm thấy sản phẩm.";
                    return RedirectToAction(nameof(Index));
                }

                sanPham.TrangThai = false;
                var updateResult = await _sanPhamService.UpdateAsync(id, sanPham);

                if (updateResult.Success)
                {
                    TempData["Success"] = "Sản phẩm đã được vô hiệu hóa.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Error"] = "Xóa thất bại!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // ================== PROCESS VARIANT ==================
        private async Task ProcessVariants(Guid sanPhamId, List<SanPhamChiTietCreateViewModel> submittedVariants)
        {
            var existing = (await _chiTietService.GetAllAsync())
                .Where(x => x.SanPhamId == sanPhamId).ToList();

            var submittedIds = submittedVariants
                .Where(x => x.SanPhamChiTietId.HasValue)
                .Select(x => x.SanPhamChiTietId.Value)
                .ToList();

            foreach (var item in existing.Where(x => !submittedIds.Contains(x.SanPhamChiTietId)))
            {
                await _chiTietService.DeleteAsync(item.SanPhamChiTietId);
            }

            foreach (var item in submittedVariants)
            {
                var dto = new SanPhamChiTietDTO
                {
                    SanPhamId = sanPhamId,
                    MauSacId = item.MauSacId,
                    KichCoId = item.KichCoId,
                    SoLuong = item.SoLuongTon,
                    Gia = item.GiaBan,
                    GiaNhap = item.GiaNhap,
                    MoTa = item.MoTa,
                    AnhId = item.AnhId,
                    TrangThai = 1
                };

                if (item.SanPhamChiTietId.HasValue && item.SanPhamChiTietId != Guid.Empty)
                    await _chiTietService.UpdateAsync(item.SanPhamChiTietId.Value, dto);
                else
                    await _chiTietService.CreateAsync(dto);
            }
        }

        // ================== LOAD DROPDOWN ==================
        private async Task LoadDropdownData(SanPhamDTO? sanPham = null, bool isCreateMode = false)
        {
            ViewBag.KichCoList = new SelectList(await _kichCoService.GetAllAsync(), "KichCoId", "TenKichCo");
            ViewBag.MauSacList = new SelectList(await _mauSacService.GetAllAsync(), "MauSacId", "TenMau");
            ViewBag.AnhList = await _anhService.GetAllAsync();
            ViewBag.ThuongHieuList = new SelectList(await _thuongHieuService.GetAllAsync(), "ThuongHieuId", "TenThuongHieu");
        }

        private void AddApiErrorsToModelState<T>(ApiResult<T> result)
        {
            if (result.Errors != null)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Key, string.Join(", ", error.Value));
                }
            }
        }
    }
}