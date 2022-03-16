using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assessments_Modules_ModeuleId",
                table: "Assessments");

            migrationBuilder.RenameColumn(
                name: "ModeuleId",
                table: "Assessments",
                newName: "ModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_Assessments_ModeuleId",
                table: "Assessments",
                newName: "IX_Assessments_ModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assessments_Modules_ModuleId",
                table: "Assessments",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assessments_Modules_ModuleId",
                table: "Assessments");

            migrationBuilder.RenameColumn(
                name: "ModuleId",
                table: "Assessments",
                newName: "ModeuleId");

            migrationBuilder.RenameIndex(
                name: "IX_Assessments_ModuleId",
                table: "Assessments",
                newName: "IX_Assessments_ModeuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assessments_Modules_ModeuleId",
                table: "Assessments",
                column: "ModeuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
