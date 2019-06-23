using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SB.AuthenticationDomain.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Authentication");

            migrationBuilder.CreateTable(
                name: "ChangeLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EntityName = table.Column<string>(nullable: true),
                    Operation = table.Column<int>(nullable: false),
                    PrimaryKey = table.Column<string>(nullable: true),
                    PropertyName = table.Column<string>(nullable: true),
                    OldValue = table.Column<string>(nullable: true),
                    NewValue = table.Column<string>(nullable: true),
                    ChangedBy = table.Column<string>(nullable: true),
                    ChangedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Authentication",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Hmac = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Username = table.Column<string>(maxLength: 50, nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    Password = table.Column<string>(maxLength: 255, nullable: false),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProfilPhoto",
                schema: "Authentication",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Hmac = table.Column<string>(nullable: false),
                    Photo = table.Column<byte[]>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfilPhoto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfilPhoto_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Authentication",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfilPhoto_UserId",
                schema: "Authentication",
                table: "ProfilPhoto",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                schema: "Authentication",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Username",
                schema: "Authentication",
                table: "User",
                column: "Username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChangeLogs");

            migrationBuilder.DropTable(
                name: "ProfilPhoto",
                schema: "Authentication");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Authentication");
        }
    }
}
