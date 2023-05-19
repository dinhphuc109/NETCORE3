using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NETCORE3.Data.Migrations
{
    public partial class updatedb764 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietHangs_Nhoms_Nhom_Id",
                table: "ChiTietHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietNhoms_Loais_Loai_Id",
                table: "ChiTietNhoms");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietNhoms_Loai_Id",
                table: "ChiTietNhoms");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietHangs_Nhom_Id",
                table: "ChiTietHangs");

            migrationBuilder.DropColumn(
                name: "Loai_Id",
                table: "ChiTietNhoms");

            migrationBuilder.DropColumn(
                name: "Nhom_Id",
                table: "ChiTietHangs");

            migrationBuilder.AddColumn<Guid>(
                name: "Hang_Id",
                table: "ChiTietNhoms",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Loai_Id",
                table: "ChiTietHangs",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietNhoms_Hang_Id",
                table: "ChiTietNhoms",
                column: "Hang_Id");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHangs_Loai_Id",
                table: "ChiTietHangs",
                column: "Loai_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietHangs_Loais_Loai_Id",
                table: "ChiTietHangs",
                column: "Loai_Id",
                principalTable: "Loais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietNhoms_Hangs_Hang_Id",
                table: "ChiTietNhoms",
                column: "Hang_Id",
                principalTable: "Hangs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietHangs_Loais_Loai_Id",
                table: "ChiTietHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietNhoms_Hangs_Hang_Id",
                table: "ChiTietNhoms");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietNhoms_Hang_Id",
                table: "ChiTietNhoms");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietHangs_Loai_Id",
                table: "ChiTietHangs");

            migrationBuilder.DropColumn(
                name: "Hang_Id",
                table: "ChiTietNhoms");

            migrationBuilder.DropColumn(
                name: "Loai_Id",
                table: "ChiTietHangs");

            migrationBuilder.AddColumn<Guid>(
                name: "Loai_Id",
                table: "ChiTietNhoms",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Nhom_Id",
                table: "ChiTietHangs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietNhoms_Loai_Id",
                table: "ChiTietNhoms",
                column: "Loai_Id");

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
                name: "FK_ChiTietNhoms_Loais_Loai_Id",
                table: "ChiTietNhoms",
                column: "Loai_Id",
                principalTable: "Loais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
