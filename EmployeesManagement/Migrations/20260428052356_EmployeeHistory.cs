using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeesManagement.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    OldDepartmentId = table.Column<int>(type: "int", nullable: true),
                    NewDepartmentId = table.Column<int>(type: "int", nullable: true),
                    OldDesignationId = table.Column<int>(type: "int", nullable: true),
                    NewDesignationId = table.Column<int>(type: "int", nullable: true),
                    OldSalary = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    NewSalary = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    ChangeTypeId = table.Column<int>(type: "int", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Reason = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedById = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedById = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeHistories_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeHistories_AspNetUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeHistories_Departments_NewDepartmentId",
                        column: x => x.NewDepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeHistories_Departments_OldDepartmentId",
                        column: x => x.OldDepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeHistories_Designations_NewDesignationId",
                        column: x => x.NewDesignationId,
                        principalTable: "Designations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeHistories_Designations_OldDesignationId",
                        column: x => x.OldDesignationId,
                        principalTable: "Designations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeHistories_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeHistories_SystemCodeDetails_ChangeTypeId",
                        column: x => x.ChangeTypeId,
                        principalTable: "SystemCodeDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeHistories_ChangeTypeId",
                table: "EmployeeHistories",
                column: "ChangeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeHistories_CreatedById",
                table: "EmployeeHistories",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeHistories_EmployeeId",
                table: "EmployeeHistories",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeHistories_ModifiedById",
                table: "EmployeeHistories",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeHistories_NewDepartmentId",
                table: "EmployeeHistories",
                column: "NewDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeHistories_NewDesignationId",
                table: "EmployeeHistories",
                column: "NewDesignationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeHistories_OldDepartmentId",
                table: "EmployeeHistories",
                column: "OldDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeHistories_OldDesignationId",
                table: "EmployeeHistories",
                column: "OldDesignationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeHistories");
        }
    }
}
