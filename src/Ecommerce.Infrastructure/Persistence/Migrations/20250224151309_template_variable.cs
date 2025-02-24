using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class template_variable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "variable_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_at_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_at_utc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_variable_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "variable",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    variable_type_id = table.Column<int>(type: "int", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_at_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_at_utc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_variable", x => x.id);
                    table.ForeignKey(
                        name: "fk_variable_variable_type_variable_type_id",
                        column: x => x.variable_type_id,
                        principalTable: "variable_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "template_variable",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    template_id = table.Column<int>(type: "int", nullable: false),
                    variable_id = table.Column<int>(type: "int", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_at_utc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_template_variable", x => x.id);
                    table.ForeignKey(
                        name: "fk_template_variable_template_template_id",
                        column: x => x.template_id,
                        principalTable: "template",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_template_variable_variable_variable_id",
                        column: x => x.variable_id,
                        principalTable: "variable",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_template_variable_template_id",
                table: "template_variable",
                column: "template_id");

            migrationBuilder.CreateIndex(
                name: "ix_template_variable_variable_id",
                table: "template_variable",
                column: "variable_id");

            migrationBuilder.CreateIndex(
                name: "ix_variable_variable_type_id",
                table: "variable",
                column: "variable_type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "template_variable");

            migrationBuilder.DropTable(
                name: "variable");

            migrationBuilder.DropTable(
                name: "variable_type");
        }
    }
}
