using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MilkStore.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAddressTable2nd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Address",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Address",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Address");
        }
    }
}
