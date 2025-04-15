using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UX_Shopping_Cart_Product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_shopping_cart_product_shopping_cart_id",
                table: "shopping_cart_product");

            migrationBuilder.CreateIndex(
                name: "UX_ShoppingCartProduct_ShoppingCartId_ProductId",
                table: "shopping_cart_product",
                columns: new[] { "shopping_cart_id", "product_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_ShoppingCartProduct_ShoppingCartId_ProductId",
                table: "shopping_cart_product");

            migrationBuilder.CreateIndex(
                name: "ix_shopping_cart_product_shopping_cart_id",
                table: "shopping_cart_product",
                column: "shopping_cart_id");
        }
    }
}
