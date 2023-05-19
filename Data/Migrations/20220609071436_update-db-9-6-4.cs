using Microsoft.EntityFrameworkCore.Migrations;

namespace NETCORE3.Data.Migrations
{
    public partial class updatedb964 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietHangs_Nhoms_Nhom_Id",
                table: "ChiTietHangs");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietHangs_Nhom_Id",
                table: "ChiTietHangs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHangs_Nhom_Id",
                table: "ChiTietHangs",
                column: "Nhom_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietHangs_Nhoms_Nhom_Id",
                table: "ChiTietHangs",
                column: "Nhom_Id",
                principalTable: "Nhoms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
