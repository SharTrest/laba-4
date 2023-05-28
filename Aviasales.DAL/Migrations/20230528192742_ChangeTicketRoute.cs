using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aviasales.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTicketRoute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "Tickets");

            migrationBuilder.AddColumn<string>(
                name: "Date",
                table: "Routes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Time",
                table: "Routes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "Routes");

            migrationBuilder.AddColumn<string>(
                name: "Date",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Time",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
