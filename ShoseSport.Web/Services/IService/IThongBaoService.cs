using ShoseSport.API.Models.DTO;
using ShoseSport.Web.Models;

namespace ShoseSport.Web.Services.IService
{
    public interface IThongBaoService
    {
        Task<IEnumerable<ThongBaoDTO>> GetAllAsync();
        Task CreateAsync(ThongBaoDTO dto);
        Task MarkAsReadAsync(Guid id);
        Task MarkAllAsReadAsync();
        Task DeleteAsync(Guid id);
    }
}
