using Microsoft.EntityFrameworkCore.Migrations;

namespace NETCORE3.Data.Migrations
{
    public partial class updateVatTu1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GhiChu",
                table: "VatTus");

            migrationBuilder.AddColumn<string>(
                name: "ViTri",
                table: "VatTus",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ViTri",
                table: "VatTus");

            migrationBuilder.AddColumn<string>(
                name: "GhiChu",
                table: "VatTus",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
