using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RekazTest.Migrations
{
    /// <inheritdoc />
    public partial class EditDataTypeOfDataColumnAndRemoveBackendColumnFromMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackendType",
                table: "BlobsMetadata");

            migrationBuilder.AlterColumn<string>(
                name: "Data",
                table: "Blobs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackendType",
                table: "BlobsMetadata",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "Data",
                table: "Blobs",
                type: "varbinary(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
