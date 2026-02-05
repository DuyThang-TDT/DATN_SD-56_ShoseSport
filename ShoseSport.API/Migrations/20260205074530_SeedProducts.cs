using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FurryFriends.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                values: new object[] { new DateTime(2026, 2, 5, 7, 45, 29, 130, DateTimeKind.Utc).AddTicks(9858), new DateTime(2026, 2, 5, 7, 45, 29, 130, DateTimeKind.Utc).AddTicks(9857) });

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "NhanVienId",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2026, 2, 5, 7, 45, 29, 130, DateTimeKind.Utc).AddTicks(9889), new DateTime(2026, 2, 5, 7, 45, 29, 130, DateTimeKind.Utc).AddTicks(9888) });

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
                    { new Guid("b62f81b0-a5cc-4e4e-bfe0-801cf6e8b0ae"), new Guid("cccc2222-dddd-eeee-ffff-000011112222"), new Guid("dddd2222-eeee-ffff-0000-111122223333") },
                    { new Guid("e1e7e598-e795-4e2d-ace2-350d308fa430"), new Guid("cccc1111-dddd-eeee-ffff-000011112222"), new Guid("dddd1111-eeee-ffff-0000-111122223333") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                table: "SanPhamChatLieus",
                keyColumn: "SanPhamChatLieuId",
                keyValue: new Guid("b62f81b0-a5cc-4e4e-bfe0-801cf6e8b0ae"));

            migrationBuilder.DeleteData(
                table: "SanPhamChatLieus",
                keyColumn: "SanPhamChatLieuId",
                keyValue: new Guid("e1e7e598-e795-4e2d-ace2-350d308fa430"));

            migrationBuilder.DeleteData(
                table: "ChatLieus",
                keyColumn: "ChatLieuId",
                keyValue: new Guid("cccc1111-dddd-eeee-ffff-000011112222"));

            migrationBuilder.DeleteData(
                table: "ChatLieus",
                keyColumn: "ChatLieuId",
                keyValue: new Guid("cccc2222-dddd-eeee-ffff-000011112222"));

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
                values: new object[] { new DateTime(2026, 2, 5, 7, 37, 17, 229, DateTimeKind.Utc).AddTicks(3745), new DateTime(2026, 2, 5, 7, 37, 17, 229, DateTimeKind.Utc).AddTicks(3744) });

            migrationBuilder.UpdateData(
                table: "ChucVus",
                keyColumn: "ChucVuId",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2026, 2, 5, 7, 37, 17, 229, DateTimeKind.Utc).AddTicks(3768), new DateTime(2026, 2, 5, 7, 37, 17, 229, DateTimeKind.Utc).AddTicks(3768) });

            migrationBuilder.UpdateData(
                table: "KhachHangs",
                keyColumn: "KhachHangId",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                columns: new[] { "NgayCapNhatCuoiCung", "NgayTaoTaiKhoan" },
                values: new object[] { new DateTime(2026, 2, 5, 7, 37, 17, 229, DateTimeKind.Utc).AddTicks(3434), new DateTime(2026, 2, 5, 7, 37, 17, 229, DateTimeKind.Utc).AddTicks(3432) });

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "NhanVienId",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2026, 2, 5, 7, 37, 17, 229, DateTimeKind.Utc).AddTicks(3804), new DateTime(2026, 2, 5, 7, 37, 17, 229, DateTimeKind.Utc).AddTicks(3803) });

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "NhanVienId",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2026, 2, 5, 7, 37, 17, 229, DateTimeKind.Utc).AddTicks(3826), new DateTime(2026, 2, 5, 7, 37, 17, 229, DateTimeKind.Utc).AddTicks(3825) });

            migrationBuilder.UpdateData(
                table: "TaiKhoans",
                keyColumn: "TaiKhoanId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "NgayTaoTaiKhoan",
                value: new DateTime(2026, 2, 5, 7, 37, 17, 229, DateTimeKind.Utc).AddTicks(3617));

            migrationBuilder.UpdateData(
                table: "TaiKhoans",
                keyColumn: "TaiKhoanId",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "NgayTaoTaiKhoan",
                value: new DateTime(2026, 2, 5, 7, 37, 17, 229, DateTimeKind.Utc).AddTicks(3707));
        }
    }
}
