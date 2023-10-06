using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YBS.Data.Migrations
{
    public partial class FixAccountAndCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Linkedln",
                table: "Company",
                newName: "LinkedInUrl");

            migrationBuilder.RenameColumn(
                name: "ConstractStartDate",
                table: "Company",
                newName: "ContractStartDate");

            migrationBuilder.RenameColumn(
                name: "HashedPassword",
                table: "Account",
                newName: "Password");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LinkedInUrl",
                table: "Company",
                newName: "Linkedln");

            migrationBuilder.RenameColumn(
                name: "ContractStartDate",
                table: "Company",
                newName: "ConstractStartDate");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Account",
                newName: "HashedPassword");
        }
    }
}
