using ShoseSport.API.Models;
using ShoseSport.API.Models.DTO;
using ShoseSport.API.Repository.IRepository;
using ShoseSport.API.Services.IServices;
using ShoseSport.API.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoseSport.API.Services
{
    public class SanPhamChiTietService : ISanPhamChiTietService
    {
        private readonly ISanPhamChiTietRepository _repository;
        private readonly AppDbContext _context;

        public SanPhamChiTietService(ISanPhamChiTietRepository repository, AppDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<IEnumerable<SanPhamChiTietDTO>> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();
            return list.Select(x => new SanPhamChiTietDTO
            {
                SanPhamChiTietId = x.SanPhamChiTietId,
                SanPhamId = x.SanPhamId,
                TenSanPham = x.SanPham?.TenSanPham,
                KichCoId = x.KichCoId,
                TenKichCo = x.KichCo?.TenKichCo,
                MauSacId = x.MauSacId,
                TenMau = x.MauSac?.TenMau,
                Gia = x.Gia,
                GiaNhap = x.GiaNhap,
                SoLuong = x.SoLuong,
                MoTa = x.MoTa,
                AnhId = x.AnhId,
                DuongDan = x.Anh?.DuongDan,
                NgayTao = x.NgayTao,
                NgaySua = x.NgaySua,
                TrangThai = x.TrangThai,
                TrangThaiSanPham = x.SanPham?.TrangThai,
                HanSuDung = x.SanPham?.HanSuDung
            });
        }

        public async Task<SanPhamChiTietDTO?> GetByIdAsync(Guid id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null) return null;

            return new SanPhamChiTietDTO
            {
                SanPhamChiTietId = item.SanPhamChiTietId,
                SanPhamId = item.SanPhamId,
                TenSanPham = item.SanPham?.TenSanPham,
                KichCoId = item.KichCoId,
                TenKichCo = item.KichCo?.TenKichCo,
                MauSacId = item.MauSacId,
                TenMau = item.MauSac?.TenMau,
                Gia = item.Gia,
                GiaNhap = item.GiaNhap,
                SoLuong = item.SoLuong,
                MoTa = item.MoTa,
                AnhId = item.AnhId,
                DuongDan = item.Anh?.DuongDan,
                NgayTao = item.NgayTao,
                NgaySua = item.NgaySua,
                TrangThai = item.TrangThai,
                HanSuDung = item.SanPham?.HanSuDung
            };
        }

        // ============================
        // 🔥 FIX CHÍNH Ở ĐÂY
        // ============================
        public async Task<SanPhamChiTietDTO?> CreateAndReturnAsync(SanPhamChiTietDTO dto)
        {
            // 1. Lấy hoặc tạo sản phẩm
            var sanPham = await _context.SanPhams
                .FirstOrDefaultAsync(x => x.SanPhamId == dto.SanPhamId);

            if (sanPham == null)
            {
                sanPham = new SanPham
                {
                    SanPhamId = Guid.NewGuid(),
                    TenSanPham = "Sản phẩm auto",
                    TrangThai = true
                };

                _context.SanPhams.Add(sanPham);
                await _context.SaveChangesAsync();
            }

            // 2. 🔥 CHECK TRÙNG (QUAN TRỌNG NHẤT)
            var exists = await _context.SanPhamChiTiets.AnyAsync(x =>
                x.SanPhamId == sanPham.SanPhamId &&
                x.KichCoId == dto.KichCoId &&
                x.MauSacId == dto.MauSacId
            );

            if (exists)
            {
                // 👉 Không tạo nữa, tránh trùng
                return null;
            }

            // 3. Tạo mới SPCT
            var entity = new SanPhamChiTiet
            {
                SanPhamChiTietId = Guid.NewGuid(),
                SanPhamId = sanPham.SanPhamId,
                KichCoId = dto.KichCoId,
                MauSacId = dto.MauSacId,
                Gia = dto.Gia,
                GiaNhap = dto.GiaNhap,
                SoLuong = dto.SoLuong,
                MoTa = dto.MoTa,
                AnhId = dto.AnhId,
                NgayTao = DateTime.Now,
                TrangThai = dto.TrangThai ?? 1
            };

            await _repository.AddAsync(entity);
            await _repository.SaveAsync();

            return new SanPhamChiTietDTO
            {
                SanPhamChiTietId = entity.SanPhamChiTietId,
                SanPhamId = entity.SanPhamId,
                KichCoId = entity.KichCoId,
                MauSacId = entity.MauSacId,
                Gia = entity.Gia,
                GiaNhap = entity.GiaNhap,
                SoLuong = entity.SoLuong,
                MoTa = entity.MoTa,
                AnhId = entity.AnhId,
                NgayTao = entity.NgayTao,
                TrangThai = entity.TrangThai
            };
        }

        public async Task<bool> CreateAsync(SanPhamChiTietDTO dto)
        {
            var result = await CreateAndReturnAsync(dto);
            return result != null;
        }

        public async Task<bool> UpdateAsync(Guid id, SanPhamChiTietDTO dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return false;

            entity.KichCoId = dto.KichCoId;
            entity.MauSacId = dto.MauSacId;
            entity.Gia = dto.Gia;
            entity.GiaNhap = dto.GiaNhap;
            entity.SoLuong = dto.SoLuong;
            entity.MoTa = dto.MoTa;
            entity.AnhId = dto.AnhId;
            entity.NgaySua = DateTime.Now;
            entity.TrangThai = dto.TrangThai ?? entity.TrangThai;

            _repository.Update(entity);
            await _repository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return false;

            _repository.Delete(entity);
            await _repository.SaveAsync();
            return true;
        }
    }
}