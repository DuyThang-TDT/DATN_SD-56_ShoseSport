using FurryFriends.Web.Models;
using FurryFriends.Web.Services.IService;
using Microsoft.AspNetCore.Mvc;
using FurryFriends.API.Models;
using LoginRequest = FurryFriends.API.Models.LoginRequest;
using Microsoft.Extensions.Logging;

public class KhachHangLoginController : Controller
{
    private readonly ITaiKhoanService _taiKhoanService;
    private readonly ILogger<KhachHangLoginController> _logger;

    public KhachHangLoginController(ITaiKhoanService taiKhoanService, ILogger<KhachHangLoginController> logger)
    {
        _taiKhoanService = taiKhoanService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult DangNhap()
    {
        // Xóa TempData cũ để tránh hiển thị thông báo không mong muốn
        TempData.Clear();
        
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("TaiKhoanId")))
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> DangNhap(LoginRequest model)
    {
        if (!ModelState.IsValid) 
        {
            TempData["Error"] = "Vui lòng kiểm tra lại thông tin đăng nhập!";
            return View(model); // Return view instead of redirect
        }

        _logger.LogInformation($"Khách hàng đăng nhập với UserName: {model.UserName}");

        try
        {
            var result = await _taiKhoanService.DangNhapKhachHangAsync(model);
            _logger.LogInformation($"Kết quả đăng nhập khách hàng: Thành công");

            // Lưu session
            HttpContext.Session.SetString("TaiKhoanId", result.TaiKhoanId.ToString());
            HttpContext.Session.SetString("KhachHangId", result.KhachHangId.ToString());
            HttpContext.Session.SetString("Role", result.Role);
            HttpContext.Session.SetString("HoTen", result.HoTen ?? "");

            TempData["Success"] = $"Đăng nhập thành công! Xin chào {result.HoTen} 🎉";
            return RedirectToAction("Index", "Home");
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning($"Đăng nhập thất bại: {ex.Message}");
            TempData["Error"] = ex.Message;
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Lỗi đăng nhập: {ex.Message}");
            TempData["Error"] = "Sai tên đăng nhập hoặc mật khẩu. Vui lòng kiểm tra lại!";
            return View(model);
        }
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        TempData["Success"] = "Đăng xuất thành công! Hẹn gặp lại bạn! 👋";
        return RedirectToAction("Index", "Home");
    }
}