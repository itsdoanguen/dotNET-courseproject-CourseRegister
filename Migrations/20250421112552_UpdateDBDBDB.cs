using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotNET_courseproject_CourseRegister.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDBDBDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RegistedTime",
                table: "UserCourses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CourseImage",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistedTime",
                table: "UserCourses");

            migrationBuilder.DropColumn(
                name: "CourseImage",
                table: "Courses");
        }
    }
}
