using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileDownLoadSystem.Core.Migrations
{
    public partial class AddFilePackagesToFileModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FilePackages_FileId",
                table: "FilePackages",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_FilePackages_FileModel_FileId",
                table: "FilePackages",
                column: "FileId",
                principalTable: "FileModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilePackages_FileModel_FileId",
                table: "FilePackages");

            migrationBuilder.DropIndex(
                name: "IX_FilePackages_FileId",
                table: "FilePackages");
        }
    }
}
