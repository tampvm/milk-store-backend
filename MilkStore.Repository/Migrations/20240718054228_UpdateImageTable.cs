using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MilkStore.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Brand_ImageId",
                table: "Brand");

            migrationBuilder.CreateIndex(
                name: "IX_Brand_ImageId",
                table: "Brand",
                column: "ImageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Brand_ImageId",
                table: "Brand");

            migrationBuilder.CreateIndex(
                name: "IX_Brand_ImageId",
                table: "Brand",
                column: "ImageId",
                unique: true,
                filter: "[ImageId] IS NOT NULL");
        }
    }
}
