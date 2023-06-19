using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooksForYou.Data.Migrations
{
    public partial class RemoveGenresFromAuthor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Genres_Authors_AuthorId",
                table: "Genres");

            migrationBuilder.DropIndex(
                name: "IX_Genres_AuthorId",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "Born",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Books",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_ApplicationUserId",
                table: "Books",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_AspNetUsers_ApplicationUserId",
                table: "Books",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_AspNetUsers_ApplicationUserId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_ApplicationUserId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Books");

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Genres",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Born",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AspNetUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Genres_AuthorId",
                table: "Genres",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Genres_Authors_AuthorId",
                table: "Genres",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id");
        }
    }
}
