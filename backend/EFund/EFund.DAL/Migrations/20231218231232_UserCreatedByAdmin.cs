using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFund.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UserCreatedByAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendId",
                table: "MonobankFundraisings");

            migrationBuilder.AddColumn<string>(
                name: "AvatarPath",
                table: "Fundraisings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                table: "Fundraisings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CreatedByAdmin",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "FundraisingReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FundraisingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FundraisingReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FundraisingReports_Fundraisings_FundraisingId",
                        column: x => x.FundraisingId,
                        principalTable: "Fundraisings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportAttachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FundraisingReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportAttachments_FundraisingReports_FundraisingReportId",
                        column: x => x.FundraisingReportId,
                        principalTable: "FundraisingReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FundraisingReports_FundraisingId",
                table: "FundraisingReports",
                column: "FundraisingId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportAttachments_FundraisingReportId",
                table: "ReportAttachments",
                column: "FundraisingReportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportAttachments");

            migrationBuilder.DropTable(
                name: "FundraisingReports");

            migrationBuilder.DropColumn(
                name: "AvatarPath",
                table: "Fundraisings");

            migrationBuilder.DropColumn(
                name: "IsClosed",
                table: "Fundraisings");

            migrationBuilder.DropColumn(
                name: "CreatedByAdmin",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "SendId",
                table: "MonobankFundraisings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
