using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagazineCMS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class fixcomment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_ParentId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ParentId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Comments");

            migrationBuilder.UpdateData(
                table: "Contributions",
                keyColumn: "Id",
                keyValue: 1,
                column: "SubmissionDate",
                value: new DateTime(2024, 4, 19, 9, 9, 14, 825, DateTimeKind.Local).AddTicks(6780));

            migrationBuilder.UpdateData(
                table: "Magazines",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 4, 26, 9, 9, 14, 825, DateTimeKind.Local).AddTicks(6758), new DateTime(2024, 4, 12, 9, 9, 14, 825, DateTimeKind.Local).AddTicks(6744) });

            migrationBuilder.UpdateData(
                table: "Magazines",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 4, 26, 9, 9, 14, 825, DateTimeKind.Local).AddTicks(6761), new DateTime(2024, 4, 12, 9, 9, 14, 825, DateTimeKind.Local).AddTicks(6760) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Contributions",
                keyColumn: "Id",
                keyValue: 1,
                column: "SubmissionDate",
                value: new DateTime(2024, 4, 18, 16, 33, 59, 705, DateTimeKind.Local).AddTicks(294));

            migrationBuilder.UpdateData(
                table: "Magazines",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 4, 25, 16, 33, 59, 705, DateTimeKind.Local).AddTicks(270), new DateTime(2024, 4, 11, 16, 33, 59, 705, DateTimeKind.Local).AddTicks(260) });

            migrationBuilder.UpdateData(
                table: "Magazines",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 4, 25, 16, 33, 59, 705, DateTimeKind.Local).AddTicks(272), new DateTime(2024, 4, 11, 16, 33, 59, 705, DateTimeKind.Local).AddTicks(271) });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentId",
                table: "Comments",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_ParentId",
                table: "Comments",
                column: "ParentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
