# Final Exam Template - RPG Character Manager

This is a simplified template for the final exam. The solution provides a basic structure with Entity Framework Core, database migrations, and a console application that demonstrates CRUD operations.

## Project Structure

The solution consists of two projects:

### 1. ConsoleRpg (Console Application)
- **Program.cs**: Entry point that sets up dependency injection
- **Startup.cs**: Configures services including logging and database context
- **Services/GameEngine.cs**: Contains example CRUD operations and template methods for student implementation
- **Helpers/MenuManager.cs**: Manages the main menu display with all feature levels
- **Helpers/OutputManager.cs**: Handles console output with buffering

### 2. ConsoleRpgEntities (Entity Framework Library)
- **Data/GameContext.cs**: EF Core DbContext with all DbSets
- **Models/**: Contains all entity models
  - **Characters/Player.cs**: Player entity
  - **Characters/Monsters/Monster.cs**: Abstract monster base class (uses Table-Per-Hierarchy)
  - **Characters/Monsters/Goblin.cs**: Concrete monster implementation
  - **Abilities/PlayerAbilities/Ability.cs**: Abstract ability base class (uses Table-Per-Hierarchy)
  - **Abilities/PlayerAbilities/ShoveAbility.cs**: Concrete ability implementation
  - **Equipments/Item.cs**: Item/Weapon/Armor entity
  - **Equipments/Equipment.cs**: Equipment entity linking weapons and armor
  - **Rooms/Room.cs**: Room entity for B-level requirements
- **Migrations/**: Database migrations (pre-configured)

## Models Overview

### Player
- Properties: Id, Name, Health, Experience
- Foreign Keys: EquipmentId (nullable), RoomId (nullable)
- Navigation Properties: Equipment, Room, Abilities (many-to-many)
- Methods: Attack(ITargetable), UseAbility(IAbility, ITargetable)

### Monster (Abstract - uses TPH)
- Properties: Id, Name, Health, AggressionLevel, MonsterType (discriminator)
- Foreign Key: RoomId (nullable)
- Navigation Property: Room
- Concrete implementation: **Goblin** (adds Sneakiness property)
- Abstract Method: Attack(ITargetable)

### Ability (Abstract - uses TPH)
- Properties: Id, Name, Description, AbilityType (discriminator)
- Many-to-many relationship with Player via PlayerAbilities join table
- Concrete implementation: **ShoveAbility** (adds Damage and Distance properties)
- Abstract Method: Activate(IPlayer user, ITargetable target)

### Equipment
- Properties: Id, WeaponId (nullable FK to Item), ArmorId (nullable FK to Item)
- Navigation Properties: Weapon (Item), Armor (Item)
- Carefully configured to avoid cascade delete conflicts

### Item
- Properties: Id, Name, Type, Attack, Defense, Weight, Value
- Used for both weapons and armor

### Room
- Properties: Id, Name, Description
- Navigation Properties: Players (collection), Monsters (collection)
- One-to-many relationships with both Players and Monsters

## Exam Requirements & Implementation Guide

### ✅ Basic Required Functionality (ALREADY IMPLEMENTED)

These features are fully implemented as examples:

1. **Add a new Character** - Prompts for Name, Health, Experience and saves to database
2. **Edit an existing Character** - Allows updating attributes
3. **Display all Characters** - Shows all characters with details
4. **Search for a Character by name** - Case-insensitive search
5. **Logging** - All operations are logged using Microsoft.Extensions.Logging

### 📝 "C" Level Requirements (405/500 points)

**Required:** All basic features above

**Additional Features to Implement:**

1. **Add Abilities to a Character** (Template method: `AddAbilityToCharacter`)
   - Allow users to add Abilities to existing Characters
   - Prompt for Ability details (Name, Attack Bonus, Defense Bonus, etc.)
   - Associate the Ability with the Character using the many-to-many relationship
   - Save to database and output confirmation

2. **Display Character Abilities** (Template method: `DisplayCharacterAbilities`)
   - For a selected Character, display all their Abilities
   - Include all ability properties (Name, Description, Damage, Distance, etc.)
   - Handle characters with no abilities gracefully

3. **Execute an ability during an attack** (Template method: `AttackWithAbility`)
   - When attacking, execute the ability using the Activate method
   - Display appropriate output showing ability effects
   - Update health values in database

### 📝 "B" Level Requirements (445/500 points)

**Required:** All basic and "C" level features

**Additional Features to Implement:**

1. **Add new Room** (Template method: `AddRoom`)
   - Prompt for Room name, Description, and other properties
   - Optionally add a character/player to that room
   - Save Room to database and output confirmation

2. **Display details of a Room** (Template method: `DisplayRoomDetails`)
   - Display all associated properties of the room
   - Include list of inhabitants (Players and Monsters)
   - Handle cases where Room has no characters gracefully

3. **Navigate the Rooms** (Template method: `NavigateRooms`)
   - Allow character to navigate through rooms
   - Display room details upon entering (name, description, inhabitants)
   - Update character's RoomId in database
   - **BONUS:** Display a map using Spectre.Console or another tool

### 📝 "A" Level Requirements (475/500 points)

**Required:** All basic, "C", and "B" level features

**Additional Features to Implement (Admin-style features):**

1. **List characters in the room by selected attribute** (Template method: `ListCharactersInRoomByAttribute`)
   - Allow users to find Characters matching criteria (Health, Attack, Name, etc.)
   - Filter characters within a specific room
   - Display matching results

2. **List all Rooms with all characters in those rooms** (Template method: `ListAllRoomsWithCharacters`)
   - Group Characters by their Room
   - Display in a formatted list
   - Show room details and all inhabitants
   - Handle empty rooms gracefully

3. **Find a specific piece of equipment and list the associated character and location** (Template method: `FindEquipmentLocation`)
   - Allow user to specify the name of an item
   - Find which character has that equipment
   - Output: Character holding the item, Location (Room) of the character
   - Handle cases where equipment is not found or not equipped

## Database Configuration

**Database:** SQL Server LocalDB

**Connection String** (in `ConsoleRpgEntities/appsettings.json`):
```
Server=(localdb)\\mssqllocaldb;Database=GameDatabase;Trusted_Connection=True;
```

**Migrations:**
The database schema is already created through existing migrations:
- Initial schema creation (Players, Monsters, Abilities, PlayerAbilities join table)
- Equipment tables (Items, Equipment)
- Room tables and relationships
- Seed data for testing

## Getting Started

1. **Prerequisites:**
   - .NET 6.0 SDK or later
   - SQL Server LocalDB (usually included with Visual Studio)

2. **Setup Database:**
   ```bash
   # Navigate to project root
   cd /path/to/wctc-final-template

   # Apply migrations to create database
   dotnet ef database update --project ConsoleRpgEntities --startup-project ConsoleRpg
   ```

3. **Build the Solution:**
   ```bash
   dotnet build
   ```

4. **Run the Application:**
   ```bash
   dotnet run --project ConsoleRpg
   ```

## Implementation Tips

### General Guidelines
- Follow the existing code patterns in `GameEngine.cs`
- Use the `_outputManager` for all console output (supports colors)
- Always log user interactions using `_logger`
- Handle edge cases (null checks, empty results, invalid input)
- Save changes to database using `_context.SaveChanges()`

### Working with Entity Framework
- **Lazy Loading** is enabled - navigation properties load automatically
- For explicit loading, use `.Include()` in LINQ queries
- Example: `_context.Players.Include(p => p.Abilities).ToList()`
- Many-to-many relationship between Player and Ability is handled automatically

### LINQ Query Examples
```csharp
// Case-insensitive search
var players = _context.Players
    .Where(p => p.Name.ToLower().Contains(searchTerm.ToLower()))
    .ToList();

// Filter by attribute
var healthyPlayers = _context.Players
    .Where(p => p.Health > 50)
    .ToList();

// Include related data
var playerWithAbilities = _context.Players
    .Include(p => p.Abilities)
    .FirstOrDefault(p => p.Id == playerId);

// Group by room
var roomsWithPlayers = _context.Rooms
    .Include(r => r.Players)
    .Include(r => r.Monsters)
    .ToList();
```

### Working with Abilities (Many-to-Many)
```csharp
// Add ability to character
var player = _context.Players.Include(p => p.Abilities).FirstOrDefault(p => p.Id == playerId);
var ability = _context.Abilities.FirstOrDefault(a => a.Id == abilityId);

if (player != null && ability != null)
{
    player.Abilities.Add(ability);
    _context.SaveChanges();
}
```

### Using Interfaces
The template uses interfaces for polymorphism:
- `ITargetable` - Implemented by Player and Monster (for combat)
- `IPlayer` - Implemented by Player
- `IMonster` - Implemented by Monster and derived classes
- `IAbility` - Implemented by Ability and derived classes

## Logging

Logging is pre-configured in `Startup.cs`:
- **Console logging** for development
- **File logging** (stored in `Logs/log.txt`)
- Logs are automatically created for all user interactions

Simply use the injected `_logger` in your methods:
```csharp
_logger.LogInformation("User added character {Name}", characterName);
_logger.LogWarning("Character with Id {Id} not found", id);
_logger.LogError("Error saving to database: {Error}", ex.Message);
```

## Table-Per-Hierarchy (TPH) Explained

The template uses TPH for inheritance:

**Monster Hierarchy:**
- All monsters stored in single `Monsters` table
- `MonsterType` column determines the actual type
- Goblin adds `Sneakiness` column (nullable for other monster types)

**Ability Hierarchy:**
- All abilities stored in single `Abilities` table
- `AbilityType` column determines the actual type
- ShoveAbility adds `Damage` and `Distance` columns

**Creating New Types:**
```csharp
// Add to GameContext.OnModelCreating
modelBuilder.Entity<Monster>()
    .HasDiscriminator<string>(m => m.MonsterType)
    .HasValue<Goblin>("Goblin")
    .HasValue<Orc>("Orc");  // New type
```

## Common Challenges & Solutions

### Challenge: Player has no equipment/room
**Solution:** Foreign keys are nullable. Always check for null:
```csharp
if (player.Equipment?.Weapon != null)
{
    // Use weapon
}
```

### Challenge: Many-to-many relationships
**Solution:** EF Core 6.0 handles this automatically. Just add/remove from the collection:
```csharp
player.Abilities.Add(ability);  // Automatic join table management
```

### Challenge: Cascade delete errors
**Solution:** Equipment relationships use `DeleteBehavior.Restrict` to prevent conflicts

## Testing Your Implementation

1. **Test with valid data** - Happy path scenarios
2. **Test with invalid input** - Null checks, empty strings, invalid IDs
3. **Test edge cases** - Empty collections, missing relationships
4. **Test database operations** - Verify saves, updates, deletes work correctly
5. **Check logging** - Ensure all operations are logged

## Grading Reminders

- **Basic features** must work correctly
- **C-level:** Implement all 3 ability-related features
- **B-level:** Implement all 3 room-related features
- **A-level:** Implement all 3 advanced query features
- **Code quality:** Clean code, proper error handling, logging
- **Edge cases:** Handle null values, empty results gracefully

Good luck with your final exam!
