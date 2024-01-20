using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFund.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddAttachmentName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ReportAttachments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "ReportAttachments");
        }
    }
}
