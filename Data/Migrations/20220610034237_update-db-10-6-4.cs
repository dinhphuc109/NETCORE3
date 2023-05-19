using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NETCORE3.Data.Migrations
{
    public partial class updatedb1064 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChiTietNhomLoais",
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
                    Nhom_Id = table.Column<Guid>(nullable: false),
                    Loai_Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietNhomLoais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietNhomLoais_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChiTietNhomLoais_Loais_Loai_Id",
                        column: x => x.Loai_Id,
                        principalTable: "Loais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChiTietNhomLoais_Nhoms_Nhom_Id",
                        column: x => x.Nhom_Id,
                        principalTable: "Nhoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietNhomLoais_CreatedBy",
                table: "ChiTietNhomLoais",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietNhomLoais_Loai_Id",
                table: "ChiTietNhomLoais",
                column: "Loai_Id");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietNhomLoais_Nhom_Id",
                table: "ChiTietNhomLoais",
                column: "Nhom_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietNhomLoais");
        }
    }
}
