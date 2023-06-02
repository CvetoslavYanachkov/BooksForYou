using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooksForYou.Data.Migrations
{
    public partial class AddGenre : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GenreId",
                table: "Authors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Authors_GenreId",
                table: "Authors",
                column: "GenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_Genres_GenreId",
                table: "Authors",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Genres_GenreId",
                table: "Authors");

            migrationBuilder.DropIndex(
                name: "IX_Authors_GenreId",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "GenreId",
                table: "Authors");
        }
    }
}
