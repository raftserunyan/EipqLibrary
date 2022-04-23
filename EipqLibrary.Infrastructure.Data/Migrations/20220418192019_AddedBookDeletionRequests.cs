using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EipqLibrary.Infrastructure.Data.Migrations
{
    public partial class AddedBookDeletionRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_BookInstances_BookInstanceId",
                table: "Reservations");

            migrationBuilder.AlterColumn<int>(
                name: "BookInstanceId",
                table: "Reservations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "BookDeletionRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Count = table.Column<int>(type: "int", nullable: false),
                    DeletionReason = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemporarelyDeletedBorrowableBooksCount = table.Column<int>(type: "int", nullable: false),
                    RequestCreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountantActionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AccountantNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookDeletionRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookDeletionRequests_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookDeletionRequests_BookId",
                table: "BookDeletionRequests",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_BookInstances_BookInstanceId",
                table: "Reservations",
                column: "BookInstanceId",
                principalTable: "BookInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_BookInstances_BookInstanceId",
                table: "Reservations");

            migrationBuilder.DropTable(
                name: "BookDeletionRequests");

            migrationBuilder.AlterColumn<int>(
                name: "BookInstanceId",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_BookInstances_BookInstanceId",
                table: "Reservations",
                column: "BookInstanceId",
                principalTable: "BookInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
