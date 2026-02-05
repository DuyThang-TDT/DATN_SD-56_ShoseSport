using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurryFriends.API.Migrations
{
    /// <inheritdoc />
    public partial class AddLoaiToSanPham : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Loai",
                table: "SanPhams",
                type: "nvarchar(max)",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Loai",
                table: "SanPhams");

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

            migrationBuilder.UpdateData(
                table: "KhachHangs",
                keyColumn: "KhachHangId",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                columns: new[] { "NgayCapNhatCuoiCung", "NgayTaoTaiKhoan" },
                values: new object[] { new DateTime(2026, 2, 3, 13, 26, 52, 699, DateTimeKind.Utc).AddTicks(5059), new DateTime(2026, 2, 3, 13, 26, 52, 699, DateTimeKind.Utc).AddTicks(5057) });

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
                column: "NgayTaoTaiKhoan",
                value: new DateTime(2026, 2, 3, 13, 26, 52, 699, DateTimeKind.Utc).AddTicks(5424));

            migrationBuilder.UpdateData(
                table: "TaiKhoans",
                keyColumn: "TaiKhoanId",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "NgayTaoTaiKhoan",
                value: new DateTime(2026, 2, 3, 13, 26, 52, 699, DateTimeKind.Utc).AddTicks(5505));
        }
    }
}
