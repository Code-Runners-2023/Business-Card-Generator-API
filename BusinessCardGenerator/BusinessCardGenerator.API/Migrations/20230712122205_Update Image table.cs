using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessCardGenerator.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImagetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Length",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Images",
                newName: "FileExtension");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileExtension",
                table: "Images",
                newName: "FileName");

            migrationBuilder.AddColumn<long>(
                name: "Length",
                table: "Images",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
