using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EipqLibrary.Infrastructure.Data.Migrations
{
    public partial class GroupGraduationYearReplacedWithGraduationDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationYear",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "GraduationYear",
                table: "Groups");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Groups",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "GraduationDate",
                table: "Groups",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "GraduationDate",
                table: "Groups");

            migrationBuilder.AddColumn<int>(
                name: "CreationYear",
                table: "Groups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GraduationYear",
                table: "Groups",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
