using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NETCORE3.Data.Migrations
{
    public partial class updatedb1063 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietNhoms_Loais_LoaiId",
                table: "ChiTietNhoms");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietNhoms_LoaiId",
                table: "ChiTietNhoms");

            migrationBuilder.DropColumn(
                name: "LoaiId",
                table: "ChiTietNhoms");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LoaiId",
                table: "ChiTietNhoms",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietNhoms_LoaiId",
                table: "ChiTietNhoms",
                column: "LoaiId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietNhoms_Loais_LoaiId",
                table: "ChiTietNhoms",
                column: "LoaiId",
                principalTable: "Loais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
