using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TClone.Migrations
{
    /// <inheritdoc />
    public partial class username : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Posts",
                newName: "UserName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Posts",
                newName: "UserId");
        }
    }
}
