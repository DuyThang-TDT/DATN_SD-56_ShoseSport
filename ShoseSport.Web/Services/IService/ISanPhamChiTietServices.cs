using ShoseSport.API.Models;
using ShoseSport.API.Models.DTO;

namespace ShoseSport.Web.Services.IService
{
    public interface ISanPhamChiTietService
    {
        Task<IEnumerable<SanPhamChiTietDTO>> GetAllAsync();
        Task<SanPhamChiTietDTO?> GetByIdAsync(Guid id);
        Task<ApiResult<SanPhamChiTietDTO>> CreateAsync(SanPhamChiTietDTO dto);
        Task<ApiResult<bool>> UpdateAsync(Guid id, SanPhamChiTietDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
