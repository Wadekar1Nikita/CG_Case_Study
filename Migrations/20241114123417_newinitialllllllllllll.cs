using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LatestUpdate.Migrations
{
    /// <inheritdoc />
    public partial class newinitialllllllllllll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoldHistories_Biddings_BidID",
                table: "SoldHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_SoldHistories_Crops_CropID",
                table: "SoldHistories");

            migrationBuilder.DropIndex(
                name: "IX_SoldHistories_BidID",
                table: "SoldHistories");

            migrationBuilder.AddColumn<int>(
                name: "BiddingBidID",
                table: "SoldHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SoldHistories_BiddingBidID",
                table: "SoldHistories",
                column: "BiddingBidID");

            migrationBuilder.AddForeignKey(
                name: "FK_SoldHistories_Biddings_BiddingBidID",
                table: "SoldHistories",
                column: "BiddingBidID",
                principalTable: "Biddings",
                principalColumn: "BidID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SoldHistories_Crops_CropID",
                table: "SoldHistories",
                column: "CropID",
                principalTable: "Crops",
                principalColumn: "CropID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoldHistories_Biddings_BiddingBidID",
                table: "SoldHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_SoldHistories_Crops_CropID",
                table: "SoldHistories");

            migrationBuilder.DropIndex(
                name: "IX_SoldHistories_BiddingBidID",
                table: "SoldHistories");

            migrationBuilder.DropColumn(
                name: "BiddingBidID",
                table: "SoldHistories");

            migrationBuilder.CreateIndex(
                name: "IX_SoldHistories_BidID",
                table: "SoldHistories",
                column: "BidID");

            migrationBuilder.AddForeignKey(
                name: "FK_SoldHistories_Biddings_BidID",
                table: "SoldHistories",
                column: "BidID",
                principalTable: "Biddings",
                principalColumn: "BidID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SoldHistories_Crops_CropID",
                table: "SoldHistories",
                column: "CropID",
                principalTable: "Crops",
                principalColumn: "CropID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
