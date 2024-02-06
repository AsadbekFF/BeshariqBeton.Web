using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeshariqBeton.DAL.Migrations
{
    public partial class SaleDebtPaymentLetterNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DebtPaid",
                table: "Sales",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LetterNumber",
                table: "Sales",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte>(
                name: "PaymentType",
                table: "Sales",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DebtPaid",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "LetterNumber",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "Sales");
        }
    }
}
