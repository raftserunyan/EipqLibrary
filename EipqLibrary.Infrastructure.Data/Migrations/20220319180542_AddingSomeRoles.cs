using Microsoft.EntityFrameworkCore.Migrations;

namespace EipqLibrary.Infrastructure.Data.Migrations
{
    public partial class AddingSomeRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"insert into AspNetRoles(Id, [Name], NormalizedName)
                                   values (1, 'SuperAdmin', 'SUPERADMIN'),
	                                      (2, 'Accountant', 'ACCOUNTANT'),
	                                      (3, 'Librarian', 'LIBRARIAN')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"delete from AspNetRoles");
        }
    }
}
