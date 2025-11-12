using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoshiVibe.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomDesignEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomDesigns",
                columns: table => new
                {
                    CustomDesign_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    User_Id = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    RawImageBase64 = table.Column<string>(type: "text", nullable: true),
                    AiImageUrl = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OrderDetailsOrderDetail_Id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomDesigns", x => x.CustomDesign_Id);
                    table.ForeignKey(
                        name: "FK_CustomDesigns_OrderDetails_OrderDetailsOrderDetail_Id",
                        column: x => x.OrderDetailsOrderDetail_Id,
                        principalTable: "OrderDetails",
                        principalColumn: "OrderDetail_Id");
                    table.ForeignKey(
                        name: "FK_CustomDesigns_Users_User_Id",
                        column: x => x.User_Id,
                        principalTable: "Users",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "CustomDesignCharms",
                columns: table => new
                {
                    CustomDesignCharm_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomDesign_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CProduct_Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomDesignCharms", x => x.CustomDesignCharm_Id);
                    table.ForeignKey(
                        name: "FK_CustomDesignCharms_CustomDesigns_CustomDesign_Id",
                        column: x => x.CustomDesign_Id,
                        principalTable: "CustomDesigns",
                        principalColumn: "CustomDesign_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomDesignCharms_CustomProduct_CProduct_Id",
                        column: x => x.CProduct_Id,
                        principalTable: "CustomProduct",
                        principalColumn: "CProduct_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomDesignCharms_CProduct_Id",
                table: "CustomDesignCharms",
                column: "CProduct_Id");

            migrationBuilder.CreateIndex(
                name: "IX_CustomDesignCharms_CustomDesign_Id",
                table: "CustomDesignCharms",
                column: "CustomDesign_Id");

            migrationBuilder.CreateIndex(
                name: "IX_CustomDesigns_OrderDetailsOrderDetail_Id",
                table: "CustomDesigns",
                column: "OrderDetailsOrderDetail_Id");

            migrationBuilder.CreateIndex(
                name: "IX_CustomDesigns_User_Id",
                table: "CustomDesigns",
                column: "User_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomDesignCharms");

            migrationBuilder.DropTable(
                name: "CustomDesigns");
        }
    }
}
