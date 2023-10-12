using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YBS.Data.Migrations
{
    public partial class Addupdatrequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UpdateRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    Hotline = table.Column<string>(type: "varchar(15)", nullable: false),
                    Logo = table.Column<string>(type: "varchar(255)", nullable: false),
                    FacebookURL = table.Column<string>(type: "varchar(255)", nullable: true),
                    InstagramURL = table.Column<string>(type: "varchar(255)", nullable: true),
                    LinkedInURL = table.Column<string>(type: "varchar(255)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpdateRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UpdateRequest_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UpdateRequest_CompanyId",
                table: "UpdateRequest",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UpdateRequest");
        }
    }
}
