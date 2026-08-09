using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructer.Migrations
{
    /// <inheritdoc />
    public partial class barberedit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "StartDate",
                table: "Barbers",
                type: "date",
                nullable: false,
                defaultValueSql: "getdate()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Barbers");
        }
    }
}
