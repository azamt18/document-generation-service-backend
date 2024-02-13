using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "input_html_files",
                columns: table => new
                {
                    id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    createdon = table.Column<DateTime>(name: "created_on", type: "TEXT", nullable: false),
                    updatedon = table.Column<DateTime>(name: "updated_on", type: "TEXT", nullable: false),
                    isdeleted = table.Column<bool>(name: "is_deleted", type: "INTEGER", nullable: false),
                    deletedon = table.Column<DateTime>(name: "deleted_on", type: "TEXT", nullable: true),
                    title = table.Column<string>(type: "text", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", maxLength: 100, nullable: false),
                    status = table.Column<int>(type: "INTEGER", nullable: false),
                    guid = table.Column<string>(type: "text", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_input_html_files", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "output_pdf_files",
                columns: table => new
                {
                    id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    createdon = table.Column<DateTime>(name: "created_on", type: "TEXT", nullable: false),
                    updatedon = table.Column<DateTime>(name: "updated_on", type: "TEXT", nullable: false),
                    isdeleted = table.Column<bool>(name: "is_deleted", type: "INTEGER", nullable: false),
                    deletedon = table.Column<DateTime>(name: "deleted_on", type: "TEXT", nullable: true),
                    inputhtmlfileid = table.Column<long>(name: "input_html_file_id", type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_output_pdf_files", x => x.id);
                    table.ForeignKey(
                        name: "FK_output_pdf_files_input_html_files_input_html_file_id",
                        column: x => x.inputhtmlfileid,
                        principalTable: "input_html_files",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_output_pdf_files_input_html_file_id",
                table: "output_pdf_files",
                column: "input_html_file_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "output_pdf_files");

            migrationBuilder.DropTable(
                name: "input_html_files");
        }
    }
}
