using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YBS.Data.Migrations
{
    public partial class Adddockyachtyachttype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longtitude = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getDate()"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getDate()"),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dock_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "YachtType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YachtType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DockYachtType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DockId = table.Column<int>(type: "int", nullable: false),
                    YachtTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DockYachtType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DockYachtType_Dock_DockId",
                        column: x => x.DockId,
                        principalTable: "Dock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DockYachtType_YachtType_YachtTypeId",
                        column: x => x.YachtTypeId,
                        principalTable: "YachtType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Yacht",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    YachtTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", nullable: true),
                    Manufacture = table.Column<string>(type: "varchar(100)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    LOA = table.Column<double>(type: "float", nullable: false),
                    BEAM = table.Column<double>(type: "float", nullable: false),
                    DRAFT = table.Column<double>(type: "float", nullable: false),
                    FuelCapacity = table.Column<string>(type: "varchar(20)", nullable: false),
                    MaximumGuestLimit = table.Column<int>(type: "int", nullable: false),
                    Cabin = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getDate()"),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Yacht", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Yacht_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Yacht_YachtType_YachtTypeId",
                        column: x => x.YachtTypeId,
                        principalTable: "YachtType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dock_CompanyId",
                table: "Dock",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DockYachtType_DockId",
                table: "DockYachtType",
                column: "DockId");

            migrationBuilder.CreateIndex(
                name: "IX_DockYachtType_YachtTypeId",
                table: "DockYachtType",
                column: "YachtTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Yacht_CompanyId",
                table: "Yacht",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Yacht_YachtTypeId",
                table: "Yacht",
                column: "YachtTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DockYachtType");

            migrationBuilder.DropTable(
                name: "Yacht");

            migrationBuilder.DropTable(
                name: "Dock");

            migrationBuilder.DropTable(
                name: "YachtType");
        }
    }
}
