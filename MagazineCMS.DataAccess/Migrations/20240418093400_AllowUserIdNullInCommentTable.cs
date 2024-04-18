using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagazineCMS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AllowUserIdNullInCommentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
