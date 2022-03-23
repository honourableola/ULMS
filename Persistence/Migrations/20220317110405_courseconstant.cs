using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class courseconstant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assessments_Instructors_InstructorId",
                table: "Assessments");

            migrationBuilder.AddColumn<bool>(
                name: "IsTaken",
                table: "Topics",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AssessmentGenerated",
                table: "Modules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTaken",
                table: "Modules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "DurationOfAssessment",
                table: "CourseConstants",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AlterColumn<Guid>(
                name: "InstructorId",
                table: "Assessments",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Assessments_Instructors_InstructorId",
                table: "Assessments",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assessments_Instructors_InstructorId",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "IsTaken",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "AssessmentGenerated",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "IsTaken",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "DurationOfAssessment",
                table: "CourseConstants");

            migrationBuilder.AlterColumn<Guid>(
                name: "InstructorId",
                table: "Assessments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Assessments_Instructors_InstructorId",
                table: "Assessments",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
