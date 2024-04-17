using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagazineCMS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addStatusforFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Feedbacks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Contributions",
                keyColumn: "Id",
                keyValue: 1,
                column: "SubmissionDate",
                value: new DateTime(2024, 4, 14, 14, 33, 28, 372, DateTimeKind.Local).AddTicks(1349));

            migrationBuilder.UpdateData(
                table: "Magazines",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 4, 21, 14, 33, 28, 372, DateTimeKind.Local).AddTicks(1327), new DateTime(2024, 4, 7, 14, 33, 28, 372, DateTimeKind.Local).AddTicks(1309) });

            migrationBuilder.UpdateData(
                table: "Magazines",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 4, 21, 14, 33, 28, 372, DateTimeKind.Local).AddTicks(1330), new DateTime(2024, 4, 7, 14, 33, 28, 372, DateTimeKind.Local).AddTicks(1329) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Feedbacks");

            migrationBuilder.UpdateData(
                table: "Contributions",
                keyColumn: "Id",
                keyValue: 1,
                column: "SubmissionDate",
                value: new DateTime(2024, 3, 29, 23, 57, 40, 853, DateTimeKind.Local).AddTicks(7350));

            migrationBuilder.UpdateData(
                table: "Magazines",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 57, 40, 853, DateTimeKind.Local).AddTicks(7322), new DateTime(2024, 3, 22, 23, 57, 40, 853, DateTimeKind.Local).AddTicks(7298) });

            migrationBuilder.UpdateData(
                table: "Magazines",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 57, 40, 853, DateTimeKind.Local).AddTicks(7326), new DateTime(2024, 3, 22, 23, 57, 40, 853, DateTimeKind.Local).AddTicks(7326) });
        }
    }
}
