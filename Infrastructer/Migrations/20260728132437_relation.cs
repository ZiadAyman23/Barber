using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructer.Migrations
{
    /// <inheritdoc />
    public partial class relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_Barbers_BarberId",
                table: "Attendance");

            migrationBuilder.DropForeignKey(
                name: "FK_Deductions_Barbers_BarberId",
                table: "Deductions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_Barbers_BarberId",
                table: "Attendance",
                column: "BarberId",
                principalTable: "Barbers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Deductions_Barbers_BarberId",
                table: "Deductions",
                column: "BarberId",
                principalTable: "Barbers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_Barbers_BarberId",
                table: "Attendance");

            migrationBuilder.DropForeignKey(
                name: "FK_Deductions_Barbers_BarberId",
                table: "Deductions");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "Orders",
                type: "date",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_Barbers_BarberId",
                table: "Attendance",
                column: "BarberId",
                principalTable: "Barbers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Deductions_Barbers_BarberId",
                table: "Deductions",
                column: "BarberId",
                principalTable: "Barbers",
                principalColumn: "Id");
        }
    }
}
