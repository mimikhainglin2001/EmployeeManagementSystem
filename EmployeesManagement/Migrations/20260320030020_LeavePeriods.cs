using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeesManagement.Migrations
{
    /// <inheritdoc />
    public partial class LeavePeriods : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeavePeriod",
                table: "LeaveAdjustmentEntries");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LeaveAdjustmentDate",
                table: "LeaveAdjustmentEntries",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "LeavePeriodId",
                table: "LeaveAdjustmentEntries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "LeavePeriods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Closed = table.Column<bool>(type: "bit", nullable: false),
                    Locked = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeavePeriods", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeaveAdjustmentEntries_LeavePeriodId",
                table: "LeaveAdjustmentEntries",
                column: "LeavePeriodId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveAdjustmentEntries_LeavePeriods_LeavePeriodId",
                table: "LeaveAdjustmentEntries",
                column: "LeavePeriodId",
                principalTable: "LeavePeriods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveAdjustmentEntries_LeavePeriods_LeavePeriodId",
                table: "LeaveAdjustmentEntries");

            migrationBuilder.DropTable(
                name: "LeavePeriods");

            migrationBuilder.DropIndex(
                name: "IX_LeaveAdjustmentEntries_LeavePeriodId",
                table: "LeaveAdjustmentEntries");

            migrationBuilder.DropColumn(
                name: "LeavePeriodId",
                table: "LeaveAdjustmentEntries");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LeaveAdjustmentDate",
                table: "LeaveAdjustmentEntries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LeavePeriod",
                table: "LeaveAdjustmentEntries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
