using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoshiVibe.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Product_Id",
                table: "Orders");

            migrationBuilder.AddColumn<Guid>(
                name: "CProduct_Id",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Cart_Id",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Cart_Id1",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CustomProductCProduct_Id",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CustomProductCProduct_Id",
                table: "OrderDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomProduct",
                columns: table => new
                {
                    CProduct_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomProduct", x => x.CProduct_Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Cart_Id1",
                table: "Orders",
                column: "Cart_Id1");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomProductCProduct_Id",
                table: "Orders",
                column: "CustomProductCProduct_Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_CustomProductCProduct_Id",
                table: "OrderDetails",
                column: "CustomProductCProduct_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_CustomProduct_CustomProductCProduct_Id",
                table: "OrderDetails",
                column: "CustomProductCProduct_Id",
                principalTable: "CustomProduct",
                principalColumn: "CProduct_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CustomProduct_CustomProductCProduct_Id",
                table: "Orders",
                column: "CustomProductCProduct_Id",
                principalTable: "CustomProduct",
                principalColumn: "CProduct_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ShoppingCarts_Cart_Id1",
                table: "Orders",
                column: "Cart_Id1",
                principalTable: "ShoppingCarts",
                principalColumn: "Cart_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_CustomProduct_CustomProductCProduct_Id",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CustomProduct_CustomProductCProduct_Id",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ShoppingCarts_Cart_Id1",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "CustomProduct");

            migrationBuilder.DropIndex(
                name: "IX_Orders_Cart_Id1",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CustomProductCProduct_Id",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_CustomProductCProduct_Id",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "CProduct_Id",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Cart_Id",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Cart_Id1",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CustomProductCProduct_Id",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CustomProductCProduct_Id",
                table: "OrderDetails");

            migrationBuilder.AddColumn<Guid>(
                name: "Product_Id",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
