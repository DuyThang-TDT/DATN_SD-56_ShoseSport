using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FurryFriends.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedProductDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SanPhamChatLieus",
                keyColumn: "SanPhamChatLieuId",
                keyValue: new Guid("b62f81b0-a5cc-4e4e-bfe0-801cf6e8b0ae"));

            migrationBuilder.DeleteData(
                table: "SanPhamChatLieus",
                keyColumn: "SanPhamChatLieuId",
                keyValue: new Guid("e1e7e598-e795-4e2d-ace2-350d308fa430"));

            migrationBuilder.InsertData(
                table: "Anhs",
                columns: new[] { "AnhId", "DuongDan", "SanPhamChiTietId", "TenAnh", "TrangThai" },
                values: new object[,]
                {
                    { new Guid("ffff1111-0000-1111-2222-333344445555"), "/images/products/nike-revolution-6.jpg", new Guid("eeee1111-ffff-0000-1111-222233334444"), "nike_rev_6.jpg", true },
                    { new Guid("ffff2222-0000-1111-2222-333344445555"), "/images/products/bata-oxford.jpg", new Guid("eeee2222-ffff-0000-1111-222233334444"), "bata_oxford.jpg", true }
                });

            migrationBuilder.UpdateData(
                table: "ChucVus",
                keyColumn: "ChucVuId",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2026, 2, 5, 7, 49, 22, 612, DateTimeKind.Utc).AddTicks(1230), new DateTime(2026, 2, 5, 7, 49, 22, 612, DateTimeKind.Utc).AddTicks(1229) });

            migrationBuilder.UpdateData(
                table: "ChucVus",
                keyColumn: "ChucVuId",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2026, 2, 5, 7, 49, 22, 612, DateTimeKind.Utc).AddTicks(1255), new DateTime(2026, 2, 5, 7, 49, 22, 612, DateTimeKind.Utc).AddTicks(1255) });

            migrationBuilder.UpdateData(
                table: "KhachHangs",
                keyColumn: "KhachHangId",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                columns: new[] { "NgayCapNhatCuoiCung", "NgayTaoTaiKhoan" },
                values: new object[] { new DateTime(2026, 2, 5, 7, 49, 22, 612, DateTimeKind.Utc).AddTicks(804), new DateTime(2026, 2, 5, 7, 49, 22, 612, DateTimeKind.Utc).AddTicks(798) });

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "NhanVienId",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2026, 2, 5, 7, 49, 22, 612, DateTimeKind.Utc).AddTicks(1297), new DateTime(2026, 2, 5, 7, 49, 22, 612, DateTimeKind.Utc).AddTicks(1297) });

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "NhanVienId",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2026, 2, 5, 7, 49, 22, 612, DateTimeKind.Utc).AddTicks(1319), new DateTime(2026, 2, 5, 7, 49, 22, 612, DateTimeKind.Utc).AddTicks(1319) });

            migrationBuilder.InsertData(
                table: "SanPhamChatLieus",
                columns: new[] { "SanPhamChatLieuId", "ChatLieuId", "SanPhamId" },
                values: new object[,]
                {
                    { new Guid("84d68c5c-b8f9-4ce6-baf7-960a53afaa40"), new Guid("cccc1111-dddd-eeee-ffff-000011112222"), new Guid("dddd1111-eeee-ffff-0000-111122223333") },
                    { new Guid("ccfcd2a9-a215-47ac-a6c4-0712501193a5"), new Guid("cccc2222-dddd-eeee-ffff-000011112222"), new Guid("dddd2222-eeee-ffff-0000-111122223333") }
                });

            migrationBuilder.InsertData(
                table: "SanPhamChiTiets",
                columns: new[] { "SanPhamChiTietId", "AnhId", "Gia", "GiaNhap", "KichCoId", "MauSacId", "MoTa", "NgaySua", "NgayTao", "SanPhamId", "SoLuong", "TrangThai" },
                values: new object[,]
                {
                    { new Guid("eeee1111-ffff-0000-1111-222233334444"), null, 1500000m, null, new Guid("bbbb1111-cccc-dddd-eeee-ffff00001111"), new Guid("aaaa1111-bbbb-cccc-dddd-eeeeffff0000"), null, null, new DateTime(2026, 2, 5, 7, 49, 22, 612, DateTimeKind.Utc).AddTicks(1723), new Guid("dddd1111-eeee-ffff-0000-111122223333"), 100, 1 },
                    { new Guid("eeee2222-ffff-0000-1111-222233334444"), null, 2000000m, null, new Guid("bbbb2222-cccc-dddd-eeee-ffff00001111"), new Guid("aaaa2222-bbbb-cccc-dddd-eeeeffff0000"), null, null, new DateTime(2026, 2, 5, 7, 49, 22, 612, DateTimeKind.Utc).AddTicks(1725), new Guid("dddd2222-eeee-ffff-0000-111122223333"), 50, 1 }
                });

            migrationBuilder.UpdateData(
                table: "TaiKhoans",
                keyColumn: "TaiKhoanId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "NgayTaoTaiKhoan",
                value: new DateTime(2026, 2, 5, 7, 49, 22, 612, DateTimeKind.Utc).AddTicks(1163));

            migrationBuilder.UpdateData(
                table: "TaiKhoans",
                keyColumn: "TaiKhoanId",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "NgayTaoTaiKhoan",
                value: new DateTime(2026, 2, 5, 7, 49, 22, 612, DateTimeKind.Utc).AddTicks(1190));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Anhs",
                keyColumn: "AnhId",
                keyValue: new Guid("ffff1111-0000-1111-2222-333344445555"));

            migrationBuilder.DeleteData(
                table: "Anhs",
                keyColumn: "AnhId",
                keyValue: new Guid("ffff2222-0000-1111-2222-333344445555"));

            migrationBuilder.DeleteData(
                table: "SanPhamChatLieus",
                keyColumn: "SanPhamChatLieuId",
                keyValue: new Guid("84d68c5c-b8f9-4ce6-baf7-960a53afaa40"));

            migrationBuilder.DeleteData(
                table: "SanPhamChatLieus",
                keyColumn: "SanPhamChatLieuId",
                keyValue: new Guid("ccfcd2a9-a215-47ac-a6c4-0712501193a5"));

            migrationBuilder.DeleteData(
                table: "SanPhamChiTiets",
                keyColumn: "SanPhamChiTietId",
                keyValue: new Guid("eeee1111-ffff-0000-1111-222233334444"));

            migrationBuilder.DeleteData(
                table: "SanPhamChiTiets",
                keyColumn: "SanPhamChiTietId",
                keyValue: new Guid("eeee2222-ffff-0000-1111-222233334444"));

            migrationBuilder.UpdateData(
                table: "ChucVus",
                keyColumn: "ChucVuId",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2026, 2, 5, 7, 45, 29, 130, DateTimeKind.Utc).AddTicks(9782), new DateTime(2026, 2, 5, 7, 45, 29, 130, DateTimeKind.Utc).AddTicks(9782) });

            migrationBuilder.UpdateData(
                table: "ChucVus",
                keyColumn: "ChucVuId",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2026, 2, 5, 7, 45, 29, 130, DateTimeKind.Utc).AddTicks(9817), new DateTime(2026, 2, 5, 7, 45, 29, 130, DateTimeKind.Utc).AddTicks(9816) });

            migrationBuilder.UpdateData(
                table: "KhachHangs",
                keyColumn: "KhachHangId",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                columns: new[] { "NgayCapNhatCuoiCung", "NgayTaoTaiKhoan" },
                values: new object[] { new DateTime(2026, 2, 5, 7, 45, 29, 130, DateTimeKind.Utc).AddTicks(9232), new DateTime(2026, 2, 5, 7, 45, 29, 130, DateTimeKind.Utc).AddTicks(9229) });

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "NhanVienId",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2026, 2, 5, 7, 45, 29, 130, DateTimeKind.Utc).AddTicks(9858), new DateTime(2026, 2, 5, 7, 45, 29, 130, DateTimeKind.Utc).AddTicks(9857) });

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "NhanVienId",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2026, 2, 5, 7, 45, 29, 130, DateTimeKind.Utc).AddTicks(9889), new DateTime(2026, 2, 5, 7, 45, 29, 130, DateTimeKind.Utc).AddTicks(9888) });

            migrationBuilder.InsertData(
                table: "SanPhamChatLieus",
                columns: new[] { "SanPhamChatLieuId", "ChatLieuId", "SanPhamId" },
                values: new object[,]
                {
                    { new Guid("b62f81b0-a5cc-4e4e-bfe0-801cf6e8b0ae"), new Guid("cccc2222-dddd-eeee-ffff-000011112222"), new Guid("dddd2222-eeee-ffff-0000-111122223333") },
                    { new Guid("e1e7e598-e795-4e2d-ace2-350d308fa430"), new Guid("cccc1111-dddd-eeee-ffff-000011112222"), new Guid("dddd1111-eeee-ffff-0000-111122223333") }
                });

            migrationBuilder.UpdateData(
                table: "TaiKhoans",
                keyColumn: "TaiKhoanId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "NgayTaoTaiKhoan",
                value: new DateTime(2026, 2, 5, 7, 45, 29, 130, DateTimeKind.Utc).AddTicks(9696));

            migrationBuilder.UpdateData(
                table: "TaiKhoans",
                keyColumn: "TaiKhoanId",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "NgayTaoTaiKhoan",
                value: new DateTime(2026, 2, 5, 7, 45, 29, 130, DateTimeKind.Utc).AddTicks(9731));
        }
    }
}
