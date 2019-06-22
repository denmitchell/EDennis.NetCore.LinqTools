using System;
using System.IO;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EDennis.Samples.ColorsRepo.EfCore.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Color",
                columns: table => new
                {
                    Name = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Red = table.Column<int>(nullable: false),
                    Green = table.Column<int>(nullable: false),
                    Blue = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pkColor", x => x.Name);
                });

            migrationBuilder.Sql(File.ReadAllText("MigrationsInserts\\Initial_Insert.sql"));

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Color");
        }
    }
}
