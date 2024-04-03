using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagazineCMS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationModel : Migration
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

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipientUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserIds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_RecipientUserId",
                        column: x => x.RecipientUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_RecipientUserId",
                table: "Notifications",
                column: "RecipientUserId");

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

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.UpdateData(
                table: "Contributions",
                keyColumn: "Id",
                keyValue: 1,
                column: "SubmissionDate",
                value: new DateTime(2024, 3, 18, 23, 13, 32, 243, DateTimeKind.Local).AddTicks(6091));

            migrationBuilder.UpdateData(
                table: "Magazines",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 3, 25, 23, 13, 32, 243, DateTimeKind.Local).AddTicks(6054), new DateTime(2024, 3, 11, 23, 13, 32, 243, DateTimeKind.Local).AddTicks(6031) });

            migrationBuilder.UpdateData(
                table: "Magazines",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 3, 25, 23, 13, 32, 243, DateTimeKind.Local).AddTicks(6060), new DateTime(2024, 3, 11, 23, 13, 32, 243, DateTimeKind.Local).AddTicks(6059) });

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
