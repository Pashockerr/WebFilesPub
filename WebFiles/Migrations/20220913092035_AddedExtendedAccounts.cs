using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebFiles.Migrations
{
    public partial class AddedExtendedAccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsExtendedAccount",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExtendedAccount",
                table: "AspNetUsers");
        }
    }
}
