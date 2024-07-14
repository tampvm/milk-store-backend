using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MilkStore.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAddressTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "Address",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "Address");
        }
    }
}
