using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TClone.Migrations
{
    /// <inheritdoc />
    public partial class usernameeeeessss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Follower",
                table: "Follows",
                newName: "UserName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Follows",
                newName: "Follower");
        }
    }
}
