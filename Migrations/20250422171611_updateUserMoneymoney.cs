using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotNET_courseproject_CourseRegister.Migrations
{
    /// <inheritdoc />
    public partial class updateUserMoneymoney : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Money",
                table: "Users",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Money",
                table: "Users");
        }
    }
}
