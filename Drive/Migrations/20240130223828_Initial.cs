using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Drive.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BaseDirectorys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    dir = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    Author = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseDirectorys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseDirectorys_Users_Author",
                        column: x => x.Author,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DirectoryDescs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DirectoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectoryDescs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DirectoryDescs_BaseDirectorys_DirectoryId",
                        column: x => x.DirectoryId,
                        principalTable: "BaseDirectorys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAccessToBaseDirectorys",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BaseDirectoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HasReadPermission = table.Column<bool>(type: "bit", nullable: false),
                    HasWritePermission = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccessToBaseDirectorys", x => new { x.UserId, x.BaseDirectoryId });
                    table.ForeignKey(
                        name: "FK_UserAccessToBaseDirectorys_BaseDirectorys_BaseDirectoryId",
                        column: x => x.BaseDirectoryId,
                        principalTable: "BaseDirectorys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAccessToBaseDirectorys_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaseDirectorys_Author",
                table: "BaseDirectorys",
                column: "Author");

            migrationBuilder.CreateIndex(
                name: "IX_DirectoryDescs_DirectoryId",
                table: "DirectoryDescs",
                column: "DirectoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAccessToBaseDirectorys_BaseDirectoryId",
                table: "UserAccessToBaseDirectorys",
                column: "BaseDirectoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DirectoryDescs");

            migrationBuilder.DropTable(
                name: "UserAccessToBaseDirectorys");

            migrationBuilder.DropTable(
                name: "BaseDirectorys");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
