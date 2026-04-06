using ShoseSport.API.Models;

namespace ShoseSport.API.Repository.IRepository
{
    public interface IChucVuRepository
    {
        Task<IEnumerable<ChucVu>> GetAllAsync();
        Task<ChucVu?> GetByIdAsync(Guid id);
        Task AddAsync(ChucVu chucVu);
        Task UpdateAsync(ChucVu chucVu);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<ChucVu>> FindByTenChucVuAsync(string tenChucVu);
    }
}