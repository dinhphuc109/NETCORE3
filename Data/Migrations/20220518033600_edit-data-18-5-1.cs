using Microsoft.EntityFrameworkCore.Migrations;

namespace NETCORE3.Data.Migrations
{
    public partial class editdata1851 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_VatTus_DVT_Id",
                table: "VatTus",
                column: "DVT_Id");

            migrationBuilder.CreateIndex(
                name: "IX_VatTus_Hang_Id",
                table: "VatTus",
                column: "Hang_Id");

            migrationBuilder.CreateIndex(
                name: "IX_VatTus_Nhom_Id",
                table: "VatTus",
                column: "Nhom_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VatTus_DonViTinhs_DVT_Id",
                table: "VatTus",
                column: "DVT_Id",
                principalTable: "DonViTinhs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VatTus_Hangs_Hang_Id",
                table: "VatTus",
                column: "Hang_Id",
                principalTable: "Hangs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VatTus_Nhoms_Nhom_Id",
                table: "VatTus",
                column: "Nhom_Id",
                principalTable: "Nhoms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VatTus_DonViTinhs_DVT_Id",
                table: "VatTus");

            migrationBuilder.DropForeignKey(
                name: "FK_VatTus_Hangs_Hang_Id",
                table: "VatTus");

            migrationBuilder.DropForeignKey(
                name: "FK_VatTus_Nhoms_Nhom_Id",
                table: "VatTus");

            migrationBuilder.DropIndex(
                name: "IX_VatTus_DVT_Id",
                table: "VatTus");

            migrationBuilder.DropIndex(
                name: "IX_VatTus_Hang_Id",
                table: "VatTus");

            migrationBuilder.DropIndex(
                name: "IX_VatTus_Nhom_Id",
                table: "VatTus");
        }
    }
}
