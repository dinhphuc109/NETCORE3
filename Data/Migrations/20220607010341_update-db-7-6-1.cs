using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NETCORE3.Data.Migrations
{
    public partial class updatedb761 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChiTietHangs",
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
                    Hang_Id = table.Column<Guid>(nullable: false),
                    Nhom_Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietHangs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietHangs_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChiTietHangs_Hangs_Hang_Id",
                        column: x => x.Hang_Id,
                        principalTable: "Hangs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChiTietHangs_Nhoms_Nhom_Id",
                        column: x => x.Nhom_Id,
                        principalTable: "Nhoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietNhoms",
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
                    table.PrimaryKey("PK_ChiTietNhoms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietNhoms_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChiTietNhoms_Loais_Loai_Id",
                        column: x => x.Loai_Id,
                        principalTable: "Loais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChiTietNhoms_Nhoms_Nhom_Id",
                        column: x => x.Nhom_Id,
                        principalTable: "Nhoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHangs_CreatedBy",
                table: "ChiTietHangs",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHangs_Hang_Id",
                table: "ChiTietHangs",
                column: "Hang_Id");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHangs_Nhom_Id",
                table: "ChiTietHangs",
                column: "Nhom_Id");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietNhoms_CreatedBy",
                table: "ChiTietNhoms",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietNhoms_Loai_Id",
                table: "ChiTietNhoms",
                column: "Loai_Id");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietNhoms_Nhom_Id",
                table: "ChiTietNhoms",
                column: "Nhom_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietHangs");

            migrationBuilder.DropTable(
                name: "ChiTietNhoms");
        }
    }
}
