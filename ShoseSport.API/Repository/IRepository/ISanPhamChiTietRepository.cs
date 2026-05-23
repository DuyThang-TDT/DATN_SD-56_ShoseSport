using ShoseSport.API.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ShoseSport.API.Repository.IRepository
{
    public interface ISanPhamChiTietRepository
    {
        Task<IEnumerable<SanPhamChiTiet>> GetAllAsync();
        Task<SanPhamChiTiet?> GetByIdAsync(Guid id);
        Task<IEnumerable<SanPhamChiTiet>> FindAsync(Expression<Func<SanPhamChiTiet, bool>> predicate);
        Task<SanPhamChiTiet?> FindOneAsync(Expression<Func<SanPhamChiTiet, bool>> predicate);
        Task<bool> ExistsAsync(Guid id);
        Task AddAsync(SanPhamChiTiet entity);
        void Update(SanPhamChiTiet entity);
        void Delete(SanPhamChiTiet entity);
        Task SaveAsync();
    }
}
