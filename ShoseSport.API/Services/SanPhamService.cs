using FurryFriends.API.Data;
using FurryFriends.API.Models;
using FurryFriends.API.Models.DTO;
using FurryFriends.API.Repository.IRepository;
using FurryFriends.API.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurryFriends.API.Services
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

        public async Task<SanPhamDTO> CreateAsync(SanPhamDTO dto)
        {
            var sanPham = new SanPham
            {
                SanPhamId = Guid.NewGuid(),
                TenSanPham = dto.TenSanPham,
                ThuongHieuId = dto.ThuongHieuId,
                TrangThai = dto.TrangThai,
                HanSuDung = dto.HanSuDung,
                Loai = dto.LoaiSanPham,
                SanPhamThanhPhans = new List<SanPhamThanhPhan>(),
                SanPhamChatLieus = new List<SanPhamChatLieu>()
            };

            if (dto.LoaiSanPham == "DoAn" && dto.ThanhPhanIds != null)
            {
                foreach (var tpId in dto.ThanhPhanIds)
                {
                    sanPham.SanPhamThanhPhans.Add(new SanPhamThanhPhan
                    {
                        SanPhamId = sanPham.SanPhamId,
                        ThanhPhanId = tpId
                    });
                }
            }
            else if ((dto.LoaiSanPham == "DoDung"
                || dto.LoaiSanPham == "GiayTheThao"
                || dto.LoaiSanPham == "GiayTay")
                && dto.ChatLieuIds != null)
            {
                foreach (var clId in dto.ChatLieuIds)
                {
                    sanPham.SanPhamChatLieus.Add(new SanPhamChatLieu
                    {
                        SanPhamId = sanPham.SanPhamId,
                        ChatLieuId = clId
                    });
                }
            }

            await _repository.AddAsync(sanPham);
            await _repository.SaveAsync();

            dto.SanPhamId = sanPham.SanPhamId;

            return dto;
        }

        public async Task UpdateAsync(Guid id, SanPhamDTO dto)
        {
            var existing = await _context.SanPhams
                .Include(sp => sp.SanPhamThanhPhans)
                .Include(sp => sp.SanPhamChatLieus)
                .FirstOrDefaultAsync(sp => sp.SanPhamId == id);

            if (existing == null)
                throw new KeyNotFoundException($"Không tìm thấy sản phẩm với ID {id}");

            existing.TenSanPham = dto.TenSanPham;
            existing.ThuongHieuId = dto.ThuongHieuId;
            existing.TrangThai = dto.TrangThai;
            existing.HanSuDung = dto.HanSuDung;
            existing.Loai = dto.LoaiSanPham;

            existing.SanPhamThanhPhans.Clear();
            existing.SanPhamChatLieus.Clear();

            if (dto.LoaiSanPham == "DoAn" && dto.ThanhPhanIds != null)
            {
                foreach (var tpId in dto.ThanhPhanIds)
                {
                    existing.SanPhamThanhPhans.Add(new SanPhamThanhPhan
                    {
                        ThanhPhanId = tpId
                    });
                }
            }
            else if ((dto.LoaiSanPham == "DoDung"
                || dto.LoaiSanPham == "GiayTheThao"
                || dto.LoaiSanPham == "GiayTay")
                && dto.ChatLieuIds != null)
            {
                foreach (var clId in dto.ChatLieuIds)
                {
                    existing.SanPhamChatLieus.Add(new SanPhamChatLieu
                    {
                        ChatLieuId = clId
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                if (!await _repository.ExistsAsync(id))
                    throw new KeyNotFoundException($"Không tìm thấy sản phẩm với ID {id}");

                var sanPhamChiTiets = await _context.SanPhamChiTiets
                    .Where(spct => spct.SanPhamId == id)
                    .ToListAsync();

                var sanPhamChiTietIds = sanPhamChiTiets
                    .Select(spct => spct.SanPhamChiTietId)
                    .ToList();

                if (sanPhamChiTietIds.Any())
                {
                    var dotGiamGiaSanPham = await _context.DotGiamGiaSanPhams
                        .Where(dggsp => sanPhamChiTietIds.Contains(dggsp.SanPhamChiTietId))
                        .FirstOrDefaultAsync();

                    if (dotGiamGiaSanPham != null)
                    {
                        throw new InvalidOperationException(
                            "Không thể xóa sản phẩm vì đang được áp dụng trong chương trình giảm giá."
                        );
                    }
                }

                var sanPham = await _repository.GetByIdAsync(id);

                if (sanPham != null)
                {
                    sanPham.TrangThai = false;
                    _repository.Update(sanPham);
                    await _repository.SaveAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting product {id}: {ex.Message}");
                throw;
            }
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
                sp.Loai == loaiSanPham);

            var totalCount = filtered.Count();

            var paged = filtered
                .OrderByDescending(sp => sp.SanPhamId)
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
            var topProductsInfo = await _context.HoaDonChiTiets
                .Include(ct => ct.SanPhamChiTiet)
                    .ThenInclude(spct => spct.SanPham)
                .GroupBy(ct => ct.SanPhamChiTiet.SanPhamId)
                .Select(g => new
                {
                    SanPhamId = g.Key,
                    TotalSold = g.Sum(x => x.SoLuongSanPham)
                })
                .OrderByDescending(x => x.TotalSold)
                .Take(top)
                .ToListAsync();

            var ids = topProductsInfo.Select(x => x.SanPhamId).ToList();

            var allSanPhams = await _context.SanPhams
                .Include(x => x.ThuongHieu)
                .Include(x => x.SanPhamChiTiets)
                    .ThenInclude(ct => ct.Anh)
                .ToListAsync();

            var topSellingSanPhams = allSanPhams
                .Where(sp => ids.Contains(sp.SanPhamId));

            return topSellingSanPhams.Select(MapToDTO);
        }

        private static SanPhamDTO MapToDTO(SanPham x)
        {
            string loaiSanPham = x.Loai;

            if (string.IsNullOrEmpty(loaiSanPham))
            {
                loaiSanPham = (x.SanPhamThanhPhans?.Any() == true)
                    ? "GiayTheThao"
                    : "GiayTay";
            }

            return new SanPhamDTO
            {
                SanPhamId = x.SanPhamId,
                TenSanPham = x.TenSanPham,
                ThuongHieuId = x.ThuongHieuId ?? Guid.Empty,
                TenThuongHieu = x.ThuongHieu?.TenThuongHieu,
                LoaiSanPham = loaiSanPham,

                ThanhPhanIds = x.SanPhamThanhPhans?
                    .Select(tp => tp.ThanhPhanId)
                    .ToList() ?? new List<Guid>(),

                ChatLieuIds = x.SanPhamChatLieus?
                    .Select(cl => cl.ChatLieuId)
                    .ToList() ?? new List<Guid>(),

                TenThanhPhans = x.SanPhamThanhPhans?
                    .Select(tp => tp.ThanhPhan != null
                        ? tp.ThanhPhan.TenThanhPhan
                        : "")
                    .ToList() ?? new List<string>(),

                TenChatLieus = x.SanPhamChatLieus?
                    .Select(cl => cl.ChatLieu != null
                        ? cl.ChatLieu.TenChatLieu
                        : "")
                    .ToList() ?? new List<string>(),

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