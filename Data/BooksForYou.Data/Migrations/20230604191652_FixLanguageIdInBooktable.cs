using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooksForYou.Data.Migrations
{
    public partial class FixLanguageIdInBooktable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Languages_LanguageId",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "LanguageId",
                table: "Books",
                newName: "LanguagesId");

            migrationBuilder.RenameIndex(
                name: "IX_Books_LanguageId",
                table: "Books",
                newName: "IX_Books_LanguagesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Languages_LanguagesId",
                table: "Books",
                column: "LanguagesId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Languages_LanguagesId",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "LanguagesId",
                table: "Books",
                newName: "LanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_Books_LanguagesId",
                table: "Books",
                newName: "IX_Books_LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Languages_LanguageId",
                table: "Books",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
