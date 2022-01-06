using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class NewMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientIPAddress = table.Column<string>(type: "text", nullable: true),
                    LastValidRequest = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastInvalidRequest = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    InvalidRequestsCounter = table.Column<int>(type: "integer", nullable: false),
                    RequestsCounter = table.Column<int>(type: "integer", nullable: false),
                    Credit = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
