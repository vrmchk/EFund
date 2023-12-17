using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFund.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddTagAndFundraising : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fundraisings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Provider = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fundraisings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fundraisings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "MonobankFundraisings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JarId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SendId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FundraisingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonobankFundraisings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonobankFundraisings_Fundraisings_FundraisingId",
                        column: x => x.FundraisingId,
                        principalTable: "Fundraisings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FundraisingTag",
                columns: table => new
                {
                    FundraisingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagsName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FundraisingTag", x => new { x.FundraisingsId, x.TagsName });
                    table.ForeignKey(
                        name: "FK_FundraisingTag_Fundraisings_FundraisingsId",
                        column: x => x.FundraisingsId,
                        principalTable: "Fundraisings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FundraisingTag_Tags_TagsName",
                        column: x => x.TagsName,
                        principalTable: "Tags",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fundraisings_UserId",
                table: "Fundraisings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FundraisingTag_TagsName",
                table: "FundraisingTag",
                column: "TagsName");

            migrationBuilder.CreateIndex(
                name: "IX_MonobankFundraisings_FundraisingId",
                table: "MonobankFundraisings",
                column: "FundraisingId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FundraisingTag");

            migrationBuilder.DropTable(
                name: "MonobankFundraisings");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Fundraisings");
        }
    }
}
