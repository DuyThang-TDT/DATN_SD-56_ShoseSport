using ShoseSport.API.Models;

namespace ShoseSport.Web.Services.IService
{
    public interface IChucVuService
    {
        Task<IEnumerable<ChucVu>> GetAllAsync();
        Task<ChucVu?> GetByIdAsync(Guid chucVuId);
        Task AddAsync(ChucVu chucVu);
        Task UpdateAsync(ChucVu chucVu);
        Task DeleteAsync(Guid chucVuId);
        Task<IEnumerable<ChucVu>> FindByTenChucVuAsync(string tenChucVu);
    }
}