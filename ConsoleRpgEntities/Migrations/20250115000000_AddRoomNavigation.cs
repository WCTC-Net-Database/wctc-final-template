using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsoleRpgEntities.Migrations
{
    public partial class AddRoomNavigation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add directional navigation columns to Rooms table
            migrationBuilder.AddColumn<int>(
                name: "NorthRoomId",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SouthRoomId",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EastRoomId",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WestRoomId",
                table: "Rooms",
                type: "int",
                nullable: true);

            // Create indexes for the navigation foreign keys
            migrationBuilder.CreateIndex(
                name: "IX_Rooms_NorthRoomId",
                table: "Rooms",
                column: "NorthRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_SouthRoomId",
                table: "Rooms",
                column: "SouthRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_EastRoomId",
                table: "Rooms",
                column: "EastRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_WestRoomId",
                table: "Rooms",
                column: "WestRoomId");

            // Add foreign key constraints
            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Rooms_NorthRoomId",
                table: "Rooms",
                column: "NorthRoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Rooms_SouthRoomId",
                table: "Rooms",
                column: "SouthRoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Rooms_EastRoomId",
                table: "Rooms",
                column: "EastRoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Rooms_WestRoomId",
                table: "Rooms",
                column: "WestRoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key constraints
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Rooms_NorthRoomId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Rooms_SouthRoomId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Rooms_EastRoomId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Rooms_WestRoomId",
                table: "Rooms");

            // Drop indexes
            migrationBuilder.DropIndex(
                name: "IX_Rooms_NorthRoomId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_SouthRoomId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_EastRoomId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_WestRoomId",
                table: "Rooms");

            // Drop columns
            migrationBuilder.DropColumn(
                name: "NorthRoomId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "SouthRoomId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "EastRoomId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "WestRoomId",
                table: "Rooms");
        }
    }
}
