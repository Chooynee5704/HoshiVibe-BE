using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoshiVibe.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomDesignToOrderDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomDesigns_OrderDetails_OrderDetailsOrderDetail_Id",
                table: "CustomDesigns");

            migrationBuilder.DropIndex(
                name: "IX_CustomDesigns_OrderDetailsOrderDetail_Id",
                table: "CustomDesigns");

            migrationBuilder.DropColumn(
                name: "OrderDetailsOrderDetail_Id",
                table: "CustomDesigns");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomDesign_Id",
                table: "OrderDetails",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_CustomDesign_Id",
                table: "OrderDetails",
                column: "CustomDesign_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_CustomDesigns_CustomDesign_Id",
                table: "OrderDetails",
                column: "CustomDesign_Id",
                principalTable: "CustomDesigns",
                principalColumn: "CustomDesign_Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_CustomDesigns_CustomDesign_Id",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_CustomDesign_Id",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "CustomDesign_Id",
                table: "OrderDetails");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderDetailsOrderDetail_Id",
                table: "CustomDesigns",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomDesigns_OrderDetailsOrderDetail_Id",
                table: "CustomDesigns",
                column: "OrderDetailsOrderDetail_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomDesigns_OrderDetails_OrderDetailsOrderDetail_Id",
                table: "CustomDesigns",
                column: "OrderDetailsOrderDetail_Id",
                principalTable: "OrderDetails",
                principalColumn: "OrderDetail_Id");
        }
    }
}
