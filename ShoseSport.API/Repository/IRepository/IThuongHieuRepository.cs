using ShoseSport.API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoseSport.API.Repository.IRepository
{
    public interface IThuongHieuRepository
    {
        Task<IEnumerable<ThuongHieu>> GetAllAsync();
        Task<ThuongHieu> GetByIdAsync(Guid id);
        Task AddAsync(ThuongHieu entity);
        Task UpdateAsync(ThuongHieu entity);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}