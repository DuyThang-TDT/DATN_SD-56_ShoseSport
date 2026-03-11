using ShoseSport.API.Models;

namespace ShoseSport.Web.Services.IService
{
    public interface IHinhThucThanhToanService
    {
        Task<IEnumerable<HinhThucThanhToan>> GetAllAsync();
        Task<HinhThucThanhToan?> GetByIdAsync(Guid id);
    }

}
