using ShoseSport.API.Models;
using ShoseSport.API.Models.DTO;
using ShoseSport.Web.Filter;
using ShoseSport.Web.Services.IService;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Channels;

namespace ShoseSport.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthorizeAdminOnly]
    public class TaiKhoanController : Controller
    {
        public readonly ITaiKhoanService _taiKhoanService;
        private readonly IThongBaoService _thongBaoService;
        private readonly IKhachHangService _khachHangService;
        private readonly INhanVienService _nhanVienService;

        public TaiKhoanController(
            ITaiKhoanService taiKhoanService, 
            IThongBaoService thongBaoService,
            IKhachHangService khachHangService,
            INhanVienService nhanVienService)
        {
            _taiKhoanService = taiKhoanService;
            _thongBaoService = thongBaoService;
            _khachHangService = khachHangService;
            _nhanVienService = nhanVienService;
        }

        public async Task<IActionResult> Index()
        {
            var allTaiKhoans = await _taiKhoanService.GetAllAsync();
            ViewBag.TotalCount = allTaiKhoans.Count();
            ViewBag.ActiveCount = allTaiKhoans.Count(x => x.TrangThai);
            ViewBag.InactiveCount = allTaiKhoans.Count(x => !x.TrangThai);
            return View(allTaiKhoans);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    taiKhoan.TaiKhoanId = Guid.NewGuid();
                    taiKhoan.NgayTaoTaiKhoan = DateTime.Now;
                    taiKhoan.TrangThai = taiKhoan.TrangThai;
                    taiKhoan.KhachHang = null;
                    taiKhoan.KhachHangId = null;

                    await _taiKhoanService.AddAsync(taiKhoan);

                    var userName = HttpContext.Session.GetString("HoTen") ?? "Hệ thống";
                    await _thongBaoService.CreateAsync(new ThongBaoDTO
                    {
                        TieuDe = "Tạo tài khoản",
                        NoiDung = $"Tài khoản '{taiKhoan.UserName}' đã được tạo thành công.",
                        Loai = "TaiKhoan",
                        UserName = userName,
                        NgayTao = DateTime.Now,
                        DaDoc = false
                    });

                    TempData["Success"] = "Tài khoản đã được tạo thành công.";
                    return RedirectToAction(nameof(Index));
                }
                catch (ValidationException ex)
                {
                    var problemDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<ValidationProblemDetails>(ex.Message);
                    if (problemDetails?.Errors != null)
                    {
                        foreach (var error in problemDetails.Errors)
                        {
                            foreach (var msg in error.Value)
                            {
                                ModelState.AddModelError(error.Key, msg);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Lỗi xác thực.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Lỗi: {ex.Message}");
                }
            }

            return View(taiKhoan);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var taiKhoan = await _taiKhoanService.GetByIdAsync(id);
            if (taiKhoan == null)
            {
                return NotFound();
            }
            return View(taiKhoan);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, TaiKhoan taiKhoan)
        {
            if (id != taiKhoan.TaiKhoanId)
                return BadRequest("ID không khớp.");

            if (ModelState.IsValid)
            {
                try
                {
                    var taiKhoanCu = await _taiKhoanService.GetByIdAsync(id);
                    if (string.IsNullOrWhiteSpace(taiKhoan.Password) && taiKhoanCu != null)
                    {
                        taiKhoan.Password = taiKhoanCu.Password;
                    }

                    // Giữ nguyên liên kết cũ khi chỉ thay đổi trạng thái
                    if (taiKhoan.TrangThai != taiKhoanCu.TrangThai) // Chỉ thay đổi trạng thái
                    {
                        // Giữ nguyên KhachHangId và NhanVienId cũ
                        taiKhoan.KhachHangId = taiKhoanCu.KhachHangId;
                        taiKhoan.NhanVienId = taiKhoanCu.NhanVienId;
                    }

                    // Logic mới: Khi khóa tài khoản thì cũng khóa luôn khách hàng/nhân viên liên kết
                    if (!taiKhoan.TrangThai && taiKhoanCu.TrangThai) // Đang khóa tài khoản
                    {
                        // Khóa khách hàng liên kết
                        if (taiKhoanCu.KhachHangId.HasValue)
                        {
                            try
                            {
                                var khachHang = await _khachHangService.GetByIdAsync(taiKhoanCu.KhachHangId.Value);
                                if (khachHang != null)
                                {
                                    khachHang.TrangThai = 2; // 2 = Đã khóa
                                    await _khachHangService.UpdateAsync(khachHang.KhachHangId, khachHang);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error updating KhachHang status: {ex.Message}");
                            }
                        }

                        // Khóa nhân viên liên kết
                        if (taiKhoanCu.NhanVienId.HasValue)
                        {
                            try
                            {
                                var nhanVien = await _nhanVienService.GetByIdAsync(taiKhoanCu.NhanVienId.Value);
                                if (nhanVien != null)
                                {
                                    nhanVien.TrangThai = false;
                                    await _nhanVienService.UpdateAsync(nhanVien);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error updating NhanVien status: {ex.Message}");
                            }
                        }
                    }

                    await _taiKhoanService.UpdateAsync(taiKhoan);

                    // Đồng bộ trạng thái: Mở khóa khách hàng/nhân viên liên kết khi mở khóa tài khoản
                    if (taiKhoan.TrangThai && !taiKhoanCu.TrangThai) // Đang mở khóa tài khoản
                    {
                        // Sử dụng liên kết cũ để đồng bộ trạng thái
                        if (taiKhoanCu.KhachHangId.HasValue)
                        {
                            try
                            {
                                var khachHang = await _khachHangService.GetByIdAsync(taiKhoanCu.KhachHangId.Value);
                                if (khachHang != null)
                                {
                                    khachHang.TrangThai = 1; // 1 = Đang hoạt động
                                    await _khachHangService.UpdateAsync(khachHang.KhachHangId, khachHang);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error updating KhachHang status: {ex.Message}");
                            }
                        }

                        if (taiKhoanCu.NhanVienId.HasValue)
                        {
                            try
                            {
                                var nhanVien = await _nhanVienService.GetByIdAsync(taiKhoanCu.NhanVienId.Value);
                                if (nhanVien != null)
                                {
                                    nhanVien.TrangThai = true;
                                    await _nhanVienService.UpdateAsync(nhanVien);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error updating NhanVien status: {ex.Message}");
                            }
                        }
                    }

                    var userNameSession = HttpContext.Session.GetString("HoTen") ?? "Hệ thống";
                    await _thongBaoService.CreateAsync(new ThongBaoDTO
                    {
                        TieuDe = "Cập nhật tài khoản",
                        NoiDung = $"Tài khoản '{taiKhoan.UserName}' đã được cập nhật",
                        Loai = "TaiKhoan",
                        UserName = userNameSession,
                        NgayTao = DateTime.Now,
                        DaDoc = false
                    });

                    TempData["Success"] = "Tài khoản đã được cập nhật thành công.";
                    return RedirectToAction(nameof(Index));
                }
                catch (ValidationException ex)
                {
                    var problemDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<ValidationProblemDetails>(ex.Message);
                    if (problemDetails?.Errors != null)
                    {
                        foreach (var error in problemDetails.Errors)
                        {
                            foreach (var msg in error.Value)
                            {
                                ModelState.AddModelError(error.Key, msg);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Lỗi xác thực.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Lỗi: {ex.Message}");
                }
            }
            return View(taiKhoan);
        }

        // POST: /TaiKhoan/ToggleStatus/{id}
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(Guid id)
        {
            try
            {
                var taiKhoan = await _taiKhoanService.GetByIdAsync(id);
                if (taiKhoan == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy tài khoản." });
                }

                // Toggle trạng thái
                taiKhoan.TrangThai = !taiKhoan.TrangThai;
                await _taiKhoanService.UpdateAsync(taiKhoan);
                
                // Đồng bộ trạng thái với khách hàng hoặc nhân viên liên kết
                if (taiKhoan.KhachHangId.HasValue)
                {
                    try
                    {
                        var khachHang = await _khachHangService.GetByIdAsync(taiKhoan.KhachHangId.Value);
                        if (khachHang != null)
                        {
                            // Đồng bộ trạng thái: tài khoản hoạt động thì khách hàng cũng hoạt động
                            khachHang.TrangThai = taiKhoan.TrangThai ? 1 : 2; // 1 = hoạt động, 2 = đã khóa
                            await _khachHangService.UpdateAsync(khachHang.KhachHangId, khachHang);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error updating KhachHang status: {ex.Message}");
                    }
                }
                else if (taiKhoan.NhanVienId.HasValue)
                {
                    try
                    {
                        var nhanVien = await _nhanVienService.GetByIdAsync(taiKhoan.NhanVienId.Value);
                        if (nhanVien != null)
                        {
                            // Đồng bộ trạng thái: tài khoản hoạt động thì nhân viên cũng hoạt động
                            nhanVien.TrangThai = taiKhoan.TrangThai;
                            await _nhanVienService.UpdateAsync(nhanVien);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error updating NhanVien status: {ex.Message}");
                    }
                }
                
                var action = taiKhoan.TrangThai ? "kích hoạt" : "vô hiệu hóa";
                var message = $"Tài khoản '{taiKhoan.UserName}' đã được {action} thành công.";

                // 🔔 Thêm thông báo
                var userName = HttpContext.Session.GetString("HoTen") ?? "Hệ thống";
                await _thongBaoService.CreateAsync(new ThongBaoDTO
                {
                    TieuDe = taiKhoan.TrangThai ? "Kích hoạt tài khoản" : "Vô hiệu hóa tài khoản",
                    NoiDung = $"Tài khoản '{taiKhoan.UserName}' đã được {action}",
                    Loai = "TaiKhoan",
                    UserName = userName,
                    NgayTao = DateTime.Now,
                    DaDoc = false
                });

                return Json(new { 
                    success = true, 
                    message = message,
                    newStatus = taiKhoan.TrangThai,
                    statusText = taiKhoan.TrangThai ? "Đang hoạt động" : "Không hoạt động",
                    statusClass = taiKhoan.TrangThai ? "bg-success" : "bg-secondary"
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Search(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return RedirectToAction(nameof(Index));

            try
            {
                var taiKhoans = await _taiKhoanService.FindByUserNameAsync(userName);

                if (taiKhoans == null || !taiKhoans.Any())
                {
                    TempData["Error"] = "Không tìm thấy tài khoản.";
                    return RedirectToAction(nameof(Index));
                }

                return View("Index", taiKhoans);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Index", new List<TaiKhoan>());
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Index", new List<TaiKhoan>());
            }
        }
    }
}