using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NETCORE3.Data.Migrations
{
    public partial class updateVatTu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "User_Id",
                table: "VatTus",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VatTus_User_Id",
                table: "VatTus",
                column: "User_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VatTus_AspNetUsers_User_Id",
                table: "VatTus",
                column: "User_Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VatTus_AspNetUsers_User_Id",
                table: "VatTus");

            migrationBuilder.DropIndex(
                name: "IX_VatTus_User_Id",
                table: "VatTus");

            migrationBuilder.DropColumn(
                name: "User_Id",
                table: "VatTus");
        }
    }
}
