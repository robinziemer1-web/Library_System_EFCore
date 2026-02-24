using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Library_systemEF.Migrations
{
    /// <inheritdoc />
    public partial class AddMemberAndLoan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "members",
                columns: table => new
                {
                    member_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_members", x => x.member_id);
                });

            migrationBuilder.CreateTable(
                name: "loans",
                columns: table => new
                {
                    loan_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    borrowed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    returned_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    member_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_loans", x => x.loan_id);
                    table.ForeignKey(
                        name: "fk_loans_members_member_id",
                        column: x => x.member_id,
                        principalTable: "members",
                        principalColumn: "member_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_loans_member_id",
                table: "loans",
                column: "member_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "loans");

            migrationBuilder.DropTable(
                name: "members");
        }
    }
}
