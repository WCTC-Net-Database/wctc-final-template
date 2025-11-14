namespace ConsoleRpgEntities.Models.Rooms
{
    public interface IRoom
    {
        int Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        int? NorthRoomId { get; set; }
        int? SouthRoomId { get; set; }
        int? EastRoomId { get; set; }
        int? WestRoomId { get; set; }
        int X { get; set; }
        int Y { get; set; }
    }
}
