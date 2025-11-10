using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoshiVibe.Migrations
{
    /// <inheritdoc />
    public partial class addatribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "User_Id",
                table: "CustomProduct",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "User_Id1",
                table: "CustomProduct",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomProduct_User_Id1",
                table: "CustomProduct",
                column: "User_Id1");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomProduct_Users_User_Id1",
                table: "CustomProduct",
                column: "User_Id1",
                principalTable: "Users",
                principalColumn: "User_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomProduct_Users_User_Id1",
                table: "CustomProduct");

            migrationBuilder.DropIndex(
                name: "IX_CustomProduct_User_Id1",
                table: "CustomProduct");

            migrationBuilder.DropColumn(
                name: "User_Id",
                table: "CustomProduct");

            migrationBuilder.DropColumn(
                name: "User_Id1",
                table: "CustomProduct");
        }
    }
}
