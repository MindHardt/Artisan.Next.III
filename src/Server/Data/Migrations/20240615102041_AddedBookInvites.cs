using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedBookInvites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookInvites",
                columns: table => new
                {
                    Key = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    BookName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookInvites", x => x.Key);
                    table.ForeignKey(
                        name: "FK_BookInvites_Books_BookName",
                        column: x => x.BookName,
                        principalTable: "Books",
                        principalColumn: "UrlName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookVisits",
                columns: table => new
                {
                    BookName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookVisits", x => new { x.BookName, x.UserId });
                    table.ForeignKey(
                        name: "FK_BookVisits_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookVisits_Books_BookName",
                        column: x => x.BookName,
                        principalTable: "Books",
                        principalColumn: "UrlName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookInvites_BookName",
                table: "BookInvites",
                column: "BookName");

            migrationBuilder.CreateIndex(
                name: "IX_BookVisits_UserId",
                table: "BookVisits",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookInvites");

            migrationBuilder.DropTable(
                name: "BookVisits");
        }
    }
}
