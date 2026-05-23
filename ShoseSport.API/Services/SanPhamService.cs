using ShoseSport.API.Data;
using ShoseSport.API.Models;
using ShoseSport.API.Models.DTO;
using ShoseSport.API.Repository.IRepository;
using ShoseSport.API.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoseSport.API.Services
{
    public class SanPhamService : ISanPhamService
    {
        private readonly ISanPhamRepository _repository;
        private readonly AppDbContext _context;

        public SanPhamService(ISanPhamRepository repository, AppDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<IEnumerable<SanPhamDTO>> GetAllAsync()
        {
            var list = await _context.SanPhams
                .Include(x => x.ThuongHieu)
                .Include(x => x.SanPhamChiTiets)
                    .ThenInclude(ct => ct.Anh)
                .Include(x => x.SanPhamThanhPhans)
                    .ThenInclude(tp => tp.ThanhPhan)
                .Include(x => x.SanPhamChatLieus)
                    .ThenInclude(cl => cl.ChatLieu)
                .ToListAsync();

            return list.Select(MapToDTO);
        }

        public async Task<SanPhamDTO> GetByIdAsync(Guid id)
        {
            var sp = await _context.SanPhams
                .Include(x => x.ThuongHieu)
                .Include(x => x.SanPhamChiTiets)
                    .ThenInclude(ct => ct.Anh)
                .Include(x => x.SanPhamThanhPhans)
                    .ThenInclude(tp => tp.ThanhPhan)
                .Include(x => x.SanPhamChatLieus)
                    .ThenInclude(cl => cl.ChatLieu)
                .FirstOrDefaultAsync(x => x.SanPhamId == id);

            if (sp == null)
                throw new KeyNotFoundException($"Không tìm thấy sản phẩm với ID {id}");

            return MapToDTO(sp);
        }

        // ============================
        // 🚀 CREATE + AUTO GEN VARIANT
        // ============================
        public async Task<SanPhamDTO> CreateAsync(SanPhamDTO dto)
        {
            var sanPham = new SanPham
            {
                SanPhamId = Guid.NewGuid(),
                TenSanPham = dto.TenSanPham,
                ThuongHieuId = dto.ThuongHieuId,
                TrangThai = true,
                HanSuDung = dto.HanSuDung,

            {
                foreach (var color in colors)
                {


            dto.SanPhamId = sanPham.SanPhamId;

            return dto;
        }

        public async Task UpdateAsync(Guid id, SanPhamDTO dto)
        {
            var existing = await _context.SanPhams


            existing.TenSanPham = dto.TenSanPham;
            existing.ThuongHieuId = dto.ThuongHieuId;
            existing.TrangThai = dto.TrangThai;
            existing.HanSuDung = dto.HanSuDung;
            existing.Loai = dto.LoaiSanPham;
<

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {

        }

        public async Task<(IEnumerable<SanPhamDTO> Data, int TotalCount)> GetFilteredAsync(
            string? loaiSanPham,
            int page,
            int pageSize)
        {
            var all = await _context.SanPhams
                .Include(x => x.ThuongHieu)
                .Include(x => x.SanPhamChiTiets)
                    .ThenInclude(ct => ct.Anh)
                .Include(x => x.SanPhamThanhPhans)
                    .ThenInclude(tp => tp.ThanhPhan)
                .Include(x => x.SanPhamChatLieus)
                    .ThenInclude(cl => cl.ChatLieu)
                .ToListAsync();

            var filtered = all.Where(sp =>
                string.IsNullOrEmpty(loaiSanPham) ||


            var totalCount = filtered.Count();

            var paged = filtered
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(MapToDTO);

            return (paged, totalCount);
        }

        public async Task<int> GetTotalProductsAsync()
        {
            return await _context.SanPhams.CountAsync();
        }

        public async Task<IEnumerable<SanPhamDTO>> GetTopSellingProductsAsync(int top)
        {
            var data = await _context.HoaDonChiTiets
                .GroupBy(x => x.SanPhamChiTiet.SanPhamId)
                .Select(g => new
                {
                    SanPhamId = g.Key,
                    Total = g.Sum(x => x.SoLuongSanPham)
                })
                .OrderByDescending(x => x.Total)
                .Take(top)
                .ToListAsync();



            return all.Where(x => ids.Contains(x.SanPhamId))
                      .Select(MapToDTO);
        }

        private static SanPhamDTO MapToDTO(SanPham x)
        {

            return new SanPhamDTO
            {
                SanPhamId = x.SanPhamId,
                TenSanPham = x.TenSanPham,
                ThuongHieuId = x.ThuongHieuId ?? Guid.Empty,
                TenThuongHieu = x.ThuongHieu?.TenThuongHieu,

                TrangThai = x.TrangThai,
                HanSuDung = x.HanSuDung,

                SanPhamChiTiets = x.SanPhamChiTiets?
                    .Select(spct => new SanPhamChiTietDTO
                    {
                        SanPhamChiTietId = spct.SanPhamChiTietId,
                        SanPhamId = spct.SanPhamId,
                        Gia = spct.Gia,
                        SoLuong = spct.SoLuong,
                        DuongDan = spct.Anh != null
                            ? spct.Anh.DuongDan
                            : ""
                    })
                    .ToList()
            };
        }
    }
}