using ShoseSport.API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoseSport.API.Repository.IRepository
{
    public interface IMauSacRepository
    {
        Task<IEnumerable<MauSac>> GetAllAsync();
        Task<MauSac> GetByIdAsync(Guid id);
        Task AddAsync(MauSac entity);
        Task UpdateAsync(MauSac entity);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}