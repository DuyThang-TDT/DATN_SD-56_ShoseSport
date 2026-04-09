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
            var list = await _repository.GetAllAsync();
            return list.Select(MapToDTO);
        }

        public async Task<SanPhamDTO> GetByIdAsync(Guid id)
        {
            var sp = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Không tìm thấy sản phẩm với ID {id}");

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
                Loai = dto.LoaiSanPham
            };

            await _repository.AddAsync(sanPham);
            await _repository.SaveAsync();

            // 🔥 LẤY SIZE + MÀU TỪ DB
            var sizes = await _context.KichCos.ToListAsync();
            var colors = await _context.MauSacs.ToListAsync();

            if (!sizes.Any() || !colors.Any())
                throw new Exception("Chưa có dữ liệu Size hoặc Màu trong DB");

            var listChiTiet = new List<SanPhamChiTiet>();

            foreach (var size in sizes)
            {
                foreach (var color in colors)
                {
                    listChiTiet.Add(new SanPhamChiTiet
                    {
                        SanPhamChiTietId = Guid.NewGuid(),
                        SanPhamId = sanPham.SanPhamId,
                        KichCoId = size.KichCoId,
                        MauSacId = color.MauSacId,
                        SoLuong = 10,            // 👉 mặc định
                        Gia = 1000000,           // 👉 mặc định
                        GiaNhap = 800000,
                        NgayTao = DateTime.Now,
                        TrangThai = 1
                    });
                }
            }

            await _context.SanPhamChiTiets.AddRangeAsync(listChiTiet);
            await _context.SaveChangesAsync();

            dto.SanPhamId = sanPham.SanPhamId;
            return dto;
        }

        public async Task UpdateAsync(Guid id, SanPhamDTO dto)
        {
            var existing = await _context.SanPhams
                .FirstOrDefaultAsync(sp => sp.SanPhamId == id)
                ?? throw new KeyNotFoundException("Không tìm thấy sản phẩm");

            existing.TenSanPham = dto.TenSanPham;
            existing.ThuongHieuId = dto.ThuongHieuId;
            existing.TrangThai = dto.TrangThai;
            existing.HanSuDung = dto.HanSuDung;
            existing.Loai = dto.LoaiSanPham;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var sanPham = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Không tìm thấy");

            sanPham.TrangThai = false;
            _repository.Update(sanPham);
            await _repository.SaveAsync();
        }

        public async Task<(IEnumerable<SanPhamDTO> Data, int TotalCount)> GetFilteredAsync(string? loaiSanPham, int page, int pageSize)
        {
            var all = await _repository.GetAllAsync();

            var filtered = all.Where(sp =>
                string.IsNullOrEmpty(loaiSanPham) ||
                sp.Loai == loaiSanPham
            );

            var totalCount = filtered.Count();

            var paged = filtered
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(MapToDTO);

            return (paged, totalCount);
        }

        public async Task<int> GetTotalProductsAsync()
        {
            var all = await _repository.GetAllAsync();
            return all.Count();
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

            var ids = data.Select(x => x.SanPhamId).ToList();

            var all = await _repository.GetAllAsync();

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
                LoaiSanPham = x.Loai,
                TrangThai = x.TrangThai,
                HanSuDung = x.HanSuDung
            };
        }
    }
}