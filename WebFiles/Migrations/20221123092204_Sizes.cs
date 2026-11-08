using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebFiles.Migrations
{
    public partial class Sizes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Size",
                table: "Files",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "AvailableSpace",
                table: "AspNetUsers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "AvailableSpace",
                table: "AspNetUsers");
        }
    }
}
