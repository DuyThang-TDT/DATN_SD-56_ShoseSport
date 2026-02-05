using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FurryFriends.API.Migrations
{
    /// <inheritdoc />
    public partial class LinkImageToProductDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SanPhamChatLieus",
                keyColumn: "SanPhamChatLieuId",
                keyValue: new Guid("84d68c5c-b8f9-4ce6-baf7-960a53afaa40"));

            migrationBuilder.DeleteData(
                table: "SanPhamChatLieus",
                keyColumn: "SanPhamChatLieuId",
                keyValue: new Guid("ccfcd2a9-a215-47ac-a6c4-0712501193a5"));

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

            migrationBuilder.InsertData(
                table: "SanPhamChatLieus",
                columns: new[] { "SanPhamChatLieuId", "ChatLieuId", "SanPhamId" },
                values: new object[,]
                {
                    { new Guid("027f9d3e-048b-41a6-9b20-beb1947c340b"), new Guid("cccc2222-dddd-eeee-ffff-000011112222"), new Guid("dddd2222-eeee-ffff-0000-111122223333") },
                    { new Guid("f7072688-ee0c-4e56-9b02-d8bd81e3ea88"), new Guid("cccc1111-dddd-eeee-ffff-000011112222"), new Guid("dddd1111-eeee-ffff-0000-111122223333") }
                });

            migrationBuilder.UpdateData(
                table: "SanPhamChiTiets",
                keyColumn: "SanPhamChiTietId",
                keyValue: new Guid("eeee1111-ffff-0000-1111-222233334444"),
                columns: new[] { "AnhId", "NgayTao" },
                values: new object[] { new Guid("ffff1111-0000-1111-2222-333344445555"), new DateTime(2026, 2, 5, 7, 55, 20, 224, DateTimeKind.Utc).AddTicks(3723) });

            migrationBuilder.UpdateData(
                table: "SanPhamChiTiets",
                keyColumn: "SanPhamChiTietId",
                keyValue: new Guid("eeee2222-ffff-0000-1111-222233334444"),
                columns: new[] { "AnhId", "NgayTao" },
                values: new object[] { new Guid("ffff2222-0000-1111-2222-333344445555"), new DateTime(2026, 2, 5, 7, 55, 20, 224, DateTimeKind.Utc).AddTicks(3730) });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SanPhamChatLieus",
                keyColumn: "SanPhamChatLieuId",
                keyValue: new Guid("027f9d3e-048b-41a6-9b20-beb1947c340b"));

            migrationBuilder.DeleteData(
                table: "SanPhamChatLieus",
                keyColumn: "SanPhamChatLieuId",
                keyValue: new Guid("f7072688-ee0c-4e56-9b02-d8bd81e3ea88"));

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

            migrationBuilder.UpdateData(
                table: "SanPhamChiTiets",
                keyColumn: "SanPhamChiTietId",
                keyValue: new Guid("eeee1111-ffff-0000-1111-222233334444"),
                columns: new[] { "AnhId", "NgayTao" },
                values: new object[] { null, new DateTime(2026, 2, 5, 7, 49, 22, 612, DateTimeKind.Utc).AddTicks(1723) });

            migrationBuilder.UpdateData(
                table: "SanPhamChiTiets",
                keyColumn: "SanPhamChiTietId",
                keyValue: new Guid("eeee2222-ffff-0000-1111-222233334444"),
                columns: new[] { "AnhId", "NgayTao" },
                values: new object[] { null, new DateTime(2026, 2, 5, 7, 49, 22, 612, DateTimeKind.Utc).AddTicks(1725) });

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
    }
}
