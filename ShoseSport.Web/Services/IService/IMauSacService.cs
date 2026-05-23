using ShoseSport.API.Models;
using ShoseSport.API.Models.DTO;

namespace ShoseSport.Web.Services.IService
{
    public interface IMauSacService
    {
        Task<IEnumerable<MauSacDTO>> GetAllAsync();
        Task<MauSacDTO> GetByIdAsync(Guid id);
        Task<ApiResult<MauSacDTO>> CreateAsync(MauSacDTO dto);
        Task<ApiResult<bool>> UpdateAsync(Guid id, MauSacDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
