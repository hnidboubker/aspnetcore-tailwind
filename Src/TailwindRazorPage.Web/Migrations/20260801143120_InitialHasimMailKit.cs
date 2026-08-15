//using System;
//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace TailwindRazorPage.Web.Migrations
//{
//    /// <inheritdoc />
//    public partial class InitialHasimMailKit : Migration
//    {
//        /// <inheritdoc />
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.CreateTable(
//                name: "AspNetRoles",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
//                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
//                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "EmailMessages",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    From = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    To = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    IsHtml = table.Column<bool>(type: "bit", nullable: false),
//                    Status = table.Column<int>(type: "int", nullable: false),
//                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
//                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_EmailMessages", x => x.Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "User",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    PictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    PictureData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
//                    PictureContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
//                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
//                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
//                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
//                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
//                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
//                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
//                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
//                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
//                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
//                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_User", x => x.Id);
//                });

//            migrationBuilder.CreateTable(
//                name: "AspNetRoleClaims",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    RoleId = table.Column<int>(type: "int", nullable: false),
//                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
//                        column: x => x.RoleId,
//                        principalTable: "AspNetRoles",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "AspNetUserClaims",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    UserId = table.Column<int>(type: "int", nullable: false),
//                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_AspNetUserClaims_User_UserId",
//                        column: x => x.UserId,
//                        principalTable: "User",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "AspNetUserLogins",
//                columns: table => new
//                {
//                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
//                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
//                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    UserId = table.Column<int>(type: "int", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
//                    table.ForeignKey(
//                        name: "FK_AspNetUserLogins_User_UserId",
//                        column: x => x.UserId,
//                        principalTable: "User",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "AspNetUserRoles",
//                columns: table => new
//                {
//                    UserId = table.Column<int>(type: "int", nullable: false),
//                    RoleId = table.Column<int>(type: "int", nullable: false)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
//                    table.ForeignKey(
//                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
//                        column: x => x.RoleId,
//                        principalTable: "AspNetRoles",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                    table.ForeignKey(
//                        name: "FK_AspNetUserRoles_User_UserId",
//                        column: x => x.UserId,
//                        principalTable: "User",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "AspNetUserTokens",
//                columns: table => new
//                {
//                    UserId = table.Column<int>(type: "int", nullable: false),
//                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
//                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
//                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
//                    table.ForeignKey(
//                        name: "FK_AspNetUserTokens_User_UserId",
//                        column: x => x.UserId,
//                        principalTable: "User",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateTable(
//                name: "AuditLog",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    UserId = table.Column<int>(type: "int", nullable: true),
//                    Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
//                    EntityName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
//                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
//                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
//                    CorrelationId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_AuditLog", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_AuditLog_User_UserId",
//                        column: x => x.UserId,
//                        principalTable: "User",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Restrict);
//                });

//            migrationBuilder.CreateTable(
//                name: "EntityChange",
//                columns: table => new
//                {
//                    Id = table.Column<int>(type: "int", nullable: false)
//                        .Annotation("SqlServer:Identity", "1, 1"),
//                    AuditLogId = table.Column<int>(type: "int", nullable: false),
//                    EntityName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
//                    PropertyName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
//                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
//                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
//                },
//                constraints: table =>
//                {
//                    table.PrimaryKey("PK_EntityChange", x => x.Id);
//                    table.ForeignKey(
//                        name: "FK_EntityChange_AuditLog_AuditLogId",
//                        column: x => x.AuditLogId,
//                        principalTable: "AuditLog",
//                        principalColumn: "Id",
//                        onDelete: ReferentialAction.Cascade);
//                });

//            migrationBuilder.CreateIndex(
//                name: "IX_AspNetRoleClaims_RoleId",
//                table: "AspNetRoleClaims",
//                column: "RoleId");

//            migrationBuilder.CreateIndex(
//                name: "RoleNameIndex",
//                table: "AspNetRoles",
//                column: "NormalizedName",
//                unique: true,
//                filter: "[NormalizedName] IS NOT NULL");

//            migrationBuilder.CreateIndex(
//                name: "IX_AspNetUserClaims_UserId",
//                table: "AspNetUserClaims",
//                column: "UserId");

//            migrationBuilder.CreateIndex(
//                name: "IX_AspNetUserLogins_UserId",
//                table: "AspNetUserLogins",
//                column: "UserId");

//            migrationBuilder.CreateIndex(
//                name: "IX_AspNetUserRoles_RoleId",
//                table: "AspNetUserRoles",
//                column: "RoleId");

//            migrationBuilder.CreateIndex(
//                name: "IX_AuditLog_Timestamp",
//                table: "AuditLog",
//                column: "Timestamp");

//            migrationBuilder.CreateIndex(
//                name: "IX_AuditLog_UserId",
//                table: "AuditLog",
//                column: "UserId");

//            migrationBuilder.CreateIndex(
//                name: "IX_EntityChange_AuditLogId",
//                table: "EntityChange",
//                column: "AuditLogId");

//            migrationBuilder.CreateIndex(
//                name: "EmailIndex",
//                table: "User",
//                column: "NormalizedEmail");

//            migrationBuilder.CreateIndex(
//                name: "UserNameIndex",
//                table: "User",
//                column: "NormalizedUserName",
//                unique: true,
//                filter: "[NormalizedUserName] IS NOT NULL");
//        }

//        /// <inheritdoc />
//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DropTable(
//                name: "AspNetRoleClaims");

//            migrationBuilder.DropTable(
//                name: "AspNetUserClaims");

//            migrationBuilder.DropTable(
//                name: "AspNetUserLogins");

//            migrationBuilder.DropTable(
//                name: "AspNetUserRoles");

//            migrationBuilder.DropTable(
//                name: "AspNetUserTokens");

//            migrationBuilder.DropTable(
//                name: "EmailMessages");

//            migrationBuilder.DropTable(
//                name: "EntityChange");

//            migrationBuilder.DropTable(
//                name: "AspNetRoles");

//            migrationBuilder.DropTable(
//                name: "AuditLog");

//            migrationBuilder.DropTable(
//                name: "User");
//        }
//    }
//}
