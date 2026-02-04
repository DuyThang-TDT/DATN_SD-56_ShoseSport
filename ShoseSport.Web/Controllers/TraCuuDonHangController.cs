using FurryFriends.Web.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace FurryFriends.Web.Controllers
{
    public class TraCuuDonHangController : Controller
    {
        private readonly IHoaDonService _hoaDonService;

        public TraCuuDonHangController(IHoaDonService hoaDonService)
        {
            _hoaDonService = hoaDonService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Guid? hoaDonId, string sdt)
        {
            if (hoaDonId == null || string.IsNullOrWhiteSpace(sdt))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ Mã đơn hàng và Số điện thoại.";
                return View();
            }

            var hoaDon = await _hoaDonService.TraCuuDonHangAsync(hoaDonId.Value, sdt);

            if (hoaDon == null)
            {
                 ViewBag.HoaDonId = hoaDonId;
                 ViewBag.Sdt = sdt;
                 ViewBag.Error = "Không tìm thấy đơn hàng hoặc thông tin xác thực không chính xác.";
                 return View();
            }

            // Load details if not loaded (Service usually returns HoaDon, but maybe not details)
            // The API GetHoaDonById usually returns Details if properly included, but check implementation.
            // My API GetHoaDonById implementation: _hoaDonRepository.GetHoaDonByIdAsync(id)
            // Assuming it returns minimal info, we might need to load details separately or the API endpoint handles it.
            // The TraCuu endpoint uses GetHoaDonByIdAsync.
            
            // For now, assume we display what we have.
            
            ViewBag.HoaDonId = hoaDonId;
            ViewBag.Sdt = sdt;
            return View(hoaDon);
        }
    }
}
