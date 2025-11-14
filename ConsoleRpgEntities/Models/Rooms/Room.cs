using ConsoleRpgEntities.Models.Characters;

namespace ConsoleRpgEntities.Models.Rooms
{
    public class Room : IRoom
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Navigation properties
        public virtual ICollection<Player> Players { get; set; }
        public virtual ICollection<Monster> Monsters { get; set; }
    }
}
