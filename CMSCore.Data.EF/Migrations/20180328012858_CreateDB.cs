using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CMSCore.Data.EF.Migrations
{
    public partial class CreateDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ADV_AdvertistmentPages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADV_AdvertistmentPages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoleClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserLogins",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: true),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    ProviderKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserLogins", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "AppUserRoles",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserRoles", x => new { x.RoleId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "AppUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserTokens", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "BLO_Blogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    HomeFlag = table.Column<bool>(nullable: true),
                    HotFlag = table.Column<bool>(nullable: true),
                    Image = table.Column<string>(maxLength: 256, nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    SeoAlias = table.Column<string>(maxLength: 256, nullable: true),
                    SeoDescription = table.Column<string>(maxLength: 256, nullable: true),
                    SeoKeywords = table.Column<string>(maxLength: 256, nullable: true),
                    SeoPageTitle = table.Column<string>(maxLength: 256, nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Tags = table.Column<string>(nullable: true),
                    ViewCount = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BLO_Blogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MAN_Feedbacks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(maxLength: 250, nullable: true),
                    Message = table.Column<string>(maxLength: 500, nullable: true),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MAN_Feedbacks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MAN_Slides",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(nullable: true),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    DisplayOrder = table.Column<int>(nullable: true),
                    GroupAlias = table.Column<string>(maxLength: 25, nullable: false),
                    Image = table.Column<string>(maxLength: 250, nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    Url = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MAN_Slides", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MAN_Tags",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Type = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MAN_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PRD_Colors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 250, nullable: true),
                    Name = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRD_Colors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PRD_ProductCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    HomeFlag = table.Column<bool>(nullable: true),
                    HomeOrder = table.Column<int>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    LevelCate = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ParentId = table.Column<int>(nullable: false),
                    SeoAlias = table.Column<string>(nullable: true),
                    SeoDescription = table.Column<string>(nullable: true),
                    SeoKeywords = table.Column<string>(nullable: true),
                    SeoPageTitle = table.Column<string>(nullable: true),
                    SortOrder = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRD_ProductCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PRD_Sizes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRD_Sizes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SEC_Actions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEC_Actions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SEC_AppRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NormalizedName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEC_AppRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SEC_AppUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Avatar = table.Column<string>(nullable: true),
                    Balance = table.Column<decimal>(nullable: false),
                    BirthDay = table.Column<DateTime>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    SecurityStamp = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEC_AppUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SEC_Functions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    IconCss = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    ParentId = table.Column<string>(maxLength: 128, nullable: true),
                    SortOrder = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    URL = table.Column<string>(maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEC_Functions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SYS_ContactDetails",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 255, nullable: false),
                    Address = table.Column<string>(maxLength: 250, nullable: true),
                    Email = table.Column<string>(maxLength: 250, nullable: true),
                    Lat = table.Column<double>(nullable: true),
                    Lng = table.Column<double>(nullable: true),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Other = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(maxLength: 50, nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Website = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_ContactDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SYS_Footers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Content = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_Footers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SYS_Languages",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Resources = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SYS_Pages",
                columns: table => new
                {
                    Id = table.Column<int>(maxLength: 255, nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Alias = table.Column<string>(maxLength: 256, nullable: false),
                    Content = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_Pages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SYS_SystemConfigs",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 255, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Value1 = table.Column<string>(nullable: true),
                    Value2 = table.Column<int>(nullable: true),
                    Value3 = table.Column<bool>(nullable: true),
                    Value4 = table.Column<DateTime>(nullable: true),
                    Value5 = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_SystemConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ADV_AdvertistmentPositions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: true),
                    PageId = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADV_AdvertistmentPositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ADV_AdvertistmentPositions_ADV_AdvertistmentPages_PageId",
                        column: x => x.PageId,
                        principalTable: "ADV_AdvertistmentPages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BLO_BlogTags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BlogId = table.Column<int>(nullable: false),
                    TagId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BLO_BlogTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BLO_BlogTags_BLO_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "BLO_Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BLO_BlogTags_MAN_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "MAN_Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PRD_Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    HomeFlag = table.Column<bool>(nullable: true),
                    HotFlag = table.Column<bool>(nullable: true),
                    Image = table.Column<string>(maxLength: 255, nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    OriginalPrice = table.Column<decimal>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    PromotionPrice = table.Column<decimal>(nullable: true),
                    SeoAlias = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    SeoDescription = table.Column<string>(maxLength: 255, nullable: true),
                    SeoKeywords = table.Column<string>(maxLength: 255, nullable: true),
                    SeoPageTitle = table.Column<string>(nullable: true),
                    SortOrder = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Tags = table.Column<string>(maxLength: 255, nullable: true),
                    Unit = table.Column<string>(maxLength: 255, nullable: true),
                    ViewCount = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRD_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PRD_Products_PRD_ProductCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "PRD_ProductCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ANN_Announcements",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    Content = table.Column<string>(maxLength: 250, nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 250, nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ANN_Announcements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ANN_Announcements_SEC_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "SEC_AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PRD_Bills",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BillStatus = table.Column<int>(nullable: false),
                    CustomerAddress = table.Column<string>(maxLength: 256, nullable: false),
                    CustomerId = table.Column<Guid>(nullable: true),
                    CustomerMessage = table.Column<string>(maxLength: 256, nullable: false),
                    CustomerMobile = table.Column<string>(maxLength: 50, nullable: false),
                    CustomerName = table.Column<string>(maxLength: 256, nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    PaymentMethod = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRD_Bills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PRD_Bills_SEC_AppUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "SEC_AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SEC_Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActionId = table.Column<string>(maxLength: 128, nullable: false),
                    FunctionId = table.Column<string>(maxLength: 128, nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEC_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SEC_Permissions_SEC_Actions_ActionId",
                        column: x => x.ActionId,
                        principalTable: "SEC_Actions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SEC_Permissions_SEC_Functions_FunctionId",
                        column: x => x.FunctionId,
                        principalTable: "SEC_Functions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SEC_Permissions_SEC_AppRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "SEC_AppRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ADV_Advertistments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    Image = table.Column<string>(maxLength: 250, nullable: true),
                    Name = table.Column<string>(maxLength: 250, nullable: true),
                    PositionId = table.Column<string>(maxLength: 20, nullable: true),
                    SortOrder = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Url = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADV_Advertistments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ADV_Advertistments_ADV_AdvertistmentPositions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "ADV_AdvertistmentPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PRD_ProductImages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Caption = table.Column<string>(maxLength: 250, nullable: true),
                    Path = table.Column<string>(maxLength: 250, nullable: true),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRD_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PRD_ProductImages_PRD_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "PRD_Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PRD_ProductQuantities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ColorId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    SizeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRD_ProductQuantities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PRD_ProductQuantities_PRD_Colors_ColorId",
                        column: x => x.ColorId,
                        principalTable: "PRD_Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PRD_ProductQuantities_PRD_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "PRD_Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PRD_ProductQuantities_PRD_Sizes_SizeId",
                        column: x => x.SizeId,
                        principalTable: "PRD_Sizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PRD_ProductTags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<int>(nullable: false),
                    TagId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRD_ProductTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PRD_ProductTags_PRD_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "PRD_Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PRD_ProductTags_MAN_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "MAN_Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PRD_WholePrices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FromQuantity = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ToQuantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRD_WholePrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PRD_WholePrices_PRD_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "PRD_Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ANN_AnnouncementUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AnnouncementId = table.Column<string>(maxLength: 128, nullable: false),
                    HasRead = table.Column<bool>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ANN_AnnouncementUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ANN_AnnouncementUsers_ANN_Announcements_AnnouncementId",
                        column: x => x.AnnouncementId,
                        principalTable: "ANN_Announcements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PRD_BillDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BillId = table.Column<int>(nullable: false),
                    ColorId = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    SizeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRD_BillDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PRD_BillDetails_PRD_Bills_BillId",
                        column: x => x.BillId,
                        principalTable: "PRD_Bills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PRD_BillDetails_PRD_Colors_ColorId",
                        column: x => x.ColorId,
                        principalTable: "PRD_Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PRD_BillDetails_PRD_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "PRD_Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PRD_BillDetails_PRD_Sizes_SizeId",
                        column: x => x.SizeId,
                        principalTable: "PRD_Sizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ADV_AdvertistmentPositions_PageId",
                table: "ADV_AdvertistmentPositions",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_ADV_Advertistments_PositionId",
                table: "ADV_Advertistments",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_ANN_Announcements_UserId",
                table: "ANN_Announcements",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ANN_AnnouncementUsers_AnnouncementId",
                table: "ANN_AnnouncementUsers",
                column: "AnnouncementId");

            migrationBuilder.CreateIndex(
                name: "IX_BLO_BlogTags_BlogId",
                table: "BLO_BlogTags",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_BLO_BlogTags_TagId",
                table: "BLO_BlogTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_PRD_BillDetails_BillId",
                table: "PRD_BillDetails",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_PRD_BillDetails_ColorId",
                table: "PRD_BillDetails",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_PRD_BillDetails_ProductId",
                table: "PRD_BillDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PRD_BillDetails_SizeId",
                table: "PRD_BillDetails",
                column: "SizeId");

            migrationBuilder.CreateIndex(
                name: "IX_PRD_Bills_CustomerId",
                table: "PRD_Bills",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PRD_ProductImages_ProductId",
                table: "PRD_ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PRD_ProductQuantities_ColorId",
                table: "PRD_ProductQuantities",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_PRD_ProductQuantities_ProductId",
                table: "PRD_ProductQuantities",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PRD_ProductQuantities_SizeId",
                table: "PRD_ProductQuantities",
                column: "SizeId");

            migrationBuilder.CreateIndex(
                name: "IX_PRD_Products_CategoryId",
                table: "PRD_Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PRD_ProductTags_ProductId",
                table: "PRD_ProductTags",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PRD_ProductTags_TagId",
                table: "PRD_ProductTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_PRD_WholePrices_ProductId",
                table: "PRD_WholePrices",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SEC_Permissions_ActionId",
                table: "SEC_Permissions",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_SEC_Permissions_FunctionId",
                table: "SEC_Permissions",
                column: "FunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_SEC_Permissions_RoleId",
                table: "SEC_Permissions",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ADV_Advertistments");

            migrationBuilder.DropTable(
                name: "ANN_AnnouncementUsers");

            migrationBuilder.DropTable(
                name: "AppRoleClaims");

            migrationBuilder.DropTable(
                name: "AppUserClaims");

            migrationBuilder.DropTable(
                name: "AppUserLogins");

            migrationBuilder.DropTable(
                name: "AppUserRoles");

            migrationBuilder.DropTable(
                name: "AppUserTokens");

            migrationBuilder.DropTable(
                name: "BLO_BlogTags");

            migrationBuilder.DropTable(
                name: "MAN_Feedbacks");

            migrationBuilder.DropTable(
                name: "MAN_Slides");

            migrationBuilder.DropTable(
                name: "PRD_BillDetails");

            migrationBuilder.DropTable(
                name: "PRD_ProductImages");

            migrationBuilder.DropTable(
                name: "PRD_ProductQuantities");

            migrationBuilder.DropTable(
                name: "PRD_ProductTags");

            migrationBuilder.DropTable(
                name: "PRD_WholePrices");

            migrationBuilder.DropTable(
                name: "SEC_Permissions");

            migrationBuilder.DropTable(
                name: "SYS_ContactDetails");

            migrationBuilder.DropTable(
                name: "SYS_Footers");

            migrationBuilder.DropTable(
                name: "SYS_Languages");

            migrationBuilder.DropTable(
                name: "SYS_Pages");

            migrationBuilder.DropTable(
                name: "SYS_SystemConfigs");

            migrationBuilder.DropTable(
                name: "ADV_AdvertistmentPositions");

            migrationBuilder.DropTable(
                name: "ANN_Announcements");

            migrationBuilder.DropTable(
                name: "BLO_Blogs");

            migrationBuilder.DropTable(
                name: "PRD_Bills");

            migrationBuilder.DropTable(
                name: "PRD_Colors");

            migrationBuilder.DropTable(
                name: "PRD_Sizes");

            migrationBuilder.DropTable(
                name: "MAN_Tags");

            migrationBuilder.DropTable(
                name: "PRD_Products");

            migrationBuilder.DropTable(
                name: "SEC_Actions");

            migrationBuilder.DropTable(
                name: "SEC_Functions");

            migrationBuilder.DropTable(
                name: "SEC_AppRoles");

            migrationBuilder.DropTable(
                name: "ADV_AdvertistmentPages");

            migrationBuilder.DropTable(
                name: "SEC_AppUsers");

            migrationBuilder.DropTable(
                name: "PRD_ProductCategories");
        }
    }
}
