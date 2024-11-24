using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LatestUpdate.Migrations
{
    /// <inheritdoc />
    public partial class updatetableeeeeeeee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoldHistories_Crops_CropID",
                table: "SoldHistories");

            migrationBuilder.AddForeignKey(
                name: "FK_SoldHistories_Crops_CropID",
                table: "SoldHistories",
                column: "CropID",
                principalTable: "Crops",
                principalColumn: "CropID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoldHistories_Crops_CropID",
                table: "SoldHistories");

            migrationBuilder.AddForeignKey(
                name: "FK_SoldHistories_Crops_CropID",
                table: "SoldHistories",
                column: "CropID",
                principalTable: "Crops",
                principalColumn: "CropID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
