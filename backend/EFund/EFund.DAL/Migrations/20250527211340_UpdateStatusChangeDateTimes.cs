using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFund.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStatusChangeDateTimes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FinisedAt",
                table: "Fundraisings",
                newName: "ReviewedAt");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ReadyForReviewAt",
                table: "Fundraisings",
                type: "datetimeoffset",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReadyForReviewAt",
                table: "Fundraisings");

            migrationBuilder.RenameColumn(
                name: "ReviewedAt",
                table: "Fundraisings",
                newName: "FinisedAt");
        }
    }
}
