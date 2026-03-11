using ShoseSport.API.Models;
using ShoseSport.API.Models.DTO;

namespace ShoseSport.Web.Services.IService
{
    public interface IChatLieuService
    {
        Task<IEnumerable<ChatLieuDTO>> GetAllAsync();
        Task<ChatLieuDTO> GetByIdAsync(Guid id);
        Task<ApiResult<ChatLieuDTO>> CreateAsync(ChatLieuDTO dto);
        Task<ApiResult<bool>> UpdateAsync(Guid id, ChatLieuDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
