using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoffeeShopAPI.Migrations
{
    public partial class InitializeDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alcohols",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alcohols", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Coffees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coffees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Desserts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Desserts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Milks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Milks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sandwiches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sandwiches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sauces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sauces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Snacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Snacks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Supplements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Syrups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Syrups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoffeeSizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CoffeeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeSizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoffeeSizes_Coffees_CoffeeId",
                        column: x => x.CoffeeId,
                        principalTable: "Coffees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DessertSizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    DessertId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DessertSizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DessertSizes_Desserts_DessertId",
                        column: x => x.DessertId,
                        principalTable: "Desserts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SandwichSizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    SandwichId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SandwichSizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SandwichSizes_Sandwiches_SandwichId",
                        column: x => x.SandwichId,
                        principalTable: "Sandwiches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SnackSizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    SnackId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnackSizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SnackSizes_Snacks_SnackId",
                        column: x => x.SnackId,
                        principalTable: "Snacks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeaSizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    TeaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeaSizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeaSizes_Teas_TeaId",
                        column: x => x.TeaId,
                        principalTable: "Teas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Cost = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoffeeOrders",
                columns: table => new
                {
                    CoffeeOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoffeeId = table.Column<int>(type: "int", nullable: false),
                    CoffeeSizeId = table.Column<int>(type: "int", nullable: false),
                    AlcoholId = table.Column<int>(type: "int", nullable: false),
                    SyrupId = table.Column<int>(type: "int", nullable: false),
                    MilkId = table.Column<int>(type: "int", nullable: false),
                    SupplementsId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeOrders", x => x.CoffeeOrderId);
                    table.ForeignKey(
                        name: "FK_CoffeeOrders_Alcohols_AlcoholId",
                        column: x => x.AlcoholId,
                        principalTable: "Alcohols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoffeeOrders_Coffees_CoffeeId",
                        column: x => x.CoffeeId,
                        principalTable: "Coffees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoffeeOrders_CoffeeSizes_CoffeeSizeId",
                        column: x => x.CoffeeSizeId,
                        principalTable: "CoffeeSizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoffeeOrders_Milks_MilkId",
                        column: x => x.MilkId,
                        principalTable: "Milks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoffeeOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoffeeOrders_Supplements_SupplementsId",
                        column: x => x.SupplementsId,
                        principalTable: "Supplements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoffeeOrders_Syrups_SyrupId",
                        column: x => x.SyrupId,
                        principalTable: "Syrups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DessertOrders",
                columns: table => new
                {
                    DessertOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DessertId = table.Column<int>(type: "int", nullable: false),
                    DessertSizeId = table.Column<int>(type: "int", nullable: false),
                    SyrupId = table.Column<int>(type: "int", nullable: false),
                    SupplementsId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DessertOrders", x => x.DessertOrderId);
                    table.ForeignKey(
                        name: "FK_DessertOrders_Desserts_DessertId",
                        column: x => x.DessertId,
                        principalTable: "Desserts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DessertOrders_DessertSizes_DessertSizeId",
                        column: x => x.DessertSizeId,
                        principalTable: "DessertSizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DessertOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DessertOrders_Supplements_SupplementsId",
                        column: x => x.SupplementsId,
                        principalTable: "Supplements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DessertOrders_Syrups_SyrupId",
                        column: x => x.SyrupId,
                        principalTable: "Syrups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SandwichOrders",
                columns: table => new
                {
                    SandwichOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SandwichId = table.Column<int>(type: "int", nullable: false),
                    SandwichSizeId = table.Column<int>(type: "int", nullable: false),
                    SauceId = table.Column<int>(type: "int", nullable: false),
                    SupplementsId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SandwichOrders", x => x.SandwichOrderId);
                    table.ForeignKey(
                        name: "FK_SandwichOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SandwichOrders_Sandwiches_SandwichId",
                        column: x => x.SandwichId,
                        principalTable: "Sandwiches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SandwichOrders_SandwichSizes_SandwichSizeId",
                        column: x => x.SandwichSizeId,
                        principalTable: "SandwichSizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SandwichOrders_Sauces_SauceId",
                        column: x => x.SauceId,
                        principalTable: "Sauces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SandwichOrders_Supplements_SupplementsId",
                        column: x => x.SupplementsId,
                        principalTable: "Supplements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SnackOrders",
                columns: table => new
                {
                    SnackOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SnackId = table.Column<int>(type: "int", nullable: false),
                    SnackSizeId = table.Column<int>(type: "int", nullable: false),
                    SauceId = table.Column<int>(type: "int", nullable: false),
                    SupplementsId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnackOrders", x => x.SnackOrderId);
                    table.ForeignKey(
                        name: "FK_SnackOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SnackOrders_Sauces_SauceId",
                        column: x => x.SauceId,
                        principalTable: "Sauces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SnackOrders_Snacks_SnackId",
                        column: x => x.SnackId,
                        principalTable: "Snacks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SnackOrders_SnackSizes_SnackSizeId",
                        column: x => x.SnackSizeId,
                        principalTable: "SnackSizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SnackOrders_Supplements_SupplementsId",
                        column: x => x.SupplementsId,
                        principalTable: "Supplements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeaOrders",
                columns: table => new
                {
                    TeaOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeaId = table.Column<int>(type: "int", nullable: false),
                    TeaSizeId = table.Column<int>(type: "int", nullable: false),
                    SyrupId = table.Column<int>(type: "int", nullable: false),
                    MilkId = table.Column<int>(type: "int", nullable: false),
                    SupplementsId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeaOrders", x => x.TeaOrderId);
                    table.ForeignKey(
                        name: "FK_TeaOrders_Milks_MilkId",
                        column: x => x.MilkId,
                        principalTable: "Milks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeaOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeaOrders_Supplements_SupplementsId",
                        column: x => x.SupplementsId,
                        principalTable: "Supplements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeaOrders_Syrups_SyrupId",
                        column: x => x.SyrupId,
                        principalTable: "Syrups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeaOrders_Teas_TeaId",
                        column: x => x.TeaId,
                        principalTable: "Teas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeaOrders_TeaSizes_TeaSizeId",
                        column: x => x.TeaSizeId,
                        principalTable: "TeaSizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeOrders_AlcoholId",
                table: "CoffeeOrders",
                column: "AlcoholId");

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeOrders_CoffeeId",
                table: "CoffeeOrders",
                column: "CoffeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeOrders_CoffeeSizeId",
                table: "CoffeeOrders",
                column: "CoffeeSizeId");

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeOrders_MilkId",
                table: "CoffeeOrders",
                column: "MilkId");

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeOrders_OrderId",
                table: "CoffeeOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeOrders_SupplementsId",
                table: "CoffeeOrders",
                column: "SupplementsId");

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeOrders_SyrupId",
                table: "CoffeeOrders",
                column: "SyrupId");

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeSizes_CoffeeId",
                table: "CoffeeSizes",
                column: "CoffeeId");

            migrationBuilder.CreateIndex(
                name: "IX_DessertOrders_DessertId",
                table: "DessertOrders",
                column: "DessertId");

            migrationBuilder.CreateIndex(
                name: "IX_DessertOrders_DessertSizeId",
                table: "DessertOrders",
                column: "DessertSizeId");

            migrationBuilder.CreateIndex(
                name: "IX_DessertOrders_OrderId",
                table: "DessertOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_DessertOrders_SupplementsId",
                table: "DessertOrders",
                column: "SupplementsId");

            migrationBuilder.CreateIndex(
                name: "IX_DessertOrders_SyrupId",
                table: "DessertOrders",
                column: "SyrupId");

            migrationBuilder.CreateIndex(
                name: "IX_DessertSizes_DessertId",
                table: "DessertSizes",
                column: "DessertId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SandwichOrders_OrderId",
                table: "SandwichOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SandwichOrders_SandwichId",
                table: "SandwichOrders",
                column: "SandwichId");

            migrationBuilder.CreateIndex(
                name: "IX_SandwichOrders_SandwichSizeId",
                table: "SandwichOrders",
                column: "SandwichSizeId");

            migrationBuilder.CreateIndex(
                name: "IX_SandwichOrders_SauceId",
                table: "SandwichOrders",
                column: "SauceId");

            migrationBuilder.CreateIndex(
                name: "IX_SandwichOrders_SupplementsId",
                table: "SandwichOrders",
                column: "SupplementsId");

            migrationBuilder.CreateIndex(
                name: "IX_SandwichSizes_SandwichId",
                table: "SandwichSizes",
                column: "SandwichId");

            migrationBuilder.CreateIndex(
                name: "IX_SnackOrders_OrderId",
                table: "SnackOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SnackOrders_SauceId",
                table: "SnackOrders",
                column: "SauceId");

            migrationBuilder.CreateIndex(
                name: "IX_SnackOrders_SnackId",
                table: "SnackOrders",
                column: "SnackId");

            migrationBuilder.CreateIndex(
                name: "IX_SnackOrders_SnackSizeId",
                table: "SnackOrders",
                column: "SnackSizeId");

            migrationBuilder.CreateIndex(
                name: "IX_SnackOrders_SupplementsId",
                table: "SnackOrders",
                column: "SupplementsId");

            migrationBuilder.CreateIndex(
                name: "IX_SnackSizes_SnackId",
                table: "SnackSizes",
                column: "SnackId");

            migrationBuilder.CreateIndex(
                name: "IX_TeaOrders_MilkId",
                table: "TeaOrders",
                column: "MilkId");

            migrationBuilder.CreateIndex(
                name: "IX_TeaOrders_OrderId",
                table: "TeaOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TeaOrders_SupplementsId",
                table: "TeaOrders",
                column: "SupplementsId");

            migrationBuilder.CreateIndex(
                name: "IX_TeaOrders_SyrupId",
                table: "TeaOrders",
                column: "SyrupId");

            migrationBuilder.CreateIndex(
                name: "IX_TeaOrders_TeaId",
                table: "TeaOrders",
                column: "TeaId");

            migrationBuilder.CreateIndex(
                name: "IX_TeaOrders_TeaSizeId",
                table: "TeaOrders",
                column: "TeaSizeId");

            migrationBuilder.CreateIndex(
                name: "IX_TeaSizes_TeaId",
                table: "TeaSizes",
                column: "TeaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoffeeOrders");

            migrationBuilder.DropTable(
                name: "DessertOrders");

            migrationBuilder.DropTable(
                name: "SandwichOrders");

            migrationBuilder.DropTable(
                name: "SnackOrders");

            migrationBuilder.DropTable(
                name: "TeaOrders");

            migrationBuilder.DropTable(
                name: "Alcohols");

            migrationBuilder.DropTable(
                name: "CoffeeSizes");

            migrationBuilder.DropTable(
                name: "DessertSizes");

            migrationBuilder.DropTable(
                name: "SandwichSizes");

            migrationBuilder.DropTable(
                name: "Sauces");

            migrationBuilder.DropTable(
                name: "SnackSizes");

            migrationBuilder.DropTable(
                name: "Milks");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Supplements");

            migrationBuilder.DropTable(
                name: "Syrups");

            migrationBuilder.DropTable(
                name: "TeaSizes");

            migrationBuilder.DropTable(
                name: "Coffees");

            migrationBuilder.DropTable(
                name: "Desserts");

            migrationBuilder.DropTable(
                name: "Sandwiches");

            migrationBuilder.DropTable(
                name: "Snacks");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Teas");
        }
    }
}
