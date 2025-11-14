namespace ConsoleRpgEntities.Models.Rooms
{
    public interface IRoom
    {
        int Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
    }
}
