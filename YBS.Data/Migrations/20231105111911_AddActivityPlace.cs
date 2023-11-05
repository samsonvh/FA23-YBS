using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YBS.Data.Migrations
{
    public partial class AddActivityPlace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OccuringTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activity_Route_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Route",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityPlace",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    FromDockId = table.Column<int>(type: "int", nullable: true),
                    ToDockId = table.Column<int>(type: "int", nullable: true),
                    StartLocationLatitude = table.Column<float>(type: "real", nullable: true),
                    StartLocationLongtiude = table.Column<float>(type: "real", nullable: true),
                    EndLocationLatitude = table.Column<float>(type: "real", nullable: true),
                    EndLocationLongtiude = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityPlace", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityPlace_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActivityPlace_Dock_FromDockId",
                        column: x => x.FromDockId,
                        principalTable: "Dock",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActivityPlace_Dock_ToDockId",
                        column: x => x.ToDockId,
                        principalTable: "Dock",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activity_RouteId",
                table: "Activity",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityPlace_ActivityId",
                table: "ActivityPlace",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityPlace_FromDockId",
                table: "ActivityPlace",
                column: "FromDockId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityPlace_ToDockId",
                table: "ActivityPlace",
                column: "ToDockId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityPlace");

            migrationBuilder.DropTable(
                name: "Activity");
        }
    }
}
