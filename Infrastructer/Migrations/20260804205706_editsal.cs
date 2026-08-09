using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructer.Migrations
{
    /// <inheritdoc />
    public partial class editsal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DeductionAmount",
                table: "Salaries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TradeAmount",
                table: "Salaries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeductionAmount",
                table: "Salaries");

            migrationBuilder.DropColumn(
                name: "TradeAmount",
                table: "Salaries");
        }
    }
}
