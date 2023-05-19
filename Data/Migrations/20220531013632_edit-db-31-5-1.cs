using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NETCORE3.Data.Migrations
{
    public partial class editdb3151 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VatTus_NhaCungCaps_NhaCungCap_Id",
                table: "VatTus");

            migrationBuilder.DropIndex(
                name: "IX_VatTus_NhaCungCap_Id",
                table: "VatTus");

            migrationBuilder.DropColumn(
                name: "NhaCungCap_Id",
                table: "VatTus");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "NhaCungCap_Id",
                table: "VatTus",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VatTus_NhaCungCap_Id",
                table: "VatTus",
                column: "NhaCungCap_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VatTus_NhaCungCaps_NhaCungCap_Id",
                table: "VatTus",
                column: "NhaCungCap_Id",
                principalTable: "NhaCungCaps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
