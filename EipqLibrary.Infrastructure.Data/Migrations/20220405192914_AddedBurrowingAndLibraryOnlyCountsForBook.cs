using Microsoft.EntityFrameworkCore.Migrations;

namespace EipqLibrary.Infrastructure.Data.Migrations
{
    public partial class AddedBurrowingAndLibraryOnlyCountsForBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BookAvailability",
                table: "BookCreationRequests",
                newName: "AvailableForReadingInLibraryCount");

            migrationBuilder.AddColumn<int>(
                name: "AvailableForUsingInLibraryCount",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AvailableForBorrowingCount",
                table: "BookCreationRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableForUsingInLibraryCount",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "AvailableForBorrowingCount",
                table: "BookCreationRequests");

            migrationBuilder.RenameColumn(
                name: "AvailableForReadingInLibraryCount",
                table: "BookCreationRequests",
                newName: "BookAvailability");
        }
    }
}
