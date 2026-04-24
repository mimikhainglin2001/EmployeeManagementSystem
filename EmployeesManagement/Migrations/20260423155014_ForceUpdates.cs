using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeesManagement.Migrations
{
    /// <inheritdoc />
    public partial class ForceUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        ALTER TABLE FixedAssets
        ADD Id INT IDENTITY(1,1) NOT NULL;
    ");
            migrationBuilder.Sql(@"
        ALTER TABLE FixedAssets
        ADD CONSTRAINT PK_FixedAssets PRIMARY KEY (Id);
    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FixedAssets",
                table: "FixedAssets");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "FixedAssets");
        }
    }
}
