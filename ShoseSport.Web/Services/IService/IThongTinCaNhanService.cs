using ShoseSport.API.Models.DTO;
using ShoseSport.Web.Models;

namespace ShoseSport.Web.Services.IService
{
	public interface IThongTinCaNhanService
	{
		Task<ThongTinCaNhanViewModel?> GetThongTinCaNhanAsync(Guid taiKhoanId);
		Task<bool> UpdateThongTinCaNhanAsync(Guid taiKhoanId, ThongTinCaNhanViewModel dto);
		Task<bool> DoiMatKhauAsync(Guid taiKhoanId, string matKhauCu, string matKhauMoi);
		Task<List<DiaChiKhachHangViewModel>> GetDanhSachDiaChiAsync(Guid taiKhoanId);
	}
}
