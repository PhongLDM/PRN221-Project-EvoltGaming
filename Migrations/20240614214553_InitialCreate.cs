using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvoltingStore.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    gameId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    publisher = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    releaseDate = table.Column<DateTime>(type: "date", nullable: false),
                    updateDate = table.Column<DateTime>(type: "date", nullable: false),
                    platform = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    pirateLink = table.Column<string>(type: "text", nullable: true),
                    officialLink = table.Column<string>(type: "text", nullable: true),
                    image = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.gameId);
                });

            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    genreId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    genreName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.genreId);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    roleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    roleName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.roleId);
                });

            migrationBuilder.CreateTable(
                name: "GameRequirement",
                columns: table => new
                {
                    gameId = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    os = table.Column<string>(type: "text", nullable: false),
                    processor = table.Column<string>(type: "text", nullable: false),
                    memory = table.Column<double>(type: "float", nullable: false),
                    storage = table.Column<double>(type: "float", nullable: false),
                    directX = table.Column<int>(type: "int", nullable: true),
                    graphic = table.Column<string>(type: "text", nullable: true),
                    other = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameRequirement", x => new { x.gameId, x.type });
                    table.ForeignKey(
                        name: "FK_GameRequirement_Game",
                        column: x => x.gameId,
                        principalTable: "Game",
                        principalColumn: "gameId");
                });

            migrationBuilder.CreateTable(
                name: "GameGenre",
                columns: table => new
                {
                    gameId = table.Column<int>(type: "int", nullable: false),
                    genreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameGenre", x => new { x.gameId, x.genreId });
                    table.ForeignKey(
                        name: "FK_GameGenre_Game",
                        column: x => x.gameId,
                        principalTable: "Game",
                        principalColumn: "gameId");
                    table.ForeignKey(
                        name: "FK_GameGenre_Genre",
                        column: x => x.genreId,
                        principalTable: "Genre",
                        principalColumn: "genreId");
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    roleId = table.Column<int>(type: "int", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.userId);
                    table.ForeignKey(
                        name: "FK_User_Role",
                        column: x => x.roleId,
                        principalTable: "Role",
                        principalColumn: "roleId");
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    commentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    gameId = table.Column<int>(type: "int", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    postTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.commentId);
                    table.ForeignKey(
                        name: "FK_Comment_Game",
                        column: x => x.gameId,
                        principalTable: "Game",
                        principalColumn: "gameId");
                    table.ForeignKey(
                        name: "FK_Comment_User",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "userId");
                });

            migrationBuilder.CreateTable(
                name: "Favourite",
                columns: table => new
                {
                    gameId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favourite", x => new { x.gameId, x.userId });
                    table.ForeignKey(
                        name: "FK_Favourite_Game",
                        column: x => x.gameId,
                        principalTable: "Game",
                        principalColumn: "gameId");
                    table.ForeignKey(
                        name: "FK_Favourite_User",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "userId");
                });

            migrationBuilder.CreateTable(
                name: "UserDetail",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false),
                    firstName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    lastName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    createdDate = table.Column<DateTime>(type: "date", nullable: false),
                    image = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDetail", x => x.userId);
                    table.ForeignKey(
                        name: "FK_UserDetail_User",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "userId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_gameId",
                table: "Comment",
                column: "gameId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_userId",
                table: "Comment",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Favourite_userId",
                table: "Favourite",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_GameGenre_genreId",
                table: "GameGenre",
                column: "genreId");

            migrationBuilder.CreateIndex(
                name: "IX_User_roleId",
                table: "User",
                column: "roleId");

            migrationBuilder.CreateIndex(
                name: "Unique_username",
                table: "User",
                column: "username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Favourite");

            migrationBuilder.DropTable(
                name: "GameGenre");

            migrationBuilder.DropTable(
                name: "GameRequirement");

            migrationBuilder.DropTable(
                name: "UserDetail");

            migrationBuilder.DropTable(
                name: "Genre");

            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
