using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LatestUpdate.Migrations
{
    /// <inheritdoc />
    public partial class fixingsss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Biddings_Bidders_BidderID",
                table: "Biddings");

            migrationBuilder.DropForeignKey(
                name: "FK_Biddings_Bidders_BidderID1",
                table: "Biddings");

            migrationBuilder.DropIndex(
                name: "IX_Biddings_BidderID1",
                table: "Biddings");

            migrationBuilder.DropColumn(
                name: "BidderID1",
                table: "Biddings");

            migrationBuilder.AddForeignKey(
                name: "FK_Biddings_Bidders_BidderID",
                table: "Biddings",
                column: "BidderID",
                principalTable: "Bidders",
                principalColumn: "BidderID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Biddings_Bidders_BidderID",
                table: "Biddings");

            migrationBuilder.AddColumn<int>(
                name: "BidderID1",
                table: "Biddings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Biddings_BidderID1",
                table: "Biddings",
                column: "BidderID1");

            migrationBuilder.AddForeignKey(
                name: "FK_Biddings_Bidders_BidderID",
                table: "Biddings",
                column: "BidderID",
                principalTable: "Bidders",
                principalColumn: "BidderID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Biddings_Bidders_BidderID1",
                table: "Biddings",
                column: "BidderID1",
                principalTable: "Bidders",
                principalColumn: "BidderID");
        }
    }
}
