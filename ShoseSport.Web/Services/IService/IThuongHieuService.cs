using ShoseSport.API.Models;
using ShoseSport.API.Models.DTO;

namespace ShoseSport.Web.Services.IService
{
    public interface IThuongHieuService
    {
        Task<IEnumerable<ThuongHieuDTO>> GetAllAsync();
        Task<ThuongHieuDTO> GetByIdAsync(Guid id);
        Task<ApiResult<ThuongHieuDTO>> CreateAsync(ThuongHieuDTO dto);
        Task<ApiResult<bool>> UpdateAsync(Guid id, ThuongHieuDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
