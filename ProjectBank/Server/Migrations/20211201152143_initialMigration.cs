using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectBank.Server.Migrations
{
    public partial class initialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Desc",
                table: "Projects",
                newName: "Description");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Projects",
                newName: "Desc");
        }
    }
}
