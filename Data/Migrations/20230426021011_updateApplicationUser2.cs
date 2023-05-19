using Microsoft.EntityFrameworkCore.Migrations;

namespace NETCORE3.Data.Migrations
{
    public partial class updateApplicationUser2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BoPhan_Id",
                table: "AspNetUsers",
                column: "BoPhan_Id");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DonVi_Id",
                table: "AspNetUsers",
                column: "DonVi_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_BoPhans_BoPhan_Id",
                table: "AspNetUsers",
                column: "BoPhan_Id",
                principalTable: "BoPhans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_DonVis_DonVi_Id",
                table: "AspNetUsers",
                column: "DonVi_Id",
                principalTable: "DonVis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_BoPhans_BoPhan_Id",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_DonVis_DonVi_Id",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BoPhan_Id",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DonVi_Id",
                table: "AspNetUsers");
        }
    }
}
