using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NETCORE3.Data.Migrations
{
    public partial class updatedb961 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LoaiId",
                table: "ChiTietNhoms",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Nhom_Id",
                table: "ChiTietHangs",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietNhoms_LoaiId",
                table: "ChiTietNhoms",
                column: "LoaiId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietNhoms_Loais_LoaiId",
                table: "ChiTietNhoms",
                column: "LoaiId",
                principalTable: "Loais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietHangs_Nhoms_Nhom_Id",
                table: "ChiTietHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietNhoms_Loais_LoaiId",
                table: "ChiTietNhoms");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietNhoms_LoaiId",
                table: "ChiTietNhoms");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietHangs_Nhom_Id",
                table: "ChiTietHangs");

            migrationBuilder.DropColumn(
                name: "LoaiId",
                table: "ChiTietNhoms");

            migrationBuilder.DropColumn(
                name: "Nhom_Id",
                table: "ChiTietHangs");
        }
    }
}
