using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NETCORE3.Data.Migrations
{
    public partial class vattu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VatTus",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<Guid>(nullable: true),
                    MaVatTu = table.Column<string>(maxLength: 50, nullable: false),
                    TenVatTu = table.Column<string>(maxLength: 250, nullable: false),
                    Hang_Id = table.Column<Guid>(nullable: true),
                    Nhom_Id = table.Column<Guid>(nullable: true),
                    DVT_Id = table.Column<Guid>(nullable: true),
                    GhiChu = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VatTus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VatTus_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VatTus_CreatedBy",
                table: "VatTus",
                column: "CreatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VatTus");
        }
    }
}
