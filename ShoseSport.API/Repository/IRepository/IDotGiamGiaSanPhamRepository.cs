using ShoseSport.API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoseSport.API.Repository.IRepository
{
    public interface IDotGiamGiaSanPhamRepository
    {
        // Các hàm không thay đổi
        Task<IEnumerable<DotGiamGiaSanPham>> GetAllAsync();
        Task<DotGiamGiaSanPham> GetByIdAsync(Guid id);
        Task<IEnumerable<DotGiamGiaSanPham>> GetByGiamGiaIdAsync(Guid giamGiaId);
        Task<bool> ExistsAsync(Guid id);

        // Các hàm thay đổi
        Task AddAsync(DotGiamGiaSanPham entity);
        Task AddRangeAsync(IEnumerable<DotGiamGiaSanPham> entities);
        void Update(DotGiamGiaSanPham entity);
        void Delete(DotGiamGiaSanPham entity);
        void DeleteRange(IEnumerable<DotGiamGiaSanPham> entities);

        // Lưu thay đổi
        Task SaveAsync();
    }
}