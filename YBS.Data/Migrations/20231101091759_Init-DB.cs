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
                    MoneyUnit = table.Column<string>(type: "varchar(10)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Point = table.Column<double>(type: "float", nullable: false),
                    EffectiveDuration = table.Column<int>(type: "int", nullable: false),
                    TimeUnit = table.Column<string>(type: "nvarchar(10)", nullable: false),
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
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FacebookURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstagramURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkedInURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    MembershipPackageId = table.Column<int>(type: "int", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(15)", nullable: false),
                    Nationality = table.Column<string>(type: "varchar(100)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    AvatarURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                name: "Route",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Beginning = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpectedStartingTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ExpectedEndingTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Route", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Route_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServicePackage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<float>(type: "real", nullable: false),
                    MoneyUnit = table.Column<string>(type: "varchar(10)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicePackage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServicePackage_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceType_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FacebookURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstagramURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkedInURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpdateRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UpdateRequest_Company_CompanyId",
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
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YachtType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YachtType_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MembershipRegistration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    MembershipPackageId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    MoneyUnit = table.Column<string>(type: "varchar(10)", nullable: false),
                    DateRegistered = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipRegistration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MembershipRegistration_Member_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MembershipRegistration_MembershipPackage_MembershipPackageId",
                        column: x => x.MembershipPackageId,
                        principalTable: "MembershipPackage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wallet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    Balance = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wallet_Member_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RouteServicePackage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    ServicePackageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteServicePackage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteServicePackage_Route_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Route",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RouteServicePackage_ServicePackage_ServicePackageId",
                        column: x => x.ServicePackageId,
                        principalTable: "ServicePackage",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoneyUnit = table.Column<string>(type: "varchar(10)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Service_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Service_ServiceType_ServiceTypeId",
                        column: x => x.ServiceTypeId,
                        principalTable: "ServiceType",
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
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DockYachtType_YachtType_YachtTypeId",
                        column: x => x.YachtTypeId,
                        principalTable: "YachtType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PriceMapper",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    YachtTypeId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    MoneyUnit = table.Column<string>(type: "varchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceMapper", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceMapper_Route_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Route",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PriceMapper_YachtType_YachtTypeId",
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
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "varchar(max)", nullable: true),
                    Manufacturer = table.Column<string>(type: "varchar(100)", nullable: false),
                    GrossTonnage = table.Column<int>(type: "int", nullable: false),
                    GrossTonnageUnit = table.Column<string>(type: "varchar(10)", nullable: false),
                    Range = table.Column<int>(type: "int", nullable: false),
                    RangeUnit = table.Column<string>(type: "varchar(20)", nullable: false),
                    TotalCrew = table.Column<int>(type: "int", nullable: false),
                    CrusingSpeed = table.Column<int>(type: "int", nullable: false),
                    MaxSpeed = table.Column<int>(type: "int", nullable: false),
                    SpeedUnit = table.Column<string>(type: "varchar(20)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    LOA = table.Column<string>(type: "varchar(20)", nullable: false),
                    BEAM = table.Column<string>(type: "varchar(20)", nullable: false),
                    DRAFT = table.Column<string>(type: "varchar(20)", nullable: false),
                    FuelCapacity = table.Column<int>(type: "int", nullable: false),
                    FuelCapacityUnit = table.Column<string>(type: "varchar(10)", nullable: false),
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
                name: "ServicePackageItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    ServicePackageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicePackageItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServicePackageItem_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ServicePackageItem_ServicePackage_ServicePackageId",
                        column: x => x.ServicePackageId,
                        principalTable: "ServicePackage",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    YachtId = table.Column<int>(type: "int", nullable: true),
                    MemberId = table.Column<int>(type: "int", nullable: true),
                    MembershipPackageId = table.Column<int>(type: "int", nullable: true),
                    YachtTypeId = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    TotalPrice = table.Column<float>(type: "real", nullable: false),
                    MoneyUnit = table.Column<string>(type: "varchar(10)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getDate()"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getDate()"),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Booking_Member_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Booking_MembershipPackage_MembershipPackageId",
                        column: x => x.MembershipPackageId,
                        principalTable: "MembershipPackage",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Booking_Route_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Route",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Booking_Yacht_YachtId",
                        column: x => x.YachtId,
                        principalTable: "Yacht",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Booking_YachtType_YachtTypeId",
                        column: x => x.YachtTypeId,
                        principalTable: "YachtType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Facility",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YachtId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facility", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Facility_Yacht_YachtId",
                        column: x => x.YachtId,
                        principalTable: "Yacht",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "YachtMooring",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DockId = table.Column<int>(type: "int", nullable: false),
                    YachtId = table.Column<int>(type: "int", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    LeaveTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YachtMooring", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YachtMooring_Dock_DockId",
                        column: x => x.DockId,
                        principalTable: "Dock",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_YachtMooring_Yacht_YachtId",
                        column: x => x.YachtId,
                        principalTable: "Yacht",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BookingPayment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    TotalPrice = table.Column<double>(type: "float", nullable: false),
                    MoneyUnit = table.Column<string>(type: "varchar(10)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingPayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingPayment_Booking_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingServicePackage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServicePackageId = table.Column<int>(type: "int", nullable: false),
                    BookingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingServicePackage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingServicePackage_Booking_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookingServicePackage_ServicePackage_ServicePackageId",
                        column: x => x.ServicePackageId,
                        principalTable: "ServicePackage",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Guest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: false),
                    IdentityNumber = table.Column<string>(type: "varchar(12)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(12)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    IsLeader = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Guest_Booking_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trip",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    ActualStartingTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ActualEndingTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trip", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trip_Booking_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingPaymentId = table.Column<int>(type: "int", nullable: true),
                    WalletId = table.Column<int>(type: "int", nullable: true),
                    MembershipRegistrationId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    MoneyUnit = table.Column<string>(type: "varchar(15)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transaction_BookingPayment_BookingPaymentId",
                        column: x => x.BookingPaymentId,
                        principalTable: "BookingPayment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transaction_MembershipRegistration_MembershipRegistrationId",
                        column: x => x.MembershipRegistrationId,
                        principalTable: "MembershipRegistration",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transaction_Wallet_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallet",
                        principalColumn: "Id");
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
                name: "IX_Booking_MemberId",
                table: "Booking",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_MembershipPackageId",
                table: "Booking",
                column: "MembershipPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_RouteId",
                table: "Booking",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_YachtId",
                table: "Booking",
                column: "YachtId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_YachtTypeId",
                table: "Booking",
                column: "YachtTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingPayment_BookingId",
                table: "BookingPayment",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookingServicePackage_BookingId",
                table: "BookingServicePackage",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingServicePackage_ServicePackageId",
                table: "BookingServicePackage",
                column: "ServicePackageId");

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
                name: "IX_Facility_Id",
                table: "Facility",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Facility_YachtId",
                table: "Facility",
                column: "YachtId");

            migrationBuilder.CreateIndex(
                name: "IX_Guest_BookingId",
                table: "Guest",
                column: "BookingId");

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
                name: "IX_MembershipRegistration_MemberId",
                table: "MembershipRegistration",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipRegistration_MembershipPackageId",
                table: "MembershipRegistration",
                column: "MembershipPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceMapper_RouteId",
                table: "PriceMapper",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceMapper_YachtTypeId",
                table: "PriceMapper",
                column: "YachtTypeId");

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
                name: "IX_Route_CompanyId",
                table: "Route",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteServicePackage_RouteId",
                table: "RouteServicePackage",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteServicePackage_ServicePackageId",
                table: "RouteServicePackage",
                column: "ServicePackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_CompanyId",
                table: "Service",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_ServiceTypeId",
                table: "Service",
                column: "ServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePackage_CompanyId",
                table: "ServicePackage",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePackageItem_ServiceId",
                table: "ServicePackageItem",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePackageItem_ServicePackageId",
                table: "ServicePackageItem",
                column: "ServicePackageId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceType_CompanyId",
                table: "ServiceType",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_BookingPaymentId",
                table: "Transaction",
                column: "BookingPaymentId",
                unique: true,
                filter: "[BookingPaymentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_MembershipRegistrationId",
                table: "Transaction",
                column: "MembershipRegistrationId",
                unique: true,
                filter: "[MembershipRegistrationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_WalletId",
                table: "Transaction",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Trip_BookingId",
                table: "Trip",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UpdateRequest_CompanyId",
                table: "UpdateRequest",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_MemberId",
                table: "Wallet",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Yacht_CompanyId",
                table: "Yacht",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Yacht_YachtTypeId",
                table: "Yacht",
                column: "YachtTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_YachtMooring_DockId",
                table: "YachtMooring",
                column: "DockId");

            migrationBuilder.CreateIndex(
                name: "IX_YachtMooring_YachtId",
                table: "YachtMooring",
                column: "YachtId");

            migrationBuilder.CreateIndex(
                name: "IX_YachtType_CompanyId",
                table: "YachtType",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingServicePackage");

            migrationBuilder.DropTable(
                name: "DockYachtType");

            migrationBuilder.DropTable(
                name: "Facility");

            migrationBuilder.DropTable(
                name: "Guest");

            migrationBuilder.DropTable(
                name: "PriceMapper");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "RouteServicePackage");

            migrationBuilder.DropTable(
                name: "ServicePackageItem");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "Trip");

            migrationBuilder.DropTable(
                name: "UpdateRequest");

            migrationBuilder.DropTable(
                name: "YachtMooring");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "ServicePackage");

            migrationBuilder.DropTable(
                name: "BookingPayment");

            migrationBuilder.DropTable(
                name: "MembershipRegistration");

            migrationBuilder.DropTable(
                name: "Wallet");

            migrationBuilder.DropTable(
                name: "Dock");

            migrationBuilder.DropTable(
                name: "ServiceType");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "Member");

            migrationBuilder.DropTable(
                name: "Route");

            migrationBuilder.DropTable(
                name: "Yacht");

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
