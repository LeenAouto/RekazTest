using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RekazTest.Migrations
{
    /// <inheritdoc />
    public partial class EditBlobsTableByDroppingSizeAndCreatedATColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Blobs");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Blobs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Blobs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Size",
                table: "Blobs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
