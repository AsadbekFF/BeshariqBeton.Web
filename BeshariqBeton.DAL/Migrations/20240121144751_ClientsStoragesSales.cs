using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeshariqBeton.DAL.Migrations
{
    public partial class ClientsStoragesSales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DistanceKm = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Storages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CementWeightKg = table.Column<int>(type: "int", nullable: false),
                    SandWeightKg = table.Column<int>(type: "int", nullable: false),
                    ShebenWeightKg = table.Column<int>(type: "int", nullable: false),
                    ChemicalWeightKg = table.Column<int>(type: "int", nullable: false),
                    MachineNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StorageType = table.Column<byte>(type: "tinyint", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ConcreteProductType = table.Column<byte>(type: "tinyint", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    ComeOutDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ComeInDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BottomCount = table.Column<int>(type: "int", nullable: true),
                    Sump60Count = table.Column<int>(type: "int", nullable: true),
                    Sump90Count = table.Column<int>(type: "int", nullable: true),
                    CoverCount = table.Column<int>(type: "int", nullable: true),
                    TotalPrice = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sales_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sales_ClientId",
                table: "Sales",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "Storages");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
