using ShoseSport.Web.Services.IService;
using ShoseSport.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ShoseSport.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PhieuHoanTraAdminController : Controller
    {
        private readonly IPhieuHoanTraService _service;

        public PhieuHoanTraAdminController(IPhieuHoanTraService service)
        {
            _service = service;
        }

        // Danh sách phiếu hoàn
        public async Task<IActionResult> Index(Guid? hoaDonId)
        {
            var list = await _service.GetAllAsync();
            ViewBag.HoaDonId = hoaDonId ?? Guid.Empty;
            return View(list);
        }

        // Xem chi tiết
        public async Task<IActionResult> Details(Guid id)
        {
            var phieu = await _service.GetByIdAsync(id);
            if (phieu == null) return NotFound();
            return View(phieu);
        }

        // Duyệt (chỉ đổi trạng thái)
        public async Task<IActionResult> Edit(Guid id)
        {
            var phieu = await _service.GetByIdAsync(id);
            if (phieu == null) return NotFound();

            // Trả về model edit chỉ để Hiển thị readonly + đổi trạng thái
            var vm = new PhieuHoanTraUpdateRequest
            {
                SoLuongHoan = phieu.SoLuongHoan,     // readonly ở view
                LyDoHoanTra = phieu.LyDoHoanTra,     // readonly ở view
                TrangThai = phieu.TrangThai        // only field to change
            };

            // Có thể cần hiển thị thêm thông tin ở ViewBag
            ViewBag.Header = phieu;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, PhieuHoanTraUpdateRequest request)
        {
            // Chỉ cho phép đổi trạng thái → đảm bảo không nhận thay đổi khác:
            var current = await _service.GetByIdAsync(id);
            if (current == null) return NotFound();

            // Khóa cứng các trường khác để tránh bị sửa ngoài ý muốn
            var toUpdate = new PhieuHoanTraUpdateRequest
            {
                SoLuongHoan = current.SoLuongHoan,
                LyDoHoanTra = current.LyDoHoanTra,
                TrangThai = request.TrangThai      // chỉ field cho phép
            };

            var ok = await _service.UpdateAsync(id, toUpdate);
            if (ok)
            {
                TempData["Success"] = "Cập nhật trạng thái phiếu hoàn thành công.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "Cập nhật thất bại.");
            ViewBag.Header = current;
            return View(request);
        }

        // ✅ Action AJAX duyệt phiếu hoàn trả và cộng tồn kho
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(Guid id)
        {
            var phieu = await _service.GetByIdAsync(id);
            if (phieu == null)
            {
                return Json(new { success = false, message = "Không tìm thấy phiếu hoàn trả." });
            }

            // Duyệt phiếu: nếu trạng thái khác 1 (đã duyệt) thì chuyển sang 1 (đã duyệt)
            int newTrangThai = phieu.TrangThai == 1 ? 0 : 1; 

            var updateRequest = new PhieuHoanTraUpdateRequest
            {
                SoLuongHoan = phieu.SoLuongHoan,
                LyDoHoanTra = phieu.LyDoHoanTra,
                TrangThai = newTrangThai
            };

            var ok = await _service.UpdateAsync(id, updateRequest);
            if (ok)
            {
                bool isApproved = newTrangThai == 1;
                return Json(new
                {
                    success = true,
                    newStatus = isApproved,
                    statusClass = isApproved ? "status-badge status-approved" : "status-badge status-pending",
                    statusText = isApproved ? "Đã duyệt" : "Chờ xử lý",
                    message = isApproved ? "Phê duyệt phiếu hoàn thành công! Số lượng sản phẩm đã được cộng trả lại vào kho." : "Đã chuyển phiếu về trạng thái chờ xử lý."
                });
            }

            return Json(new { success = false, message = "Không thể cập nhật trạng thái phiếu hoàn trả." });
        }

        // ✅ Cập nhật trạng thái phiếu (từ chối, duyệt...) từ Form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTrangThai(Guid id, int trangThai, Guid? hoaDonId)
        {
            var phieu = await _service.GetByIdAsync(id);
            if (phieu == null) return NotFound();

            var updateRequest = new PhieuHoanTraUpdateRequest
            {
                SoLuongHoan = phieu.SoLuongHoan,
                LyDoHoanTra = phieu.LyDoHoanTra,
                TrangThai = trangThai
            };

            var ok = await _service.UpdateAsync(id, updateRequest);
            if (ok)
            {
                TempData["Success"] = "Cập nhật trạng thái phiếu hoàn thành công.";
            }
            else
            {
                TempData["Error"] = "Cập nhật trạng thái phiếu hoàn thất bại.";
            }

            return RedirectToAction(nameof(Index), new { hoaDonId = hoaDonId });
        }

        // ✅ Xóa phiếu hoàn trả
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, Guid? hoaDonId)
        {
            var ok = await _service.DeleteAsync(id);
            if (ok)
            {
                TempData["Success"] = "Xóa phiếu hoàn trả thành công.";
            }
            else
            {
                TempData["Error"] = "Xóa phiếu hoàn trả thất bại.";
            }

            return RedirectToAction(nameof(Index), new { hoaDonId = hoaDonId });
        }
    }
}
