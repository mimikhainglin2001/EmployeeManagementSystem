using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeesManagement.Migrations
{
    /// <inheritdoc />
    public partial class FixedAssetDetailsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FixedAssets_SystemCodeDetails_CategotyId",
                table: "FixedAssets");

            migrationBuilder.DropIndex(
                name: "IX_FixedAssets_CategotyId",
                table: "FixedAssets");

            migrationBuilder.DropColumn(
                name: "CategotyId",
                table: "FixedAssets");

            migrationBuilder.CreateIndex(
                name: "IX_FixedAssets_CategoryId",
                table: "FixedAssets",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_FixedAssets_SystemCodeDetails_CategoryId",
                table: "FixedAssets",
                column: "CategoryId",
                principalTable: "SystemCodeDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FixedAssets_SystemCodeDetails_CategoryId",
                table: "FixedAssets");

            migrationBuilder.DropIndex(
                name: "IX_FixedAssets_CategoryId",
                table: "FixedAssets");

            migrationBuilder.AddColumn<int>(
                name: "CategotyId",
                table: "FixedAssets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FixedAssets_CategotyId",
                table: "FixedAssets",
                column: "CategotyId");

            migrationBuilder.AddForeignKey(
                name: "FK_FixedAssets_SystemCodeDetails_CategotyId",
                table: "FixedAssets",
                column: "CategotyId",
                principalTable: "SystemCodeDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
