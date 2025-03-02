using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Template_Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "template_category",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_at_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_at_utc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_template_category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "template",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    is_default = table.Column<bool>(type: "bit", nullable: false),
                    template_category_id = table.Column<int>(type: "int", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_at_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_at_utc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_template", x => x.id);
                    table.ForeignKey(
                        name: "fk_template_template_category_template_category_id",
                        column: x => x.template_category_id,
                        principalTable: "template_category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_template",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    template_id = table.Column<int>(type: "int", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_at_utc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_template", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_template_template_template_id",
                        column: x => x.template_id,
                        principalTable: "template",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_template_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_template_template_category_id",
                table: "template",
                column: "template_category_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_template_template_id",
                table: "user_template",
                column: "template_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_template_user_id",
                table: "user_template",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_template");

            migrationBuilder.DropTable(
                name: "template");

            migrationBuilder.DropTable(
                name: "template_category");
        }
    }
}
