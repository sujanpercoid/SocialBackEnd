﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TClone.Migrations
{
    /// <inheritdoc />
    public partial class usernameeeeesssssadasdsaasdasd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Posts",
                newName: "Username");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Posts",
                newName: "UserName");
        }
    }
}
