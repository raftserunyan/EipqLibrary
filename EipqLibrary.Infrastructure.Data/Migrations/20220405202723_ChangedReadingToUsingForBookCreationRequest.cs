using Microsoft.EntityFrameworkCore.Migrations;

namespace EipqLibrary.Infrastructure.Data.Migrations
{
    public partial class ChangedReadingToUsingForBookCreationRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AvailableForReadingInLibraryCount",
                table: "BookCreationRequests",
                newName: "AvailableForUsingInLibraryCount");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AvailableForUsingInLibraryCount",
                table: "BookCreationRequests",
                newName: "AvailableForReadingInLibraryCount");
        }
    }
}
