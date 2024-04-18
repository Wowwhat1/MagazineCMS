using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagazineCMS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddIsAnonymousFlagInCommentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAnonymous",
                table: "Comments",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAnonymous",
                table: "Comments");

            migrationBuilder.UpdateData(
                table: "Contributions",
                keyColumn: "Id",
                keyValue: 1,
                column: "SubmissionDate",
                value: new DateTime(2024, 4, 18, 15, 35, 7, 175, DateTimeKind.Local).AddTicks(5271));

            migrationBuilder.UpdateData(
                table: "Magazines",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 4, 25, 15, 35, 7, 175, DateTimeKind.Local).AddTicks(5246), new DateTime(2024, 4, 11, 15, 35, 7, 175, DateTimeKind.Local).AddTicks(5232) });

            migrationBuilder.UpdateData(
                table: "Magazines",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 4, 25, 15, 35, 7, 175, DateTimeKind.Local).AddTicks(5248), new DateTime(2024, 4, 11, 15, 35, 7, 175, DateTimeKind.Local).AddTicks(5248) });
        }
    }
}
