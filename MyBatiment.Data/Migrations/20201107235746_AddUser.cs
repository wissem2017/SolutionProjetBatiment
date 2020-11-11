using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyBatiment.Data.Migrations
{
    public partial class AddUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Owners",
                keyColumn: "Id",
                keyValue: new Guid("e6de4b11-9659-4a3c-8795-26c14b1df7b9"));

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    Username = table.Column<string>(maxLength: 50, nullable: false),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    isAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Owners",
                columns: new[] { "Id", "Adress", "Description", "Image", "Tel", "Title" },
                values: new object[] { new Guid("645afc49-6712-4f88-874f-9f7539c9f65d"), "Paris 93600", " ", "intro - bg.jpg", "0033 12 12 12 12 12", "Bâtiment" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DeleteData(
                table: "Owners",
                keyColumn: "Id",
                keyValue: new Guid("645afc49-6712-4f88-874f-9f7539c9f65d"));

            migrationBuilder.InsertData(
                table: "Owners",
                columns: new[] { "Id", "Adress", "Description", "Image", "Tel", "Title" },
                values: new object[] { new Guid("e6de4b11-9659-4a3c-8795-26c14b1df7b9"), "Paris 93600", " ", "intro - bg.jpg", "0033 12 12 12 12 12", "Bâtiment" });
        }
    }
}
