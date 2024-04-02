using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagazineCMS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddSenderNameInNoti : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SenderUserName",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SenderUserName",
                table: "Notifications");

            migrationBuilder.UpdateData(
                table: "Contributions",
                keyColumn: "Id",
                keyValue: 1,
                column: "SubmissionDate",
                value: new DateTime(2024, 3, 28, 21, 43, 14, 934, DateTimeKind.Local).AddTicks(4931));

            migrationBuilder.UpdateData(
                table: "Magazines",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 4, 4, 21, 43, 14, 934, DateTimeKind.Local).AddTicks(4902), new DateTime(2024, 3, 21, 21, 43, 14, 934, DateTimeKind.Local).AddTicks(4885) });

            migrationBuilder.UpdateData(
                table: "Magazines",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 4, 4, 21, 43, 14, 934, DateTimeKind.Local).AddTicks(4907), new DateTime(2024, 3, 21, 21, 43, 14, 934, DateTimeKind.Local).AddTicks(4906) });
        }
    }
}
