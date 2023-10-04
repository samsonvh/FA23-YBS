using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YBS.Data.Migrations
{
    public partial class AddmigrationactivitydockdockActivityyachtyachtTyperouteYachtType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Account");

            migrationBuilder.CreateTable(
                name: "DockActivity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityId = table.Column<int>(type: "int", nullable: true),
                    DockId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DockActivity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DockActivity_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DockActivity_Dock_DockId",
                        column: x => x.DockId,
                        principalTable: "Dock",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Service_Company_CompanyId",
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
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
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
                name: "RouteYachtType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    YachtTypeId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteYachtType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteYachtType_Route_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Route",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RouteYachtType_YachtType_YachtTypeId",
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
                    YachtTypeId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    LOA = table.Column<float>(type: "real", nullable: false),
                    BEAM = table.Column<float>(type: "real", nullable: false),
                    DRAFT = table.Column<float>(type: "real", nullable: false),
                    FuelCapacity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaximumGuestLimit = table.Column<int>(type: "int", nullable: false),
                    Cabin = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                name: "IX_DockActivity_ActivityId",
                table: "DockActivity",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_DockActivity_DockId",
                table: "DockActivity",
                column: "DockId");

            migrationBuilder.CreateIndex(
                name: "IX_DockYachtType_DockId",
                table: "DockYachtType",
                column: "DockId");

            migrationBuilder.CreateIndex(
                name: "IX_DockYachtType_YachtTypeId",
                table: "DockYachtType",
                column: "YachtTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteYachtType_RouteId",
                table: "RouteYachtType",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteYachtType_YachtTypeId",
                table: "RouteYachtType",
                column: "YachtTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_CompanyId",
                table: "Service",
                column: "CompanyId");

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
                name: "DockActivity");

            migrationBuilder.DropTable(
                name: "DockYachtType");

            migrationBuilder.DropTable(
                name: "RouteYachtType");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "Yacht");

            migrationBuilder.DropTable(
                name: "YachtType");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Account",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }
    }
}
