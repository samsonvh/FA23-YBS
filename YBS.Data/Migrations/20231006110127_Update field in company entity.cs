using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YBS.Data.Migrations
{
    public partial class Updatefieldincompanyentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Linkedln",
                table: "Company",
                newName: "LinkedIn");

            migrationBuilder.RenameColumn(
                name: "ConstractStartDate",
                table: "Company",
                newName: "ContractStartDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LinkedIn",
                table: "Company",
                newName: "Linkedln");

            migrationBuilder.RenameColumn(
                name: "ContractStartDate",
                table: "Company",
                newName: "ConstractStartDate");
        }
    }
}
