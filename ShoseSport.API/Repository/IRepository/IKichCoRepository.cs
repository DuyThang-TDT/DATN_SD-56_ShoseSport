using ShoseSport.API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoseSport.API.Repository.IRepository
{
    public interface IKichCoRepository
    {
        Task<IEnumerable<KichCo>> GetAllAsync();
        Task<KichCo> GetByIdAsync(Guid id);
        Task AddAsync(KichCo entity);
        Task UpdateAsync(KichCo entity);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}