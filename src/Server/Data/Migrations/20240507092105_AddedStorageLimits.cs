using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedStorageLimits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_AspNetUsers_UploaderId",
                table: "Files");

            migrationBuilder.AddColumn<long>(
                name: "CustomStorageLimit",
                table: "AspNetUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_AspNetUsers_UploaderId",
                table: "Files",
                column: "UploaderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_AspNetUsers_UploaderId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "CustomStorageLimit",
                table: "AspNetUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_AspNetUsers_UploaderId",
                table: "Files",
                column: "UploaderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
