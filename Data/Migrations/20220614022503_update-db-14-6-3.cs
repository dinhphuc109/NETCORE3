using Microsoft.EntityFrameworkCore.Migrations;

namespace NETCORE3.Data.Migrations
{
    public partial class updatedb1463 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TranThaiXuLy",
                table: "PhanHois");

            migrationBuilder.AddColumn<int>(
                name: "TrangThaiXuLy",
                table: "PhanHois",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrangThaiXuLy",
                table: "PhanHois");

            migrationBuilder.AddColumn<int>(
                name: "TranThaiXuLy",
                table: "PhanHois",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
