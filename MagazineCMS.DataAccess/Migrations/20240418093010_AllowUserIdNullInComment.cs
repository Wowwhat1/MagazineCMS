using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagazineCMS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AllowUserIdNullInComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Contributions",
                keyColumn: "Id",
                keyValue: 1,
                column: "SubmissionDate",
                value: new DateTime(2024, 4, 18, 16, 30, 10, 28, DateTimeKind.Local).AddTicks(6989));

            migrationBuilder.UpdateData(
                table: "Magazines",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 4, 25, 16, 30, 10, 28, DateTimeKind.Local).AddTicks(6962), new DateTime(2024, 4, 11, 16, 30, 10, 28, DateTimeKind.Local).AddTicks(6952) });

            migrationBuilder.UpdateData(
                table: "Magazines",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 4, 25, 16, 30, 10, 28, DateTimeKind.Local).AddTicks(6965), new DateTime(2024, 4, 11, 16, 30, 10, 28, DateTimeKind.Local).AddTicks(6964) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Contributions",
                keyColumn: "Id",
                keyValue: 1,
                column: "SubmissionDate",
                value: new DateTime(2024, 4, 18, 16, 21, 20, 293, DateTimeKind.Local).AddTicks(4558));

            migrationBuilder.UpdateData(
                table: "Magazines",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 4, 25, 16, 21, 20, 293, DateTimeKind.Local).AddTicks(4533), new DateTime(2024, 4, 11, 16, 21, 20, 293, DateTimeKind.Local).AddTicks(4520) });

            migrationBuilder.UpdateData(
                table: "Magazines",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 4, 25, 16, 21, 20, 293, DateTimeKind.Local).AddTicks(4536), new DateTime(2024, 4, 11, 16, 21, 20, 293, DateTimeKind.Local).AddTicks(4535) });
        }
    }
}
