using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagazineCMS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedDataMagazineContributionDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contributions_AspNetUsers_UserId",
                table: "Contributions");

            migrationBuilder.DropForeignKey(
                name: "FK_Contributions_Magazines_MagazineId",
                table: "Contributions");

            migrationBuilder.InsertData(
                table: "Magazines",
                columns: new[] { "Id", "Description", "EndDate", "FacultyId", "Name", "SemesterId", "StartDate" },
                values: new object[,]
                {
                    { 1, "Welcome to the Spring 2024 issue of Cutting-Edge Tech, your ultimate guide to the latest innovations and developments in the world of computing. In this edition, we delve into the forefront of technology, exploring groundbreaking advancements that are shaping the future of computing.", new DateTime(2024, 3, 25, 23, 13, 32, 243, DateTimeKind.Local).AddTicks(6054), 2, "Computing Magazine - Spring 2024", 1, new DateTime(2024, 3, 11, 23, 13, 32, 243, DateTimeKind.Local).AddTicks(6031) },
                    { 2, "Welcome", new DateTime(2024, 3, 25, 23, 13, 32, 243, DateTimeKind.Local).AddTicks(6060), 3, "Business Magazine - Spring 2024", 1, new DateTime(2024, 3, 11, 23, 13, 32, 243, DateTimeKind.Local).AddTicks(6059) }
                });

            migrationBuilder.InsertData(
                table: "Contributions",
                columns: new[] { "Id", "MagazineId", "Status", "SubmissionDate", "Title", "UserId" },
                values: new object[] { 1, 1, "Pending", new DateTime(2024, 3, 18, 23, 13, 32, 243, DateTimeKind.Local).AddTicks(6091), "The Future of AI", "StudentID1" });

            migrationBuilder.InsertData(
                table: "Documents",
                columns: new[] { "Id", "ContributionId", "DocumentUrl", "Type" },
                values: new object[] { 1, 1, "~/contributions/StudentID1/File.docx", "Word" });

            migrationBuilder.AddForeignKey(
                name: "FK_Contributions_AspNetUsers_UserId",
                table: "Contributions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contributions_Magazines_MagazineId",
                table: "Contributions",
                column: "MagazineId",
                principalTable: "Magazines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contributions_AspNetUsers_UserId",
                table: "Contributions");

            migrationBuilder.DropForeignKey(
                name: "FK_Contributions_Magazines_MagazineId",
                table: "Contributions");

            migrationBuilder.DeleteData(
                table: "Documents",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Magazines",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Contributions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Magazines",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AddForeignKey(
                name: "FK_Contributions_AspNetUsers_UserId",
                table: "Contributions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contributions_Magazines_MagazineId",
                table: "Contributions",
                column: "MagazineId",
                principalTable: "Magazines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
