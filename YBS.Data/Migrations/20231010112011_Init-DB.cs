using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YBS.Data.Migrations
{
    public partial class InitDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MembershipPackage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Unit = table.Column<string>(type: "varchar(10)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Point = table.Column<double>(type: "float", nullable: false),
                    EffectiveDuration = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getDate()"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getDate()"),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipPackage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
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
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "varchar(50)", nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", nullable: false),
                    Password = table.Column<string>(type: "varchar(500)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getDate()"),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Account_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    HotLine = table.Column<string>(type: "varchar(15)", nullable: false),
                    Logo = table.Column<string>(type: "varchar(255)", nullable: false),
                    FacebookURL = table.Column<string>(type: "varchar(255)", nullable: true),
                    InstagramURL = table.Column<string>(type: "varchar(255)", nullable: true),
                    LinkedInURL = table.Column<string>(type: "varchar(255)", nullable: true),
                    ContractStartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getDate()"),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Company_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Member",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    MembershipPackageId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    DOB = table.Column<DateTime>(type: "date", nullable: false),
                    Nationality = table.Column<string>(type: "varchar(100)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Avatar = table.Column<string>(type: "varchar(255)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    IdentityNumber = table.Column<string>(type: "varchar(15)", nullable: false),
                    MembershipStartDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getDate()"),
                    MembershipExpiredDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    MembershipSinceDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getDate()"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getDate()"),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Member_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Member_MembershipPackage_MembershipPackageId",
                        column: x => x.MembershipPackageId,
                        principalTable: "MembershipPackage",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "varchar(100)", nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Account_Email",
                table: "Account",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_RoleId",
                table: "Account",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_Username",
                table: "Account",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Company_AccountId",
                table: "Company",
                column: "AccountId",
                unique: true);

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
                name: "IX_Member_AccountId",
                table: "Member",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Member_MembershipPackageId",
                table: "Member",
                column: "MembershipPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_AccountId",
                table: "RefreshToken",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_Token",
                table: "RefreshToken",
                column: "Token",
                unique: true);

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
                name: "Member");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "Yacht");

            migrationBuilder.DropTable(
                name: "Dock");

            migrationBuilder.DropTable(
                name: "MembershipPackage");

            migrationBuilder.DropTable(
                name: "YachtType");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
