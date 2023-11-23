using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YBS.Data.Migrations
{
    public partial class fixfacilitykey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Facility_Id",
                table: "Facility");

            migrationBuilder.CreateIndex(
                name: "IX_Facility_Id",
                table: "Facility",
                column: "Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Facility_Id",
                table: "Facility");

            migrationBuilder.CreateIndex(
                name: "IX_Facility_Id",
                table: "Facility",
                column: "Id");
        }
    }
}
