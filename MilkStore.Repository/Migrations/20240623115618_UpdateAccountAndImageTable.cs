using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MilkStore.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAccountAndImageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Image",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Account",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "FacebookEmail",
                table: "Account",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoogleEmail",
                table: "Account",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "FacebookEmail",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "GoogleEmail",
                table: "Account");

            migrationBuilder.AlterColumn<bool>(
                name: "Gender",
                table: "Account",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
