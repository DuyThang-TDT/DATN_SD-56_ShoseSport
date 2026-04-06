using ShoseSport.API.Models;
using ShoseSport.API.Models.DTO;

namespace ShoseSport.Web.Services.IService
{
    public interface IKichCoService
    {
        Task<IEnumerable<KichCoDTO>> GetAllAsync();
        Task<KichCoDTO> GetByIdAsync(Guid id);
        Task<ApiResult<KichCoDTO>> CreateAsync(KichCoDTO dto);
        Task<ApiResult<bool>> UpdateAsync(Guid id, KichCoDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
