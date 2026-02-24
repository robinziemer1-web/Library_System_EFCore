using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library_systemEF.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationLOanAndBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "book_id",
                table: "loans",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_loans_book_id",
                table: "loans",
                column: "book_id");

            migrationBuilder.AddForeignKey(
                name: "fk_loans_books_book_id",
                table: "loans",
                column: "book_id",
                principalTable: "books",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_loans_books_book_id",
                table: "loans");

            migrationBuilder.DropIndex(
                name: "ix_loans_book_id",
                table: "loans");

            migrationBuilder.DropColumn(
                name: "book_id",
                table: "loans");
        }
    }
}
