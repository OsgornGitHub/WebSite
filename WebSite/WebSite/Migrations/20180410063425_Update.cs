using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebSite.Migrations
{
    public partial class Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropColumn(
                name: "LinkToPerson",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "NamePerson",
                table: "Albums");

            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    PersonFk = table.Column<Guid>(nullable: false),
                    Biography = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Photo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.PersonFk);
                });

            migrationBuilder.CreateTable(
                name: "Similars",
                columns: table => new
                {
                    ArtistId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Photo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Similars", x => x.ArtistId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropTable(
                name: "Similars");

            migrationBuilder.AddColumn<string>(
                name: "LinkToPerson",
                table: "Albums",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NamePerson",
                table: "Albums",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonFk = table.Column<Guid>(nullable: false),
                    Biography = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Photo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonFk);
                });
        }
    }
}
