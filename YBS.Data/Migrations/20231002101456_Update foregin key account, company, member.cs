using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YBS.Data.Migrations
{
    public partial class Updateforeginkeyaccountcompanymember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Role_RoleID",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Company_Account_AccountId",
                table: "Company");

            migrationBuilder.DropForeignKey(
                name: "FK_Member_Account_AccountId",
                table: "Member");

            migrationBuilder.DropIndex(
                name: "IX_Member_AccountId",
                table: "Member");

            migrationBuilder.DropIndex(
                name: "IX_Company_AccountId",
                table: "Company");

            migrationBuilder.CreateIndex(
                name: "IX_Member_AccountId",
                table: "Member",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Company_AccountId",
                table: "Company",
                column: "AccountId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Role_RoleID",
                table: "Account",
                column: "RoleID",
                principalTable: "Role",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Company_Account_AccountId",
                table: "Company",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Member_Account_AccountId",
                table: "Member",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Role_RoleID",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Company_Account_AccountId",
                table: "Company");

            migrationBuilder.DropForeignKey(
                name: "FK_Member_Account_AccountId",
                table: "Member");

            migrationBuilder.DropIndex(
                name: "IX_Member_AccountId",
                table: "Member");

            migrationBuilder.DropIndex(
                name: "IX_Company_AccountId",
                table: "Company");

            migrationBuilder.CreateIndex(
                name: "IX_Member_AccountId",
                table: "Member",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_AccountId",
                table: "Company",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Role_RoleID",
                table: "Account",
                column: "RoleID",
                principalTable: "Role",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Company_Account_AccountId",
                table: "Company",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Member_Account_AccountId",
                table: "Member",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
