using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurryFriends.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ChucVus",
                keyColumn: "ChucVuId",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2026, 2, 3, 13, 26, 52, 699, DateTimeKind.Utc).AddTicks(5549), new DateTime(2026, 2, 3, 13, 26, 52, 699, DateTimeKind.Utc).AddTicks(5549) });

            migrationBuilder.UpdateData(
                table: "ChucVus",
                keyColumn: "ChucVuId",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2026, 2, 3, 13, 26, 52, 699, DateTimeKind.Utc).AddTicks(5573), new DateTime(2026, 2, 3, 13, 26, 52, 699, DateTimeKind.Utc).AddTicks(5573) });

            migrationBuilder.InsertData(
                table: "KhachHangs",
                columns: new[] { "KhachHangId", "DiemKhachHang", "EmailCuaKhachHang", "NgayCapNhatCuoiCung", "NgayTaoTaiKhoan", "SDT", "TaiKhoanId", "TenKhachHang", "TrangThai" },
                values: new object[] { new Guid("99999999-9999-9999-9999-999999999999"), null, "admin@store.com", new DateTime(2026, 2, 3, 13, 26, 52, 699, DateTimeKind.Utc).AddTicks(5059), new DateTime(2026, 2, 3, 13, 26, 52, 699, DateTimeKind.Utc).AddTicks(5057), "0123456789", null, "Admin Customer", 1 });

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "NhanVienId",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2026, 2, 3, 13, 26, 52, 699, DateTimeKind.Utc).AddTicks(5607), new DateTime(2026, 2, 3, 13, 26, 52, 699, DateTimeKind.Utc).AddTicks(5606) });

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "NhanVienId",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2026, 2, 3, 13, 26, 52, 699, DateTimeKind.Utc).AddTicks(5630), new DateTime(2026, 2, 3, 13, 26, 52, 699, DateTimeKind.Utc).AddTicks(5630) });

            migrationBuilder.UpdateData(
                table: "TaiKhoans",
                keyColumn: "TaiKhoanId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "KhachHangId", "NgayTaoTaiKhoan" },
                values: new object[] { new Guid("99999999-9999-9999-9999-999999999999"), new DateTime(2026, 2, 3, 13, 26, 52, 699, DateTimeKind.Utc).AddTicks(5424) });

            migrationBuilder.UpdateData(
                table: "TaiKhoans",
                keyColumn: "TaiKhoanId",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "NgayTaoTaiKhoan",
                value: new DateTime(2026, 2, 3, 13, 26, 52, 699, DateTimeKind.Utc).AddTicks(5505));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "KhachHangs",
                keyColumn: "KhachHangId",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"));

            migrationBuilder.UpdateData(
                table: "ChucVus",
                keyColumn: "ChucVuId",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2025, 12, 8, 14, 45, 39, 711, DateTimeKind.Utc).AddTicks(5093), new DateTime(2025, 12, 8, 14, 45, 39, 711, DateTimeKind.Utc).AddTicks(5093) });

            migrationBuilder.UpdateData(
                table: "ChucVus",
                keyColumn: "ChucVuId",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2025, 12, 8, 14, 45, 39, 711, DateTimeKind.Utc).AddTicks(5108), new DateTime(2025, 12, 8, 14, 45, 39, 711, DateTimeKind.Utc).AddTicks(5107) });

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "NhanVienId",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2025, 12, 8, 14, 45, 39, 711, DateTimeKind.Utc).AddTicks(5133), new DateTime(2025, 12, 8, 14, 45, 39, 711, DateTimeKind.Utc).AddTicks(5132) });

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "NhanVienId",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2025, 12, 8, 14, 45, 39, 711, DateTimeKind.Utc).AddTicks(5147), new DateTime(2025, 12, 8, 14, 45, 39, 711, DateTimeKind.Utc).AddTicks(5147) });

            migrationBuilder.UpdateData(
                table: "TaiKhoans",
                keyColumn: "TaiKhoanId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "KhachHangId", "NgayTaoTaiKhoan" },
                values: new object[] { null, new DateTime(2025, 12, 8, 14, 45, 39, 711, DateTimeKind.Utc).AddTicks(5041) });

            migrationBuilder.UpdateData(
                table: "TaiKhoans",
                keyColumn: "TaiKhoanId",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "NgayTaoTaiKhoan",
                value: new DateTime(2025, 12, 8, 14, 45, 39, 711, DateTimeKind.Utc).AddTicks(5068));
        }
    }
}
