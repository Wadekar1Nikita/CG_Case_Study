using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LatestUpdate.Migrations
{
    /// <inheritdoc />
    public partial class FixForeignKeyConstraintss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    AdminID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    APassword = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.AdminID);
                });

            migrationBuilder.CreateTable(
                name: "Bidders",
                columns: table => new
                {
                    BidderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adharno = table.Column<long>(type: "bigint", nullable: false),
                    PAN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankAccountNo = table.Column<long>(type: "bigint", nullable: false),
                    IFSCCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TraderLicence = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bidders", x => x.BidderID);
                });

            migrationBuilder.CreateTable(
                name: "Farmers",
                columns: table => new
                {
                    FarmerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Adharno = table.Column<long>(type: "bigint", nullable: false),
                    PAN = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    BankAccountNo = table.Column<long>(type: "bigint", nullable: false),
                    Certificate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IFSCCode = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Area = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LandAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farmers", x => x.FarmerID);
                });

            migrationBuilder.CreateTable(
                name: "Crops",
                columns: table => new
                {
                    CropID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CropName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CropType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    FertilizerType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FarmerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Crops", x => x.CropID);
                    table.ForeignKey(
                        name: "FK_Crops_Farmers_FarmerID",
                        column: x => x.FarmerID,
                        principalTable: "Farmers",
                        principalColumn: "FarmerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Insurances",
                columns: table => new
                {
                    InsuranceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FarmerID = table.Column<int>(type: "int", nullable: false),
                    PremiumAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PolicyName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SumInsured = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Area = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Season = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    PremiumRateForSeason = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Insurances", x => x.InsuranceID);
                    table.ForeignKey(
                        name: "FK_Insurances_Farmers_FarmerID",
                        column: x => x.FarmerID,
                        principalTable: "Farmers",
                        principalColumn: "FarmerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Biddings",
                columns: table => new
                {
                    BidID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AuctionResult = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CropID = table.Column<int>(type: "int", nullable: false),
                    BidderID = table.Column<int>(type: "int", nullable: false),
                    BidderID1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Biddings", x => x.BidID);
                    table.ForeignKey(
                        name: "FK_Biddings_Bidders_BidderID",
                        column: x => x.BidderID,
                        principalTable: "Bidders",
                        principalColumn: "BidderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Biddings_Bidders_BidderID1",
                        column: x => x.BidderID1,
                        principalTable: "Bidders",
                        principalColumn: "BidderID");
                    table.ForeignKey(
                        name: "FK_Biddings_Crops_CropID",
                        column: x => x.CropID,
                        principalTable: "Crops",
                        principalColumn: "CropID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClaimInsurances",
                columns: table => new
                {
                    ClaimID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsuranceID = table.Column<int>(type: "int", nullable: false),
                    ClaimAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClaimReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DateOfLoss = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimInsurances", x => x.ClaimID);
                    table.ForeignKey(
                        name: "FK_ClaimInsurances_Insurances_InsuranceID",
                        column: x => x.InsuranceID,
                        principalTable: "Insurances",
                        principalColumn: "InsuranceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SoldHistories",
                columns: table => new
                {
                    SoldID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CropID = table.Column<int>(type: "int", nullable: false),
                    BidID = table.Column<int>(type: "int", nullable: false),
                    MSP = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SoldPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoldHistories", x => x.SoldID);
                    table.ForeignKey(
                        name: "FK_SoldHistories_Biddings_BidID",
                        column: x => x.BidID,
                        principalTable: "Biddings",
                        principalColumn: "BidID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SoldHistories_Crops_CropID",
                        column: x => x.CropID,
                        principalTable: "Crops",
                        principalColumn: "CropID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admins_AEmail",
                table: "Admins",
                column: "AEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bidders_Email",
                table: "Bidders",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Biddings_BidderID",
                table: "Biddings",
                column: "BidderID");

            migrationBuilder.CreateIndex(
                name: "IX_Biddings_BidderID1",
                table: "Biddings",
                column: "BidderID1");

            migrationBuilder.CreateIndex(
                name: "IX_Biddings_CropID",
                table: "Biddings",
                column: "CropID");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimInsurances_InsuranceID",
                table: "ClaimInsurances",
                column: "InsuranceID");

            migrationBuilder.CreateIndex(
                name: "IX_Crops_FarmerID",
                table: "Crops",
                column: "FarmerID");

            migrationBuilder.CreateIndex(
                name: "IX_Insurances_FarmerID",
                table: "Insurances",
                column: "FarmerID");

            migrationBuilder.CreateIndex(
                name: "IX_SoldHistories_BidID",
                table: "SoldHistories",
                column: "BiddingBidID");

            migrationBuilder.CreateIndex(
                name: "IX_SoldHistories_CropID",
                table: "SoldHistories",
                column: "CropID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "ClaimInsurances");

            migrationBuilder.DropTable(
                name: "SoldHistories");

            migrationBuilder.DropTable(
                name: "Insurances");

            migrationBuilder.DropTable(
                name: "Biddings");

            migrationBuilder.DropTable(
                name: "Bidders");

            migrationBuilder.DropTable(
                name: "Crops");

            migrationBuilder.DropTable(
                name: "Farmers");
        }
    }
}
