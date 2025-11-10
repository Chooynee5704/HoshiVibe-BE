using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoshiVibe.Migrations
{
    /// <inheritdoc />
    public partial class updateAtributtes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateRange",
                table: "Zodiacs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DateRange",
                table: "Zodiacs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
