using ShoseSport.API.Data;
using ShoseSport.API.Models;
using ShoseSport.API.Models.DTO;
using ShoseSport.API.Repository.IRepository;
using ShoseSport.API.Services;
using Microsoft.EntityFrameworkCore;

namespace ShoseSport.API.Repository
{
    public class GioHangRepository : IGioHangRepository
    {
        private readonly AppDbContext _context;
        private readonly VoucherCalculationService _voucherService;
        private static decimal TinhDonGiaSauGiam(decimal giaGoc, decimal phanTram)
        {
            if (phanTram <= 0) return giaGoc;
            var giaSau = giaGoc * (100 - phanTram) / 100m;
            return Math.Round(giaSau, 0, MidpointRounding.AwayFromZero);
        }

        private async Task<decimal> LayPhanTramGiamToiDaAsync(Guid sanPhamChiTietId)
        {
            var now = DateTime.Now; // Changed from DateTime.UtcNow
            var percents = await _context.DotGiamGiaSanPhams
                .AsNoTracking()
                .Include(d => d.GiamGia)
                .Where(d => d.SanPhamChiTietId == sanPhamChiTietId
                            && d.TrangThai
                            && d.GiamGia != null
                            && d.GiamGia.TrangThai
                            && d.GiamGia.NgayBatDau <= now
                            && d.GiamGia.NgayKetThuc >= now)
                .Select(d => d.PhanTramGiamGia)
                .ToListAsync();
            return percents.Any() ? percents.Max() : 0m;
        }

        private async Task<Dictionary<Guid, decimal>> LayPhanTramGiamToiDaChoNhieuAsync(List<Guid> sanPhamChiTietIds)
        {
            if (sanPhamChiTietIds == null || sanPhamChiTietIds.Count == 0)
                return new Dictionary<Guid, decimal>();

            var now = DateTime.Now; // Changed from DateTime.UtcNow
            var rows = await _context.DotGiamGiaSanPhams
                .AsNoTracking()
                .Include(d => d.GiamGia)
                .Where(d => sanPhamChiTietIds.Contains(d.SanPhamChiTietId)
                            && d.TrangThai
                            && d.GiamGia != null
                            && d.GiamGia.TrangThai
                            && d.GiamGia.NgayBatDau <= now
                            && d.GiamGia.NgayKetThuc >= now)
                .Select(d => new { d.SanPhamChiTietId, d.PhanTramGiamGia })
                .ToListAsync();

            return rows
                .GroupBy(x => x.SanPhamChiTietId)
                .ToDictionary(g => g.Key, g => g.Max(x => x.PhanTramGiamGia));
        }

        public GioHangRepository(AppDbContext context, VoucherCalculationService voucherService)
        {
            _context = context;
            _voucherService = voucherService;
        }

        public async Task<GioHangDTO> GetGioHangByKhachHangIdAsync(Guid khachHangId)
        {
            var gioHang = await _context.GioHangs
                .Include(g => g.GioHangChiTiets)
                    .ThenInclude(gc => gc.SanPhamChiTiet)
                        .ThenInclude(spc => spc.MauSac)
                .Include(g => g.GioHangChiTiets)
                    .ThenInclude(gc => gc.SanPhamChiTiet)
                        .ThenInclude(spc => spc.KichCo)
                .Include(g => g.GioHangChiTiets)
                    .ThenInclude(gc => gc.SanPhamChiTiet)
                        .ThenInclude(spc => spc.Anh)
                .Include(g => g.GioHangChiTiets)
                    .ThenInclude(gc => gc.SanPhamChiTiet)
                        .ThenInclude(spc => spc.SanPham)
                .FirstOrDefaultAsync(g => g.KhachHangId == khachHangId);

            if (gioHang == null)
                return null;

            //var invalidItems = gioHang.GioHangChiTiets
            //    .Where(gc =>
            //        gc.SanPhamChiTiet == null ||
            //        gc.SanPhamChiTiet.TrangThai == 0 ||
            //        gc.SanPhamChiTiet.SoLuong <= 0 ||
            //        (gc.SanPhamChiTiet.SanPham != null && gc.SanPhamChiTiet.SanPham.TrangThai == false)
            //    ).ToList();

            //// Nếu có item không hợp lệ → xóa khỏi DB
            //if (invalidItems.Any())
            //{
            //    _context.GioHangChiTiets.RemoveRange(invalidItems);
            //    await _context.SaveChangesAsync();
            //    gioHang.GioHangChiTiets = gioHang.GioHangChiTiets.Except(invalidItems).ToList();
            //}

            var gioHangDTO = new GioHangDTO
            {
                GioHangId = gioHang.GioHangId,
                KhachHangId = gioHang.KhachHangId,
                GioHangChiTiets = new List<GioHangChiTietDTO>()
            };

            try
            {
                var ids = (gioHang.GioHangChiTiets ?? new List<GioHangChiTiet>())
                    .Where(x => x.SanPhamChiTietId.HasValue)
                    .Select(x => x.SanPhamChiTietId!.Value)
                    .Distinct()
                    .ToList();

                var idToMaxDiscount = await LayPhanTramGiamToiDaChoNhieuAsync(ids);

                foreach (var gc in gioHang.GioHangChiTiets ?? new List<GioHangChiTiet>())
                {
                    var giaGoc = gc.SanPhamChiTiet?.Gia ?? 0m;
                    var giamMax = (gc.SanPhamChiTietId.HasValue && idToMaxDiscount.TryGetValue(gc.SanPhamChiTietId.Value, out var p)) ? p : 0m;
                    var donGiaSau = TinhDonGiaSauGiam(giaGoc, giamMax);
                    var thanhTienTinhLai = donGiaSau * gc.SoLuong;
                    
                    Console.WriteLine($"🔍 [Repository] GetGioHangByKhachHangIdAsync - Sản phẩm: {gc.SanPhamChiTiet?.SanPham?.TenSanPham}");
                    Console.WriteLine($"  - Giá gốc: {giaGoc:N0}");
                    Console.WriteLine($"  - Giảm tối đa: {giamMax}%");
                    Console.WriteLine($"  - Đơn giá sau giảm: {donGiaSau:N0}");
                    Console.WriteLine($"  - Số lượng: {gc.SoLuong}");
                    Console.WriteLine($"  - Thành tiền từ DB: {gc.ThanhTien:N0}");
                    Console.WriteLine($"  - Thành tiền tính lại: {thanhTienTinhLai:N0}");
                    Console.WriteLine($"  - Kiểm tra: {donGiaSau:N0} × {gc.SoLuong} = {thanhTienTinhLai:N0}");
                    Console.WriteLine($"  - Chênh lệch: {gc.ThanhTien - thanhTienTinhLai:N0}");
                    
                    gioHangDTO.GioHangChiTiets.Add(new GioHangChiTietDTO
                    {
                        GioHangChiTietId = gc.GioHangChiTietId,
                        SanPhamId = gc.SanPhamChiTiet != null ? gc.SanPhamChiTiet.SanPhamId : Guid.Empty,
                        SanPhamChiTietId = gc.SanPhamChiTietId ?? Guid.Empty,
                        SoLuong = gc.SoLuong,
                        TenSanPham = gc.SanPhamChiTiet?.SanPham?.TenSanPham ?? "Không xác định",
                        DonGia = donGiaSau,
                        ThanhTien = thanhTienTinhLai, // Sử dụng giá trị tính lại thay vì từ DB
                        AnhSanPham = gc.SanPhamChiTiet?.Anh?.DuongDan ?? "",
                        MauSac = gc.SanPhamChiTiet?.MauSac?.TenMau ?? "Không xác định",
                        KichCo = gc.SanPhamChiTiet?.KichCo?.TenKichCo ?? "Không xác định",
                        GiaGoc = giaGoc,
                        PhanTramGiam = giamMax
                    });
                }
            }
            catch
            {
                // Fallback: nếu có lỗi bất ngờ ở phần giảm giá, vẫn trả về giỏ hàng với giá gốc để tránh 500
                foreach (var gc in gioHang.GioHangChiTiets ?? new List<GioHangChiTiet>())
                {
                    var giaGoc = gc.SanPhamChiTiet?.Gia ?? 0m;
                    gioHangDTO.GioHangChiTiets.Add(new GioHangChiTietDTO
                    {
                        GioHangChiTietId = gc.GioHangChiTietId,
                        SanPhamId = gc.SanPhamChiTiet != null ? gc.SanPhamChiTiet.SanPhamId : Guid.Empty,
                        SanPhamChiTietId = gc.SanPhamChiTietId ?? Guid.Empty,
                        SoLuong = gc.SoLuong,
                        TenSanPham = gc.SanPhamChiTiet?.SanPham?.TenSanPham ?? "Không xác định",
                        DonGia = giaGoc,
                        ThanhTien = giaGoc * gc.SoLuong,
                        AnhSanPham = gc.SanPhamChiTiet?.Anh?.DuongDan ?? "",
                        MauSac = gc.SanPhamChiTiet?.MauSac?.TenMau ?? "Không xác định",
                        KichCo = gc.SanPhamChiTiet?.KichCo?.TenKichCo ?? "Không xác định"
                    });
                }
            }

            return gioHangDTO;
        }

        public async Task<GioHangChiTiet> AddSanPhamVaoGioAsync(Guid khachHangId, Guid sanPhamChiTietId, int soLuong)
        {
            // Lấy giỏ hàng hiện tại (không cần Include để tránh track các entity cũ gây xung đột tracking)
            if (soLuong <= 0)
            {
                throw new Exception("Số lượng phải lớn hơn 0.");
            }

            var gioHang = await _context.GioHangs
                .FirstOrDefaultAsync(g => g.KhachHangId == khachHangId);

            if (gioHang == null)
            {
                gioHang = new GioHang
                {
                    GioHangId = Guid.NewGuid(),
                    KhachHangId = khachHangId,
                    NgayTao = DateTime.Now,
                    TrangThai = true,
                    GioHangChiTiets = new List<GioHangChiTiet>()
                };
                _context.GioHangs.Add(gioHang);
                await _context.SaveChangesAsync(); // Lưu giỏ hàng trước để đảm bảo có ID trong DB
            }

            // ✅ Sử dụng database lock để lấy thông tin sản phẩm mới nhất
            var sanPhamChiTiet = await GetSanPhamChiTietWithLockAsync(sanPhamChiTietId);
            if (sanPhamChiTiet == null) return null;

            if (sanPhamChiTiet.SanPham == null)
            {
                throw new Exception("Sản phẩm không tồn tại.");
            }

            // 🔹 Kiểm tra trạng thái hoạt động
            if (sanPhamChiTiet.TrangThai == 0 || sanPhamChiTiet.SanPham.TrangThai == false)
            {
                throw new Exception("Sản phẩm đã tạm dừng hoạt động.");
            }

            // 🔹 Check số lượng tồn kho
            if (soLuong > sanPhamChiTiet.SoLuong)
            {
                throw new Exception($"Số lượng sản phẩm trong kho không đủ. Chỉ còn {sanPhamChiTiet.SoLuong} sản phẩm.");
            }

            // Lấy trực tiếp chi tiết giỏ hàng từ DB để tránh xung đột tracking
            var existingItem = await _context.GioHangChiTiets
                .FirstOrDefaultAsync(x => x.GioHangId == gioHang.GioHangId && x.SanPhamChiTietId == sanPhamChiTietId);

            if (existingItem != null)
            {
                Console.WriteLine("🔍 [Repository] AddSanPhamVaoGioAsync - Tăng số lượng sản phẩm đã có trong giỏ hàng");
                existingItem.SoLuong += soLuong;
                existingItem.ThanhTien = existingItem.DonGia * existingItem.SoLuong;
                existingItem.NgayCapNhat = DateTime.Now;
                await _context.SaveChangesAsync();
                return existingItem;
            }

            // Thêm sản phẩm mới vào giỏ
            var giaGoc = sanPhamChiTiet.Gia;
            var giamMax = await LayPhanTramGiamToiDaAsync(sanPhamChiTietId);
            var donGiaSau = TinhDonGiaSauGiam(giaGoc, giamMax);
            var thanhTien = donGiaSau * soLuong;

            var createdItem = new GioHangChiTiet
            {
                GioHangChiTietId = Guid.NewGuid(),
                GioHangId = gioHang.GioHangId,
                SanPhamChiTietId = sanPhamChiTietId,
                SoLuong = soLuong,
                DonGia = donGiaSau,
                ThanhTien = thanhTien,
                TrangThai = true,
                NgayTao = DateTime.Now
            };

            await _context.GioHangChiTiets.AddAsync(createdItem);
            await _context.SaveChangesAsync();

            // Load thêm thông tin sản phẩm để trả về đầy đủ
            await _context.Entry(createdItem).Reference(x => x.SanPhamChiTiet).LoadAsync();
            return createdItem;
        }

        public async Task<GioHangChiTiet> UpdateSoLuongAsync(Guid gioHangChiTietId, int soLuong)
        {
            var gioHangChiTiet = await _context.GioHangChiTiets
                .Include(gc => gc.SanPhamChiTiet)
                    .ThenInclude(spc => spc.SanPham)
                .FirstOrDefaultAsync(gc => gc.GioHangChiTietId == gioHangChiTietId);

            if (gioHangChiTiet == null)
                return null;

            // ✅ Sử dụng database lock để lấy thông tin sản phẩm mới nhất
            var spc = await GetSanPhamChiTietWithLockAsync(gioHangChiTiet.SanPhamChiTietId.Value);
            var soLuongTonKho = spc?.SoLuong ?? 0;

            if (spc == null || spc.SanPham == null)
            {
                throw new InvalidOperationException("Sản phẩm không tồn tại hoặc đã bị xóa.");
            }

            if (spc.TrangThai == 0 || spc.SanPham.TrangThai == false)
            {
                throw new InvalidOperationException("Sản phẩm này đã tạm ngừng hoạt động, không thể cập nhật số lượng.");
            }

            // ✅ Kiểm tra số lượng yêu cầu có vượt quá tồn kho hay không
            if (soLuong > soLuongTonKho)
            {
                throw new InvalidOperationException(
                    $"Số lượng sản phẩm trong kho không đủ. Hiện chỉ còn {soLuongTonKho} sản phẩm."
                );
            }

            if (soLuong <= 0)
            {
                throw new InvalidOperationException("Số lượng phải lớn hơn 0.");
            }

            Console.WriteLine($"🔍 [Repository] UpdateSoLuongAsync - Trước khi cập nhật:");
            Console.WriteLine($"  - Số lượng cũ: {gioHangChiTiet.SoLuong}");
            Console.WriteLine($"  - Đơn giá cũ: {gioHangChiTiet.DonGia:N0}");
            Console.WriteLine($"  - Thành tiền cũ: {gioHangChiTiet.ThanhTien:N0}");

            gioHangChiTiet.SoLuong = soLuong;

            // Đảm bảo DonGia và ThanhTien luôn đúng khi đổi số lượng
            var donGiaHienTai = gioHangChiTiet.DonGia;
            if (donGiaHienTai <= 0)
            {
                var giaGoc = spc.Gia;
                var giamMax = gioHangChiTiet.SanPhamChiTietId.HasValue
                    ? await LayPhanTramGiamToiDaAsync(gioHangChiTiet.SanPhamChiTietId.Value)
                    : 0m;
                donGiaHienTai = TinhDonGiaSauGiam(giaGoc, giamMax);
                gioHangChiTiet.DonGia = donGiaHienTai;
                
                Console.WriteLine($"🔍 [Repository] Cập nhật đơn giá: {giaGoc:N0} -> {donGiaHienTai:N0} (giảm {giamMax}%)");
            }
            
            var thanhTienMoi = donGiaHienTai * soLuong;
            gioHangChiTiet.ThanhTien = thanhTienMoi;
            gioHangChiTiet.NgayCapNhat = DateTime.Now;

            Console.WriteLine($"🔍 [Repository] Sau khi cập nhật:");
            Console.WriteLine($"  - Số lượng mới: {gioHangChiTiet.SoLuong}");
            Console.WriteLine($"  - Đơn giá mới: {gioHangChiTiet.DonGia:N0}");
            Console.WriteLine($"  - Thành tiền mới: {gioHangChiTiet.ThanhTien:N0}");
            Console.WriteLine($"  - Kiểm tra: {donGiaHienTai:N0} × {soLuong} = {thanhTienMoi:N0}");
            Console.WriteLine($"  - Chênh lệch với giá trị cũ: {gioHangChiTiet.ThanhTien - thanhTienMoi:N0}");

            await _context.SaveChangesAsync();
            return gioHangChiTiet;
        }

        public async Task<bool> RemoveSanPhamKhoiGioAsync(Guid gioHangChiTietId)
        {
            var gioHangChiTiet = await _context.GioHangChiTiets
                .FirstOrDefaultAsync(gc => gc.GioHangChiTietId == gioHangChiTietId);

            if (gioHangChiTiet == null)
                return false;

            _context.GioHangChiTiets.Remove(gioHangChiTiet);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<SanPhamChiTiet> GetSanPhamChiTietByIdAsync(Guid id)
        {
            return await _context.SanPhamChiTiets
                .Include(spc => spc.SanPham)
                .Include(spc => spc.MauSac)
                .Include(spc => spc.KichCo)
                .Include(spc => spc.Anh)
                .FirstOrDefaultAsync(spc => spc.SanPhamChiTietId == id);
        }

        public async Task<GioHangChiTietDTO> ConvertToDTOAsync(GioHangChiTiet gioHangChiTiet)
        {
            var sanPhamChiTiet = await _context.SanPhamChiTiets
                .Include(spc => spc.SanPham)
                .Include(spc => spc.MauSac)
                .Include(spc => spc.KichCo)
                .Include(spc => spc.Anh)
                .FirstOrDefaultAsync(spc => spc.SanPhamChiTietId == gioHangChiTiet.SanPhamChiTietId);

            var giaGoc = sanPhamChiTiet?.Gia ?? 0m;
            var giamMax = gioHangChiTiet.SanPhamChiTietId.HasValue
                ? await LayPhanTramGiamToiDaAsync(gioHangChiTiet.SanPhamChiTietId.Value)
                : 0m;
            var donGiaSau = TinhDonGiaSauGiam(giaGoc, giamMax);
            var thanhTienTinhLai = donGiaSau * gioHangChiTiet.SoLuong;

            Console.WriteLine($"🔍 [Repository] ConvertToDTOAsync:");
            Console.WriteLine($"  - Giá gốc: {giaGoc:N0}");
            Console.WriteLine($"  - Giảm tối đa: {giamMax}%");
            Console.WriteLine($"  - Đơn giá sau giảm: {donGiaSau:N0}");
            Console.WriteLine($"  - Số lượng: {gioHangChiTiet.SoLuong}");
            Console.WriteLine($"  - Thành tiền từ DB: {gioHangChiTiet.ThanhTien:N0}");
            Console.WriteLine($"  - Thành tiền tính lại: {thanhTienTinhLai:N0}");
            Console.WriteLine($"  - Kiểm tra: {donGiaSau:N0} × {gioHangChiTiet.SoLuong} = {thanhTienTinhLai:N0}");
            Console.WriteLine($"  - Chênh lệch: {gioHangChiTiet.ThanhTien - thanhTienTinhLai:N0}");

            return new GioHangChiTietDTO
            {
                GioHangChiTietId = gioHangChiTiet.GioHangChiTietId,
                SanPhamId = sanPhamChiTiet != null ? sanPhamChiTiet.SanPhamId : Guid.Empty,
                SanPhamChiTietId = gioHangChiTiet.SanPhamChiTietId ?? Guid.Empty,
                SoLuong = gioHangChiTiet.SoLuong,
                TenSanPham = sanPhamChiTiet?.SanPham?.TenSanPham ?? "Không xác định",
                DonGia = donGiaSau,
                ThanhTien = thanhTienTinhLai, // Sử dụng giá trị tính lại thay vì từ DB
                AnhSanPham = sanPhamChiTiet?.Anh?.DuongDan ?? "",
                MauSac = sanPhamChiTiet?.MauSac?.TenMau ?? "Không xác định",
                KichCo = sanPhamChiTiet?.KichCo?.TenKichCo ?? "Không xác định",
                GiaGoc = giaGoc,
                PhanTramGiam = giamMax
            };
        }

        public async Task<GioHang> GetGioHangEntityByKhachHangIdAsync(Guid khachHangId)
        {
            return await _context.GioHangs
                .Include(g => g.GioHangChiTiets)
                .ThenInclude(gc => gc.SanPhamChiTiet)
                .ThenInclude(spc => spc.SanPham)
                .FirstOrDefaultAsync(g => g.KhachHangId == khachHangId);
        }

        // ✅ Method mới: Sử dụng database lock để tránh race condition
        private async Task<SanPhamChiTiet> GetSanPhamChiTietWithLockAsync(Guid sanPhamChiTietId)
        {
            // Sử dụng raw SQL với UPDLOCK để lock row
            var spct = await _context.SanPhamChiTiets
                .FromSqlRaw("SELECT * FROM SanPhamChiTiets WITH (UPDLOCK) WHERE SanPhamChiTietId = {0}", sanPhamChiTietId)
                .Include(x => x.SanPham)
                    .ThenInclude(sp => sp.ThuongHieu)
                .Include(x => x.MauSac)
                .Include(x => x.KichCo)
                .Include(x => x.Anh)
                .FirstOrDefaultAsync();
            
            return spct;
        }

        // ✅ Method mới: Kiểm tra và trừ tồn kho atomically
        private async Task<bool> TryUpdateProductStockAsync(Guid sanPhamChiTietId, int soLuongCanTru)
        {
            // Sử dụng raw SQL để update atomically với điều kiện
            var affectedRows = await _context.Database.ExecuteSqlRawAsync(
                "UPDATE SanPhamChiTiets SET SoLuong = SoLuong - {0} WHERE SanPhamChiTietId = {1} AND SoLuong >= {0}",
                soLuongCanTru, sanPhamChiTietId);
            
            return affectedRows > 0;
        }

        // ✅ Method test để kiểm tra database lock
        public async Task<string> TestDatabaseLockAsync(Guid sanPhamChiTietId)
        {
            try
            {
                // Lấy sản phẩm với lock
                var spct = await GetSanPhamChiTietWithLockAsync(sanPhamChiTietId);
                if (spct == null)
                    return "Sản phẩm không tồn tại";

                var soLuongBanDau = spct.SoLuong;
                
                // Thử update atomically
                var updateSuccess = await TryUpdateProductStockAsync(sanPhamChiTietId, 1);
                
                if (updateSuccess)
                {
                    // Lấy lại thông tin sản phẩm để kiểm tra
                    var spctSau = await _context.SanPhamChiTiets
                        .FirstOrDefaultAsync(x => x.SanPhamChiTietId == sanPhamChiTietId);
                    
                    return $"✅ Database lock hoạt động tốt! Số lượng: {soLuongBanDau} → {spctSau?.SoLuong}";
                }
                else
                {
                    return $"❌ Không thể update sản phẩm. Số lượng hiện tại: {soLuongBanDau}";
                }
            }
            catch (Exception ex)
            {
                return $"❌ Lỗi: {ex.Message}";
            }
        }

        // ✅ Method mới: Lấy voucher với lock để tránh race condition
        private async Task<Voucher> GetVoucherWithLockAsync(Guid voucherId)
        {
            // Sử dụng raw SQL với UPDLOCK để lock row
            var voucher = await _context.Vouchers
                .FromSqlRaw("SELECT * FROM Vouchers WITH (UPDLOCK) WHERE VoucherId = {0}", voucherId)
                .FirstOrDefaultAsync();
            
            return voucher;
        }

        // ✅ Method mới: Kiểm tra và trừ số lượng voucher atomically
        private async Task<bool> TryUpdateVoucherQuantityAsync(Guid voucherId)
        {
            // Sử dụng raw SQL để update atomically với điều kiện
            var affectedRows = await _context.Database.ExecuteSqlRawAsync(
                "UPDATE Vouchers SET SoLuong = SoLuong - 1 WHERE VoucherId = {0} AND SoLuong > 0",
                voucherId);
            
            return affectedRows > 0;
        }

        // ✅ Method test để kiểm tra voucher lock
        public async Task<string> TestVoucherLockAsync(Guid voucherId)
        {
            try
            {
                // Lấy voucher với lock
                var voucher = await GetVoucherWithLockAsync(voucherId);
                if (voucher == null)
                    return "Voucher không tồn tại";

                var soLuongBanDau = voucher.SoLuong;
                
                // Thử update atomically
                var updateSuccess = await TryUpdateVoucherQuantityAsync(voucherId);
                
                if (updateSuccess)
                {
                    // Lấy lại thông tin voucher để kiểm tra
                    var voucherSau = await _context.Vouchers
                        .FirstOrDefaultAsync(x => x.VoucherId == voucherId);
                    
                    return $"✅ Voucher lock hoạt động tốt! Số lượng: {soLuongBanDau} → {voucherSau?.SoLuong}";
                }
                else
                {
                    return $"❌ Không thể update voucher. Số lượng hiện tại: {soLuongBanDau}";
                }
            }
            catch (Exception ex)
            {
                return $"❌ Lỗi: {ex.Message}";
            }
        }

        public async Task<object> ThanhToanAsync(ThanhToanDTO dto)
        {
            await using var tran = await _context.Database.BeginTransactionAsync();
            try
            {
                // ✅ Ngăn chặn double order: Kiểm tra đơn hàng gần đây (trong 30 giây)
                var thoiGianGanDay = DateTime.Now.AddSeconds(-30);
                var donHangGanDay = await _context.HoaDons
                    .Where(h => h.KhachHangId == dto.KhachHangId && 
                               h.NgayTao >= thoiGianGanDay &&
                               h.TrangThai >= 0) // Chỉ kiểm tra đơn hàng hợp lệ
                    .OrderByDescending(h => h.NgayTao)
                    .FirstOrDefaultAsync();
                
                if (donHangGanDay != null)
                {
                    throw new Exception($"😔 Rất tiếc! Bạn vừa tạo đơn hàng #{donHangGanDay.HoaDonId} cách đây {(DateTime.Now - donHangGanDay.NgayTao).TotalSeconds:F0} giây. Vui lòng chờ một chút trước khi tạo đơn hàng mới.");
                }

                // Lấy giỏ hàng
                var gioHang = await _context.GioHangs
                    .Include(g => g.GioHangChiTiets)
                    .ThenInclude(gc => gc.SanPhamChiTiet)
                    .ThenInclude(spc => spc.SanPham)
                    .FirstOrDefaultAsync(g => g.KhachHangId == dto.KhachHangId);

                if (gioHang == null || !gioHang.GioHangChiTiets.Any())
                    throw new Exception("Giỏ hàng trống");

                // ✅ Kiểm tra tồn kho đủ với database lock
                var sanPhamChiTiets = new List<SanPhamChiTiet>();
                foreach (var item in gioHang.GioHangChiTiets)
                {
                    var spct = await GetSanPhamChiTietWithLockAsync(item.SanPhamChiTietId.Value);
                    
                    if (spct == null || spct.SanPham == null)
                        throw new Exception("Sản phẩm không tồn tại.");

                    // ✅ Kiểm tra trạng thái hoạt động
                    if (spct.TrangThai == 0 || spct.SanPham.TrangThai == false)
                        throw new Exception($"Sản phẩm {spct.SanPham?.TenSanPham ?? "N/A"} hiện không còn hoạt động.");

                    // ✅ Kiểm tra tồn kho
                    if (spct.SoLuong < item.SoLuong)
                        throw new Exception($"Sản phẩm {spct.SanPham?.TenSanPham ?? "N/A"} trong kho không đủ. Hiện tại còn {spct.SoLuong} sản phẩm.");
                    
                    sanPhamChiTiets.Add(spct);
                }

                var soDonChoDuyet = await _context.HoaDons
                    .CountAsync(h => h.KhachHangId == dto.KhachHangId && h.TrangThai == 0);

                if (soDonChoDuyet >= 5)
                {
                    throw new Exception("Bạn đã có 5 đơn hàng đang chờ duyệt, không thể có nhiều hơn 5 đơn cùng lúc.");
                }

                // ✅ Xác định trạng thái dựa trên hình thức thanh toán
                int trangThai;
                var hinhThucThanhToan = await _context.HinhThucThanhToans
                    .FirstOrDefaultAsync(h => h.HinhThucThanhToanId == dto.HinhThucThanhToanId);
                
                if (hinhThucThanhToan != null && 
                    (hinhThucThanhToan.TenHinhThuc.Contains("VNPay", StringComparison.OrdinalIgnoreCase) ||
                     hinhThucThanhToan.TenHinhThuc.Contains("VNPAY", StringComparison.OrdinalIgnoreCase)))
                {
                    // ✅ VNPay → Đã duyệt (1) - Vì đã thanh toán online thành công
                    trangThai = 1;
                }
                else
                {
                    // ✅ Thanh toán thường (COD, chuyển khoản) → Chờ duyệt (0) - Cần admin xác nhận
                    trangThai = 0;
                }

                // Tạo hóa đơn
                var hoaDon = new HoaDon
                {
                    HoaDonId = Guid.NewGuid(),
                    KhachHangId = dto.KhachHangId,
                    NgayTao = DateTime.Now,
                    TrangThai = trangThai, // ✅ Sử dụng trạng thái đã xác định
                    TongTien = 0, // sẽ tính sau theo giá đã giảm
                    TongTienSauKhiGiam = 0,
                    HinhThucThanhToanId = dto.HinhThucThanhToanId,
                    TenCuaKhachHang = dto.TenCuaKhachHang ?? "",
                    SdtCuaKhachHang = dto.SdtCuaKhachHang ?? "",
                    EmailCuaKhachHang = dto.EmailCuaKhachHang ?? "",
                    LoaiHoaDon = dto.LoaiHoaDon ?? "Online", // ✅ Sửa: Bán hàng online phải là "Online"
                    GhiChu = dto.GhiChu ?? "",
                    DiaChiGiaoHangId = dto.DiaChiGiaoHangId,
                    HoaDonChiTiets = new List<HoaDonChiTiet>()
                };

                // ✅ Lưu snapshot địa chỉ giao hàng lúc mua
                var diaChi = await _context.DiaChiKhachHangs.FindAsync(dto.DiaChiGiaoHangId);
                if (diaChi != null)
                {
                    hoaDon.DiaChiGiaoHangLucMua = $"{diaChi.TenDiaChi}, {diaChi.PhuongXa}, {diaChi.ThanhPho}";
                }

                decimal tongSauDotGiam = 0m;
                var gioHangChiTietsList = gioHang.GioHangChiTiets.ToList();
                for (int i = 0; i < gioHangChiTietsList.Count; i++)
                {
                    var gioHangChiTiet = gioHangChiTietsList[i];
                    var spct = sanPhamChiTiets[i]; // Sử dụng sản phẩm đã lock
                    
                    // ✅ Trừ tồn kho atomically
                    var updateSuccess = await TryUpdateProductStockAsync(spct.SanPhamChiTietId, gioHangChiTiet.SoLuong);
                    if (!updateSuccess)
                    {
                        // ✅ Thông báo lỗi thân thiện hơn
                        var tenSanPham = spct.SanPham?.TenSanPham ?? "N/A";
                        var soLuongHienTai = spct.SoLuong;
                        var soLuongCanMua = gioHangChiTiet.SoLuong;
                        
                        if (soLuongHienTai == 0)
                        {
                            throw new Exception($"😔 Rất tiếc! Sản phẩm \"{tenSanPham}\" đã hết hàng. Vui lòng chọn sản phẩm khác hoặc quay lại sau.");
                        }
                        else if (soLuongHienTai < soLuongCanMua)
                        {
                            throw new Exception($"😔 Rất tiếc! Sản phẩm \"{tenSanPham}\" chỉ còn {soLuongHienTai} sản phẩm trong kho, nhưng bạn muốn mua {soLuongCanMua} sản phẩm. Vui lòng giảm số lượng hoặc chọn sản phẩm khác.");
                        }
                        else
                        {
                            throw new Exception($"😔 Rất tiếc! Sản phẩm \"{tenSanPham}\" hiện không đủ số lượng để mua. Có thể có người khác vừa mua sản phẩm này. Vui lòng thử lại hoặc chọn sản phẩm khác.");
                        }
                    }

                    // Tính giá sau giảm theo đợt giảm giá (nếu có) - sử dụng giá hiện tại
                    decimal giaGoc = spct.Gia; // Sử dụng giá hiện tại thay vì giá trong giỏ hàng
                    decimal giamMax = await LayPhanTramGiamToiDaAsync(spct.SanPhamChiTietId);
                    decimal donGiaSau = TinhDonGiaSauGiam(giaGoc, giamMax);
                    tongSauDotGiam += donGiaSau * gioHangChiTiet.SoLuong;

                    // Thêm chi tiết hóa đơn với đơn giá đã giảm và snapshot data
                    var hoaDonChiTiet = new HoaDonChiTiet
                    {
                        HoaDonChiTietId = Guid.NewGuid(),
                        HoaDonId = hoaDon.HoaDonId,
                        SanPhamChiTietId = gioHangChiTiet.SanPhamChiTietId ?? Guid.Empty,
                        SoLuongSanPham = gioHangChiTiet.SoLuong,
                        Gia = donGiaSau,
                        
                        // ✅ Snapshot data - lưu thông tin tại thời điểm mua
                        GiaLucMua = donGiaSau,
                        TenSanPhamLucMua = spct.SanPham?.TenSanPham ?? "N/A",
                        MoTaSanPhamLucMua = spct.MoTa ?? "",
                        ThuongHieuLucMua = spct.SanPham?.ThuongHieu?.TenThuongHieu ?? "N/A",
                        KichCoLucMua = spct.KichCo?.TenKichCo ?? "N/A",
                        MauSacLucMua = spct.MauSac?.TenMau ?? "N/A",
                        AnhSanPhamLucMua = spct.Anh?.DuongDan ?? "",
                        ChatLieuLucMua = "", // Để trống vì model không có trường này
                        ThanhPhanLucMua = "" // Để trống vì model không có trường này
                    };
                    hoaDon.HoaDonChiTiets.Add(hoaDonChiTiet);
                }

                _context.HoaDons.Add(hoaDon);

                // Cập nhật tổng tiền theo giá đã giảm
                hoaDon.TongTien = tongSauDotGiam; // tổng tiền hàng sau giảm giá sản phẩm

                // Xử lý voucher (nếu có)
                decimal tienGiamVoucher = 0m;
                if (dto.VoucherId.HasValue)
                {
                    // ✅ Lấy voucher với lock để tránh race condition
                    var voucher = await GetVoucherWithLockAsync(dto.VoucherId.Value);
                    
                    if (voucher != null)
                    {
                        // ✅ Kiểm tra điều kiện voucher
                        if (voucher.TrangThai == 0)
                        {
                            throw new Exception($"😔 Rất tiếc! Voucher \"{voucher.TenVoucher}\" đang ở trạng thái không hoạt động.");
                        }
                        
                        if (voucher.NgayKetThuc < DateTime.Now)
                        {
                            throw new Exception($"😔 Rất tiếc! Voucher \"{voucher.TenVoucher}\" đã hết hạn sử dụng.");
                        }
                        
                        if (voucher.SoLuong <= 0)
                        {
                            throw new Exception($"😔 Rất tiếc! Voucher \"{voucher.TenVoucher}\" đã hết lượt sử dụng. Có thể có người khác vừa sử dụng voucher này.");
                        }
                        
                        // Tính phí ship dùng cho điều kiện voucher (giống phần preview)
                        var phiShipForEligibility = _voucherService.CalculateShippingFee(tongSauDotGiam, 30000, 500000);
                        var voucherResult = _voucherService.GetVoucherApplication(voucher, tongSauDotGiam, phiShipForEligibility);
                        if (voucherResult.IsValid)
                        {
                            tienGiamVoucher = voucherResult.SoTienGiam;
                            hoaDon.VoucherId = dto.VoucherId;
                            
                            // ✅ Snapshot thông tin voucher lúc mua
                            hoaDon.ThongTinVoucherLucMua = $"{voucher.TenVoucher} - Giảm {voucher.PhanTramGiam}%" +
                                (voucher.GiaTriGiamToiDa.HasValue ? $" (tối đa {voucher.GiaTriGiamToiDa.Value:N0} VNĐ)" : "") +
                                (voucher.SoTienApDungToiThieu.HasValue ? $" - Đơn tối thiểu {voucher.SoTienApDungToiThieu.Value:N0} VNĐ" : "") +
                                $" - Tiết kiệm: {tienGiamVoucher:N0} VNĐ";
                            
                            // ✅ Trừ số lượng voucher atomically
                            var voucherUpdateSuccess = await TryUpdateVoucherQuantityAsync(voucher.VoucherId);
                            if (!voucherUpdateSuccess)
                            {
                                throw new Exception($"😔 Rất tiếc! Voucher \"{voucher.TenVoucher}\" đã hết lượt sử dụng. Có thể có người khác vừa sử dụng voucher này.");
                            }
                        }
                        else
                        {
                            throw new Exception($"😔 Rất tiếc! Voucher \"{voucher.TenVoucher}\" không đủ điều kiện áp dụng cho đơn hàng này.");
                        }
                    }
                    else
                    {
                        throw new Exception("😔 Rất tiếc! Voucher không tồn tại hoặc đã bị xóa.");
                    }
                }

                // Tính phí ship (miễn phí nếu đơn >= 500k sau khi giảm voucher, ngược lại 30k)
                var tongSauVoucher = tongSauDotGiam - tienGiamVoucher;
                var phiShip = tongSauVoucher >= 500000m ? 0m : 30000m;
                
                hoaDon.TongTienSauKhiGiam = tongSauVoucher + phiShip; // tổng thanh toán cuối cùng

                // Xóa giỏ hàng
                _context.GioHangChiTiets.RemoveRange(gioHang.GioHangChiTiets);
                _context.GioHangs.Remove(gioHang);

                await _context.SaveChangesAsync();
                await tran.CommitAsync();

                // ✅ Trả về đầy đủ thông tin cho ThanhToanResultViewModel
                return new
                {
                    success = true,
                    HoaDonId = hoaDon.HoaDonId,
                    TenCuaKhachHang = hoaDon.TenCuaKhachHang,
                    SdtCuaKhachHang = hoaDon.SdtCuaKhachHang,
                    EmailCuaKhachHang = hoaDon.EmailCuaKhachHang,
                    DiaChiCuaKhachHang = hoaDon.DiaChiGiaoHangLucMua,
                    NgayTao = hoaDon.NgayTao,
                    HinhThucThanhToan = hinhThucThanhToan?.TenHinhThuc ?? "",
                    GhiChu = hoaDon.GhiChu,
                    TongTien = hoaDon.TongTien,
                    TongTienSauKhiGiam = hoaDon.TongTienSauKhiGiam,
                    ChiTietSanPham = hoaDon.HoaDonChiTiets.Select(ct => new
                    {
                        TenSanPhamLucMua = ct.TenSanPhamLucMua,
                        MoTaSanPhamLucMua = ct.MoTaSanPhamLucMua,
                        ThuongHieuLucMua = ct.ThuongHieuLucMua,
                        KichCoLucMua = ct.KichCoLucMua,
                        MauSacLucMua = ct.MauSacLucMua,
                        AnhSanPhamLucMua = ct.AnhSanPhamLucMua,
                        ChatLieuLucMua = ct.ChatLieuLucMua,
                        ThanhPhanLucMua = ct.ThanhPhanLucMua,
                        SoLuong = ct.SoLuongSanPham,
                        GiaLucMua = ct.GiaLucMua
                    }).ToList()
                };
            }
            catch
            {
                await tran.RollbackAsync();
                throw;
            }
        }

        // ✅ Method mới: Kiểm tra và xóa đơn hàng trùng lặp
        public async Task<string> KiemTraVaXoaDonTrungLapAsync(Guid khachHangId)
        {
            try
            {
                // Tìm các đơn hàng trùng lặp (cùng khách hàng, cùng thời gian, cùng trạng thái)
                var thoiGianGanDay = DateTime.Now.AddMinutes(-5); // 5 phút gần đây
                
                var donHangTrungLap = await _context.HoaDons
                    .Where(h => h.KhachHangId == khachHangId && 
                               h.NgayTao >= thoiGianGanDay &&
                               h.TrangThai >= 0)
                    .GroupBy(h => new { h.KhachHangId, h.TongTien, h.HinhThucThanhToanId })
                    .Where(g => g.Count() > 1)
                    .SelectMany(g => g.OrderByDescending(h => h.NgayTao).Skip(1)) // Giữ lại đơn hàng mới nhất, xóa các đơn cũ hơn
                    .ToListAsync();
                
                if (donHangTrungLap.Any())
                {
                    var soDonXoa = donHangTrungLap.Count;
                    var donHangIds = donHangTrungLap.Select(h => h.HoaDonId).ToList();
                    
                    // Xóa các đơn hàng trùng lặp
                    _context.HoaDons.RemoveRange(donHangTrungLap);
                    await _context.SaveChangesAsync();
                    
                    return $"✅ Đã xóa {soDonXoa} đơn hàng trùng lặp: {string.Join(", ", donHangIds)}";
                }
                
                return "✅ Không có đơn hàng trùng lặp";
            }
            catch (Exception ex)
            {
                return $"❌ Lỗi khi kiểm tra đơn hàng trùng lặp: {ex.Message}";
            }
        }
    }
}
