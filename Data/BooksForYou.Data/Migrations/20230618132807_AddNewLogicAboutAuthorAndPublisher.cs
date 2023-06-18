using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooksForYou.Data.Migrations
{
    public partial class AddNewLogicAboutAuthorAndPublisher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Authors");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Publishers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Authors",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Publishers_UserId",
                table: "Publishers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_UserId",
                table: "Authors",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_AspNetUsers_UserId",
                table: "Authors",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Publishers_AspNetUsers_UserId",
                table: "Publishers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_AspNetUsers_UserId",
                table: "Authors");

            migrationBuilder.DropForeignKey(
                name: "FK_Publishers_AspNetUsers_UserId",
                table: "Publishers");

            migrationBuilder.DropIndex(
                name: "IX_Publishers_UserId",
                table: "Publishers");

            migrationBuilder.DropIndex(
                name: "IX_Authors_UserId",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Authors");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Authors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
