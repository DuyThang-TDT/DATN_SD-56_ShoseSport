using ShoseSport.API.Models;

namespace ShoseSport.API.Repository.IRepository
{
    public interface IHinhThucThanhToanRepository
    {
        Task<IEnumerable<HinhThucThanhToan>> GetAllAsync();
        Task<HinhThucThanhToan?> GetByIdAsync(Guid id);
    }

}
