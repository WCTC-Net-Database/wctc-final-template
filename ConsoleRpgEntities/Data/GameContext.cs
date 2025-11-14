using ConsoleRpgEntities.Models.Abilities.PlayerAbilities;
using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Characters.Monsters;
using ConsoleRpgEntities.Models.Equipments;
using ConsoleRpgEntities.Models.Rooms;
using Microsoft.EntityFrameworkCore;

namespace ConsoleRpgEntities.Data
{
    public class GameContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Monster> Monsters { get; set; }
        public DbSet<Ability> Abilities { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Room> Rooms { get; set; }

        public GameContext(DbContextOptions<GameContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure TPH for Character hierarchy
            modelBuilder.Entity<Monster>()
                .HasDiscriminator<string>(m=> m.MonsterType)
                .HasValue<Goblin>("Goblin");

            // Configure TPH for Ability hierarchy
            modelBuilder.Entity<Ability>()
                .HasDiscriminator<string>(pa=>pa.AbilityType)
                .HasValue<ShoveAbility>("ShoveAbility");

            // Configure many-to-many relationship
            modelBuilder.Entity<Player>()
                .HasMany(p => p.Abilities)
                .WithMany(a => a.Players)
                .UsingEntity(j => j.ToTable("PlayerAbilities"));

            // Configure Room relationships with Players and Monsters
            modelBuilder.Entity<Room>()
                .HasMany(r => r.Players)
                .WithOne(p => p.Room)
                .HasForeignKey(p => p.RoomId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<Room>()
                .HasMany(r => r.Monsters)
                .WithOne(m => m.Room)
                .HasForeignKey(m => m.RoomId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            // Configure Room self-referencing relationships for exits
            // These relationships allow rooms to connect to other rooms via North/South/East/West
            modelBuilder.Entity<Room>()
                .HasOne(r => r.NorthRoom)
                .WithMany()
                .HasForeignKey(r => r.NorthRoomId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<Room>()
                .HasOne(r => r.SouthRoom)
                .WithMany()
                .HasForeignKey(r => r.SouthRoomId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<Room>()
                .HasOne(r => r.EastRoom)
                .WithMany()
                .HasForeignKey(r => r.EastRoomId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<Room>()
                .HasOne(r => r.WestRoom)
                .WithMany()
                .HasForeignKey(r => r.WestRoomId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            // Call the separate configuration method to set up Equipment entity relationships
            ConfigureEquipmentRelationships(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureEquipmentRelationships(ModelBuilder modelBuilder)
        {
            // Configuring the Equipment entity to handle relationships with Item entities (Weapon and Armor)
            // without causing multiple cascade paths in SQL Server.

            // Equipment has a nullable foreign key WeaponId, pointing to the Item entity.
            // Setting DeleteBehavior.Restrict ensures that deleting an Item (Weapon) 
            // will NOT cascade delete any Equipment rows that reference it.
            // This prevents conflicts that arise with multiple cascading paths.
            modelBuilder.Entity<Equipment>()
                .HasOne(e => e.Weapon)  // Define the relationship to the Weapon item
                .WithMany()             // Equipment doesn't need to navigate back to Item
                .HasForeignKey(e => e.WeaponId)  // Specifies the foreign key column in Equipment
                //.OnDelete(DeleteBehavior.Restrict)  // Prevents cascading deletes, avoids multiple paths
                .IsRequired(false);

            // Similar configuration for ArmorId, also pointing to the Item entity.
            // Here we are using DeleteBehavior.Restrict for the Armor foreign key relationship as well.
            // The goal is to avoid cascade paths from both WeaponId and ArmorId foreign keys.
            modelBuilder.Entity<Equipment>()
                .HasOne(e => e.Armor)  // Define the relationship to the Armor item
                .WithMany()            // No need for reverse navigation back to Equipment
                .HasForeignKey(e => e.ArmorId)  // Sets ArmorId as the foreign key in Equipment
                //.OnDelete(DeleteBehavior.Restrict)  // Prevents cascading deletes to avoid conflict
                .IsRequired(false);

            // Explanation of Why DeleteBehavior.Restrict:
            // Cascade paths occur when there are multiple relationships in one table pointing to another,
            // each with cascading delete behavior. SQL Server restricts such configurations to prevent 
            // accidental recursive deletions. Here, by setting DeleteBehavior.Restrict, deleting an Item
            // (Weapon or Armor) will simply nullify the WeaponId or ArmorId in Equipment rather than 
            // cascading a delete through multiple paths.
        }
    }
}


