using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileDownLoadSystem.Core.Migrations
{
    public partial class UpdateTabele : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileId",
                table: "FileModel",
                newName: "FileTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileTypeId",
                table: "FileModel",
                newName: "FileId");
        }
    }
}
