using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoshiVibe.Migrations
{
    /// <inheritdoc />
    public partial class adjustlogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Destinies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destinies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Product_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Destiny = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Product_Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    User_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Account = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false),
                    resetToken = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.User_Id);
                });

            migrationBuilder.CreateTable(
                name: "Vouchers",
                columns: table => new
                {
                    Voucher_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vouchers", x => x.Voucher_Id);
                });

            migrationBuilder.CreateTable(
                name: "Zodiacs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateRange = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zodiacs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Cart_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    User_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Cart_Id);
                    table.ForeignKey(
                        name: "FK_Carts_Users_User_Id",
                        column: x => x.User_Id,
                        principalTable: "Users",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    UserProfile_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    User_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Point = table.Column<int>(type: "int", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Yob = table.Column<DateTime>(type: "datetime2", nullable: false),
                    YobDestination = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZodiacId = table.Column<int>(type: "int", nullable: true),
                    DestinyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.UserProfile_Id);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Destinies_DestinyId",
                        column: x => x.DestinyId,
                        principalTable: "Destinies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Users_User_Id",
                        column: x => x.User_Id,
                        principalTable: "Users",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Zodiacs_ZodiacId",
                        column: x => x.ZodiacId,
                        principalTable: "Zodiacs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    CartItem_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cart_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Product_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.CartItem_Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_Cart_Id",
                        column: x => x.Cart_Id,
                        principalTable: "Carts",
                        principalColumn: "Cart_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_Product_Id",
                        column: x => x.Product_Id,
                        principalTable: "Products",
                        principalColumn: "Product_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomProduct",
                columns: table => new
                {
                    CProduct_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderDetailsOrderDetail_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomProduct", x => x.CProduct_Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Order_Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Cart_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    User_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Voucher_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FinalPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ShippingAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cart_Id1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomProductCProduct_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Order_Id);
                    table.ForeignKey(
                        name: "FK_Orders_Carts_Cart_Id1",
                        column: x => x.Cart_Id1,
                        principalTable: "Carts",
                        principalColumn: "Cart_Id");
                    table.ForeignKey(
                        name: "FK_Orders_CustomProduct_CustomProductCProduct_Id",
                        column: x => x.CustomProductCProduct_Id,
                        principalTable: "CustomProduct",
                        principalColumn: "CProduct_Id");
                    table.ForeignKey(
                        name: "FK_Orders_Users_User_Id",
                        column: x => x.User_Id,
                        principalTable: "Users",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Vouchers_Voucher_Id",
                        column: x => x.Voucher_Id,
                        principalTable: "Vouchers",
                        principalColumn: "Voucher_Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    OrderDetail_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CProduct_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Product_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.OrderDetail_Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Order_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Product_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_Product_Id",
                        column: x => x.Product_Id,
                        principalTable: "Products",
                        principalColumn: "Product_Id");
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Payment_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order_Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Payment_Id);
                    table.ForeignKey(
                        name: "FK_Payments_Orders_Order_Id",
                        column: x => x.Order_Id,
                        principalTable: "Orders",
                        principalColumn: "Order_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTransactions",
                columns: table => new
                {
                    Transaction_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Payment_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GatewayTransactionId = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTransactions", x => x.Transaction_Id);
                    table.ForeignKey(
                        name: "FK_PaymentTransactions_Payments_Payment_Id",
                        column: x => x.Payment_Id,
                        principalTable: "Payments",
                        principalColumn: "Payment_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_Cart_Id",
                table: "CartItems",
                column: "Cart_Id");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_Product_Id",
                table: "CartItems",
                column: "Product_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_User_Id",
                table: "Carts",
                column: "User_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomProduct_OrderDetailsOrderDetail_Id",
                table: "CustomProduct",
                column: "OrderDetailsOrderDetail_Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_Product_Id",
                table: "OrderDetails",
                column: "Product_Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Cart_Id1",
                table: "Orders",
                column: "Cart_Id1");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomProductCProduct_Id",
                table: "Orders",
                column: "CustomProductCProduct_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_User_Id",
                table: "Orders",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Voucher_Id",
                table: "Orders",
                column: "Voucher_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Order_Id",
                table: "Payments",
                column: "Order_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_Payment_Id",
                table: "PaymentTransactions",
                column: "Payment_Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_DestinyId",
                table: "UserProfiles",
                column: "DestinyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_User_Id",
                table: "UserProfiles",
                column: "User_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_ZodiacId",
                table: "UserProfiles",
                column: "ZodiacId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomProduct_OrderDetails_OrderDetailsOrderDetail_Id",
                table: "CustomProduct",
                column: "OrderDetailsOrderDetail_Id",
                principalTable: "OrderDetails",
                principalColumn: "OrderDetail_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Carts_Cart_Id1",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Products_ProductId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Products_Product_Id",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_User_Id",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomProduct_OrderDetails_OrderDetailsOrderDetail_Id",
                table: "CustomProduct");

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "PaymentTransactions");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Destinies");

            migrationBuilder.DropTable(
                name: "Zodiacs");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "CustomProduct");

            migrationBuilder.DropTable(
                name: "Vouchers");
        }
    }
}
