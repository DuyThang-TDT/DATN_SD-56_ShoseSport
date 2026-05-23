using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoseSport.API.Migrations
{
    /// <inheritdoc />
    public partial class initdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ChucVus",
                keyColumn: "ChucVuId",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2026, 2, 7, 16, 44, 55, 306, DateTimeKind.Utc).AddTicks(4931), new DateTime(2026, 2, 7, 16, 44, 55, 306, DateTimeKind.Utc).AddTicks(4931) });

            migrationBuilder.UpdateData(
                table: "ChucVus",
                keyColumn: "ChucVuId",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2026, 2, 7, 16, 44, 55, 306, DateTimeKind.Utc).AddTicks(4956), new DateTime(2026, 2, 7, 16, 44, 55, 306, DateTimeKind.Utc).AddTicks(4955) });

            migrationBuilder.UpdateData(
                table: "KhachHangs",
                keyColumn: "KhachHangId",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                columns: new[] { "NgayCapNhatCuoiCung", "NgayTaoTaiKhoan" },
                values: new object[] { new DateTime(2026, 2, 7, 16, 44, 55, 306, DateTimeKind.Utc).AddTicks(4596), new DateTime(2026, 2, 7, 16, 44, 55, 306, DateTimeKind.Utc).AddTicks(4593) });

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "NhanVienId",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2026, 2, 7, 16, 44, 55, 306, DateTimeKind.Utc).AddTicks(4984), new DateTime(2026, 2, 7, 16, 44, 55, 306, DateTimeKind.Utc).AddTicks(4983) });

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "NhanVienId",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "NgayCapNhat", "NgayTao" },
                values: new object[] { new DateTime(2026, 2, 7, 16, 44, 55, 306, DateTimeKind.Utc).AddTicks(5005), new DateTime(2026, 2, 7, 16, 44, 55, 306, DateTimeKind.Utc).AddTicks(5005) });

            migrationBuilder.UpdateData(
                table: "TaiKhoans",
                keyColumn: "TaiKhoanId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "NgayTaoTaiKhoan",
                value: new DateTime(2026, 2, 7, 16, 44, 55, 306, DateTimeKind.Utc).AddTicks(4868));

            migrationBuilder.UpdateData(
                table: "TaiKhoans",
                keyColumn: "TaiKhoanId",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "NgayTaoTaiKhoan",
                value: new DateTime(2026, 2, 7, 16, 44, 55, 306, DateTimeKind.Utc).AddTicks(4893));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
