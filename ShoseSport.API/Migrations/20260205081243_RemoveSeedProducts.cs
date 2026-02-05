using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FurryFriends.API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSeedProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SanPhamChatLieus",
                keyColumn: "SanPhamChatLieuId",
                keyValue: new Guid("027f9d3e-048b-41a6-9b20-beb1947c340b"));

            migrationBuilder.DeleteData(
                table: "SanPhamChatLieus",
                keyColumn: "SanPhamChatLieuId",
                keyValue: new Guid("f7072688-ee0c-4e56-9b02-d8bd81e3ea88"));

            migrationBuilder.DeleteData(
                table: "SanPhamChiTiets",
                keyColumn: "SanPhamChiTietId",
                keyValue: new Guid("eeee1111-ffff-0000-1111-222233334444"));

            migrationBuilder.DeleteData(
                table: "SanPhamChiTiets",
                keyColumn: "SanPhamChiTietId",
                keyValue: new Guid("eeee2222-ffff-0000-1111-222233334444"));

            migrationBuilder.DeleteData(
                table: "Anhs",
                keyColumn: "AnhId",
                keyValue: new Guid("ffff1111-0000-1111-2222-333344445555"));

            migrationBuilder.DeleteData(
                table: "Anhs",
                keyColumn: "AnhId",
                keyValue: new Guid("ffff2222-0000-1111-2222-333344445555"));

            migrationBuilder.DeleteData(
                table: "ChatLieus",
                keyColumn: "ChatLieuId",
                keyValue: new Guid("cccc1111-dddd-eeee-ffff-000011112222"));

            migrationBuilder.DeleteData(
                table: "ChatLieus",
                keyColumn: "ChatLieuId",
                keyValue: new Guid("cccc2222-dddd-eeee-ffff-000011112222"));

            migrationBuilder.DeleteData(
                table: "KichCos",
                keyColumn: "KichCoId",
                keyValue: new Guid("bbbb1111-cccc-dddd-eeee-ffff00001111"));

            migrationBuilder.DeleteData(
                table: "KichCos",
                keyColumn: "KichCoId",
                keyValue: new Guid("bbbb2222-cccc-dddd-eeee-ffff00001111"));

            migrationBuilder.DeleteData(
                table: "MauSacs",
                keyColumn: "MauSacId",
                keyValue: new Guid("aaaa1111-bbbb-cccc-dddd-eeeeffff0000"));

            migrationBuilder.DeleteData(
                table: "MauSacs",
                keyColumn: "MauSacId",
                keyValue: new Guid("aaaa2222-bbbb-cccc-dddd-eeeeffff0000"));

            migrationBuilder.DeleteData(
                table: "SanPhams",
                keyColumn: "SanPhamId",
                keyValue: new Guid("dddd1111-eeee-ffff-0000-111122223333"));

            migrationBuilder.DeleteData(
                table: "SanPhams",
                keyColumn: "SanPhamId",
                keyValue: new Guid("dddd2222-eeee-ffff-0000-111122223333"));

            migrationBuilder.DeleteData(
                table: "ThuongHieus",
                keyColumn: "ThuongHieuId",
                keyValue: new Guid("11112222-3333-4444-5555-666677778888"));

            migrationBuilder.DeleteData(
                table: "ThuongHieus",
                keyColumn: "ThuongHieuId",
                keyValue: new Guid("99998888-7777-6666-5555-444433332222"));

            migrationBuilder.UpdateData(
                table: "ChucVus",
                keyColumn: "ChucVuId",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2026, 2, 5, 8, 12, 43, 34, DateTimeKind.Utc).AddTicks(4945), new DateTime(2026, 2, 5, 8, 12, 43, 34, DateTimeKind.Utc).AddTicks(4945) });

            migrationBuilder.UpdateData(
                table: "ChucVus",
                keyColumn: "ChucVuId",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2026, 2, 5, 8, 12, 43, 34, DateTimeKind.Utc).AddTicks(4970), new DateTime(2026, 2, 5, 8, 12, 43, 34, DateTimeKind.Utc).AddTicks(4969) });

            migrationBuilder.UpdateData(
                table: "KhachHangs",
                keyColumn: "KhachHangId",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                columns: new[] { "NgayCapNhatCuoiCung", "NgayTaoTaiKhoan" },
                values: new object[] { new DateTime(2026, 2, 5, 8, 12, 43, 34, DateTimeKind.Utc).AddTicks(4446), new DateTime(2026, 2, 5, 8, 12, 43, 34, DateTimeKind.Utc).AddTicks(4439) });

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "NhanVienId",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2026, 2, 5, 8, 12, 43, 34, DateTimeKind.Utc).AddTicks(5009), new DateTime(2026, 2, 5, 8, 12, 43, 34, DateTimeKind.Utc).AddTicks(5008) });

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "NhanVienId",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2026, 2, 5, 8, 12, 43, 34, DateTimeKind.Utc).AddTicks(5031), new DateTime(2026, 2, 5, 8, 12, 43, 34, DateTimeKind.Utc).AddTicks(5031) });

            migrationBuilder.UpdateData(
                table: "TaiKhoans",
                keyColumn: "TaiKhoanId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "NgayTaoTaiKhoan",
                value: new DateTime(2026, 2, 5, 8, 12, 43, 34, DateTimeKind.Utc).AddTicks(4819));

            migrationBuilder.UpdateData(
                table: "TaiKhoans",
                keyColumn: "TaiKhoanId",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "NgayTaoTaiKhoan",
                value: new DateTime(2026, 2, 5, 8, 12, 43, 34, DateTimeKind.Utc).AddTicks(4898));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Anhs",
                columns: new[] { "AnhId", "DuongDan", "SanPhamChiTietId", "TenAnh", "TrangThai" },
                values: new object[,]
                {
                    { new Guid("ffff1111-0000-1111-2222-333344445555"), "/images/products/nike-revolution-6.jpg", new Guid("eeee1111-ffff-0000-1111-222233334444"), "nike_rev_6.jpg", true },
                    { new Guid("ffff2222-0000-1111-2222-333344445555"), "/images/products/bata-oxford.jpg", new Guid("eeee2222-ffff-0000-1111-222233334444"), "bata_oxford.jpg", true }
                });

            migrationBuilder.InsertData(
                table: "ChatLieus",
                columns: new[] { "ChatLieuId", "MoTa", "TenChatLieu", "TrangThai" },
                values: new object[,]
                {
                    { new Guid("cccc1111-dddd-eeee-ffff-000011112222"), null, "Vải", true },
                    { new Guid("cccc2222-dddd-eeee-ffff-000011112222"), null, "Da", true }
                });

            migrationBuilder.UpdateData(
                table: "ChucVus",
                keyColumn: "ChucVuId",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2026, 2, 5, 7, 55, 20, 224, DateTimeKind.Utc).AddTicks(2888), new DateTime(2026, 2, 5, 7, 55, 20, 224, DateTimeKind.Utc).AddTicks(2887) });

            migrationBuilder.UpdateData(
                table: "ChucVus",
                keyColumn: "ChucVuId",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2026, 2, 5, 7, 55, 20, 224, DateTimeKind.Utc).AddTicks(2938), new DateTime(2026, 2, 5, 7, 55, 20, 224, DateTimeKind.Utc).AddTicks(2937) });

            migrationBuilder.UpdateData(
                table: "KhachHangs",
                keyColumn: "KhachHangId",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                columns: new[] { "NgayCapNhatCuoiCung", "NgayTaoTaiKhoan" },
                values: new object[] { new DateTime(2026, 2, 5, 7, 55, 20, 224, DateTimeKind.Utc).AddTicks(2072), new DateTime(2026, 2, 5, 7, 55, 20, 224, DateTimeKind.Utc).AddTicks(2069) });

            migrationBuilder.InsertData(
                table: "KichCos",
                columns: new[] { "KichCoId", "MoTa", "TenKichCo", "TrangThai" },
                values: new object[,]
                {
                    { new Guid("bbbb1111-cccc-dddd-eeee-ffff00001111"), null, "40", true },
                    { new Guid("bbbb2222-cccc-dddd-eeee-ffff00001111"), null, "41", true }
                });

            migrationBuilder.InsertData(
                table: "MauSacs",
                columns: new[] { "MauSacId", "MaMau", "MoTa", "TenMau", "TrangThai" },
                values: new object[,]
                {
                    { new Guid("aaaa1111-bbbb-cccc-dddd-eeeeffff0000"), "#000000", null, "Đen", true },
                    { new Guid("aaaa2222-bbbb-cccc-dddd-eeeeffff0000"), "#FFFFFF", null, "Trắng", true }
                });

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "NhanVienId",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2026, 2, 5, 7, 55, 20, 224, DateTimeKind.Utc).AddTicks(2991), new DateTime(2026, 2, 5, 7, 55, 20, 224, DateTimeKind.Utc).AddTicks(2990) });

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "NhanVienId",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2026, 2, 5, 7, 55, 20, 224, DateTimeKind.Utc).AddTicks(3041), new DateTime(2026, 2, 5, 7, 55, 20, 224, DateTimeKind.Utc).AddTicks(3040) });

            migrationBuilder.UpdateData(
                table: "TaiKhoans",
                keyColumn: "TaiKhoanId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "NgayTaoTaiKhoan",
                value: new DateTime(2026, 2, 5, 7, 55, 20, 224, DateTimeKind.Utc).AddTicks(2741));

            migrationBuilder.UpdateData(
                table: "TaiKhoans",
                keyColumn: "TaiKhoanId",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "NgayTaoTaiKhoan",
                value: new DateTime(2026, 2, 5, 7, 55, 20, 224, DateTimeKind.Utc).AddTicks(2815));

            migrationBuilder.InsertData(
                table: "ThuongHieus",
                columns: new[] { "ThuongHieuId", "DiaChi", "Email", "MoTa", "SDT", "TenThuongHieu", "TrangThai" },
                values: new object[,]
                {
                    { new Guid("11112222-3333-4444-5555-666677778888"), null, "contact@nike.com", null, "18001234", "Nike", true },
                    { new Guid("99998888-7777-6666-5555-444433332222"), null, "contact@bata.com", null, "18005678", "Bata", true }
                });

            migrationBuilder.InsertData(
                table: "SanPhams",
                columns: new[] { "SanPhamId", "HanSuDung", "Loai", "TenSanPham", "ThuongHieuId", "TrangThai" },
                values: new object[,]
                {
                    { new Guid("dddd1111-eeee-ffff-0000-111122223333"), null, "GiayTheThao", "Nike Revolution 6", new Guid("11112222-3333-4444-5555-666677778888"), true },
                    { new Guid("dddd2222-eeee-ffff-0000-111122223333"), null, "GiayTay", "Giày Tây Oxford Bata", new Guid("99998888-7777-6666-5555-444433332222"), true }
                });

            migrationBuilder.InsertData(
                table: "SanPhamChatLieus",
                columns: new[] { "SanPhamChatLieuId", "ChatLieuId", "SanPhamId" },
                values: new object[,]
                {
                    { new Guid("027f9d3e-048b-41a6-9b20-beb1947c340b"), new Guid("cccc2222-dddd-eeee-ffff-000011112222"), new Guid("dddd2222-eeee-ffff-0000-111122223333") },
                    { new Guid("f7072688-ee0c-4e56-9b02-d8bd81e3ea88"), new Guid("cccc1111-dddd-eeee-ffff-000011112222"), new Guid("dddd1111-eeee-ffff-0000-111122223333") }
                });

            migrationBuilder.InsertData(
                table: "SanPhamChiTiets",
                columns: new[] { "SanPhamChiTietId", "AnhId", "Gia", "GiaNhap", "KichCoId", "MauSacId", "MoTa", "NgaySua", "NgayTao", "SanPhamId", "SoLuong", "TrangThai" },
                values: new object[,]
                {
                    { new Guid("eeee1111-ffff-0000-1111-222233334444"), new Guid("ffff1111-0000-1111-2222-333344445555"), 1500000m, null, new Guid("bbbb1111-cccc-dddd-eeee-ffff00001111"), new Guid("aaaa1111-bbbb-cccc-dddd-eeeeffff0000"), null, null, new DateTime(2026, 2, 5, 7, 55, 20, 224, DateTimeKind.Utc).AddTicks(3723), new Guid("dddd1111-eeee-ffff-0000-111122223333"), 100, 1 },
                    { new Guid("eeee2222-ffff-0000-1111-222233334444"), new Guid("ffff2222-0000-1111-2222-333344445555"), 2000000m, null, new Guid("bbbb2222-cccc-dddd-eeee-ffff00001111"), new Guid("aaaa2222-bbbb-cccc-dddd-eeeeffff0000"), null, null, new DateTime(2026, 2, 5, 7, 55, 20, 224, DateTimeKind.Utc).AddTicks(3730), new Guid("dddd2222-eeee-ffff-0000-111122223333"), 50, 1 }
                });
        }
    }
}
