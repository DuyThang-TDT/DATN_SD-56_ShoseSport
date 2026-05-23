using ShoseSport.Web.Services.IService;
using Microsoft.AspNetCore.Mvc;
using ShoseSport.Web.Filter;

namespace ShoseSport.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthorizeAdminOnly]
    public class HinhThucThanhToansController : Controller
    {
        private readonly IHinhThucThanhToanService _hinhThucThanhToanService;

        public HinhThucThanhToansController(IHinhThucThanhToanService hinhThucThanhToanService)
        {
            _hinhThucThanhToanService = hinhThucThanhToanService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.HinhThucThanhToanList = await _hinhThucThanhToanService.GetAllAsync();
            // Các phần giỏ hàng khác
            return View();
        }

    }
}
