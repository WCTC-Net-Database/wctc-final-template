using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsoleRpgEntities.Migrations
{
    public partial class AddRoomModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create Rooms table
            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false),
                    NorthRoomId = table.Column<int>(type: "int", nullable: true),
                    SouthRoomId = table.Column<int>(type: "int", nullable: true),
                    EastRoomId = table.Column<int>(type: "int", nullable: true),
                    WestRoomId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Rooms_NorthRoomId",
                        column: x => x.NorthRoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rooms_Rooms_SouthRoomId",
                        column: x => x.SouthRoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rooms_Rooms_EastRoomId",
                        column: x => x.EastRoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rooms_Rooms_WestRoomId",
                        column: x => x.WestRoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            // Add RoomId column to Players table
            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Players",
                type: "int",
                nullable: true);

            // Add RoomId column to Monsters table
            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Monsters",
                type: "int",
                nullable: true);

            // Create index for Players.RoomId
            migrationBuilder.CreateIndex(
                name: "IX_Players_RoomId",
                table: "Players",
                column: "RoomId");

            // Create index for Monsters.RoomId
            migrationBuilder.CreateIndex(
                name: "IX_Monsters_RoomId",
                table: "Monsters",
                column: "RoomId");

            // Create indexes for Room exit foreign keys
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

            // Add foreign key constraint for Players -> Rooms
            migrationBuilder.AddForeignKey(
                name: "FK_Players_Rooms_RoomId",
                table: "Players",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");

            // Add foreign key constraint for Monsters -> Rooms
            migrationBuilder.AddForeignKey(
                name: "FK_Monsters_Rooms_RoomId",
                table: "Monsters",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key constraints
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Rooms_RoomId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Monsters_Rooms_RoomId",
                table: "Monsters");

            // Drop indexes
            migrationBuilder.DropIndex(
                name: "IX_Players_RoomId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Monsters_RoomId",
                table: "Monsters");

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
                name: "RoomId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Monsters");

            // Drop Rooms table
            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
