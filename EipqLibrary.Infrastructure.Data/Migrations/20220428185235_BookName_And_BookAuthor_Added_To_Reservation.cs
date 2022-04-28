using Microsoft.EntityFrameworkCore.Migrations;

namespace EipqLibrary.Infrastructure.Data.Migrations
{
    public partial class BookName_And_BookAuthor_Added_To_Reservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BookAuthor",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookName",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookAuthor",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "BookName",
                table: "Reservations");
        }
    }
}
