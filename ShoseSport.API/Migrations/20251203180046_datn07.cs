using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurryFriends.API.Migrations
{
    /// <inheritdoc />
    public partial class datn07 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ChucVus",
                keyColumn: "ChucVuId",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2025, 12, 3, 18, 0, 44, 953, DateTimeKind.Utc).AddTicks(1844), new DateTime(2025, 12, 3, 18, 0, 44, 953, DateTimeKind.Utc).AddTicks(1844) });

            migrationBuilder.UpdateData(
                table: "ChucVus",
                keyColumn: "ChucVuId",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2025, 12, 3, 18, 0, 44, 953, DateTimeKind.Utc).AddTicks(1865), new DateTime(2025, 12, 3, 18, 0, 44, 953, DateTimeKind.Utc).AddTicks(1864) });

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "NhanVienId",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2025, 12, 3, 18, 0, 44, 953, DateTimeKind.Utc).AddTicks(1898), new DateTime(2025, 12, 3, 18, 0, 44, 953, DateTimeKind.Utc).AddTicks(1897) });

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "NhanVienId",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2025, 12, 3, 18, 0, 44, 953, DateTimeKind.Utc).AddTicks(1916), new DateTime(2025, 12, 3, 18, 0, 44, 953, DateTimeKind.Utc).AddTicks(1916) });

            migrationBuilder.UpdateData(
                table: "TaiKhoans",
                keyColumn: "TaiKhoanId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "NgayTaoTaiKhoan",
                value: new DateTime(2025, 12, 3, 18, 0, 44, 953, DateTimeKind.Utc).AddTicks(1781));

            migrationBuilder.UpdateData(
                table: "TaiKhoans",
                keyColumn: "TaiKhoanId",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "NgayTaoTaiKhoan",
                value: new DateTime(2025, 12, 3, 18, 0, 44, 953, DateTimeKind.Utc).AddTicks(1817));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ChucVus",
                keyColumn: "ChucVuId",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2025, 11, 13, 4, 52, 53, 24, DateTimeKind.Utc).AddTicks(9101), new DateTime(2025, 11, 13, 4, 52, 53, 24, DateTimeKind.Utc).AddTicks(9096) });

            migrationBuilder.UpdateData(
                table: "ChucVus",
                keyColumn: "ChucVuId",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2025, 11, 13, 4, 52, 53, 24, DateTimeKind.Utc).AddTicks(9116), new DateTime(2025, 11, 13, 4, 52, 53, 24, DateTimeKind.Utc).AddTicks(9115) });

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "NhanVienId",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2025, 11, 13, 4, 52, 53, 24, DateTimeKind.Utc).AddTicks(9143), new DateTime(2025, 11, 13, 4, 52, 53, 24, DateTimeKind.Utc).AddTicks(9143) });

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "NhanVienId",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2025, 11, 13, 4, 52, 53, 24, DateTimeKind.Utc).AddTicks(9160), new DateTime(2025, 11, 13, 4, 52, 53, 24, DateTimeKind.Utc).AddTicks(9159) });

            migrationBuilder.UpdateData(
                table: "TaiKhoans",
                keyColumn: "TaiKhoanId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "NgayTaoTaiKhoan",
                value: new DateTime(2025, 11, 13, 4, 52, 53, 24, DateTimeKind.Utc).AddTicks(9034));

            migrationBuilder.UpdateData(
                table: "TaiKhoans",
                keyColumn: "TaiKhoanId",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "NgayTaoTaiKhoan",
                value: new DateTime(2025, 11, 13, 4, 52, 53, 24, DateTimeKind.Utc).AddTicks(9071));
        }
    }
}
