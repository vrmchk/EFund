using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFund.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddViolationRatingImpact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "RatingImpact",
                table: "Violations",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RatingImpact",
                table: "Violations");
        }
    }
}
