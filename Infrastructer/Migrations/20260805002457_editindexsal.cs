using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructer.Migrations
{
    /// <inheritdoc />
    public partial class editindexsal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Salaries_BarberID",
                table: "Salaries");

            migrationBuilder.CreateIndex(
                name: "IX_Salaries_BarberID",
                table: "Salaries",
                column: "BarberID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Salaries_BarberID",
                table: "Salaries");

            migrationBuilder.CreateIndex(
                name: "IX_Salaries_BarberID",
                table: "Salaries",
                column: "BarberID",
                unique: true);
        }
    }
}
