# Final Exam Template - RPG Character Manager

This is a simplified template for the final exam. The solution provides a basic structure with Entity Framework Core, database migrations, and a console application that demonstrates CRUD operations.

## Project Structure

The solution consists of two projects:

### 1. ConsoleRpg (Console Application)
- **Program.cs**: Entry point that sets up dependency injection
- **Startup.cs**: Configures services including logging and database context
- **Services/GameEngine.cs**: Contains example CRUD operations (Add, Edit, Display, Search)
- **Helpers/MenuManager.cs**: Manages the main menu display
- **Helpers/OutputManager.cs**: Handles console output with buffering

### 2. ConsoleRpgEntities (Entity Framework Library)
- **Data/GameContext.cs**: EF Core DbContext with all DbSets
- **Models/**: Contains all entity models
  - **Characters/Player.cs**: Player entity
  - **Characters/Monsters/Monster.cs**: Abstract monster base class
  - **Characters/Monsters/Goblin.cs**: Concrete monster implementation
  - **Abilities/PlayerAbilities/Ability.cs**: Abstract ability base class
  - **Abilities/PlayerAbilities/ShoveAbility.cs**: Concrete ability implementation
  - **Equipments/Item.cs**: Item/Weapon/Armor entity
  - **Equipments/Equipment.cs**: Equipment entity linking weapons and armor
  - **Rooms/Room.cs**: Room entity (new)
- **Migrations/**: Database migrations

## Models Overview

### Player
- Id, Name, Health, Experience
- Foreign Keys: EquipmentId, RoomId
- Navigation Properties: Equipment, Room, Abilities, Inventory

### Monster (Abstract)
- Id, Name, Health, AggressionLevel
- Foreign Key: RoomId
- Navigation Property: Room
- Concrete implementation: Goblin (adds Sneakiness property)

### Ability (Abstract)
- Id, Name, Description
- Many-to-many relationship with Player
- Concrete implementation: ShoveAbility (adds Damage, Distance properties)

### Equipment
- Id, WeaponId, ArmorId
- Links to Item entities for weapons and armor

### Item
- Id, Name, Type, Attack, Defense

### Room (NEW)
- Id, Name, Description
- Navigation Properties: Players, Monsters

## Basic Required Functionality (Already Implemented)

The template includes these basic operations as examples:

1. **Add a new Character** - Prompts for Name, Health, Experience and saves to database
2. **Edit an existing Character** - Allows updating attributes
3. **Display all Characters** - Shows all characters with details
4. **Search for a Character by name** - Case-insensitive search
5. **Logging** - All operations are logged using Microsoft.Extensions.Logging

## Final Exam Requirements

### "C" Level (405/500 points)
**Required:** All basic features above

**Additional Requirements:**
1. Add Abilities to a Character
   - Allow users to add Abilities to existing Characters
   - Prompt for Ability details (Name, Attack Bonus, Defense Bonus, etc.)
   - Associate the Ability with the Character and save to database
   - Output confirmation to user

2. Display Character Abilities
   - For a selected Character, display all their Abilities
   - Include all ability properties in the output

3. Execute an ability during an attack
   - When attacking, ensure the ability is executed
   - Display appropriate output

### "B" Level (445/500 points)
**Required:** All basic and "C" level features

**Additional Requirements:**
1. Add new Room
   - Prompt for Room name, Description, and other properties
   - Optionally add a character/player to that room
   - Save Room to database
   - Output confirmation

2. Display details of a Room
   - Display all associated properties
   - Include list of inhabitants in the Room
   - Handle cases where Room has no Characters gracefully

3. Navigate the Rooms
   - Allow character to navigate through rooms
   - Display room details upon entering (name, description, inhabitants, etc.)
   - **Bonus:** Display a map using Spectre.Console or another tool

### "A" Level (475/500 points)
**Required:** All basic, "C", and "B" level features

**Additional Requirements (Admin Features):**
1. List characters in the room by selected attribute
   - Allow users to find Characters matching criteria (Health, Attack, Name, etc.)

2. List all Rooms with all characters in those rooms
   - Group Characters by their Room
   - Display in a formatted list

3. Find a specific piece of equipment and list the associated character and location
   - Allow user to specify the name of an item
   - Output: Character holding the item, Location of the character

## Database Configuration

**Connection String:** LocalDB (SQL Server)
```
Server=(localdb)\\mssqllocaldb;Database=GameDatabase;Trusted_Connection=True;
```

**Migrations:**
- Run migrations to create the database schema
- Existing migrations include initial setup, equipment, and rooms

## Logging

Logging is configured in `Startup.cs` with:
- Console logging (for development)
- File logging (stored in `Logs/log.txt`)

All user interactions are logged automatically.

## Getting Started

1. Ensure you have .NET 6.0 SDK installed
2. Update the database connection string in `appsettings.json` if needed
3. Run migrations: `dotnet ef database update --project ConsoleRpgEntities --startup-project ConsoleRpg`
4. Build the solution: `dotnet build`
5. Run the application: `dotnet run --project ConsoleRpg`

## Tips for Implementation

- Use LINQ queries for searching and filtering data
- Follow the existing pattern in GameEngine.cs for new menu options
- Use the OutputManager for consistent console output
- Log all user interactions using the injected ILogger
- Test your database operations thoroughly
- Handle edge cases (empty results, invalid input, etc.)

## Entity Framework Notes

- **Lazy Loading** is enabled - navigation properties load automatically
- **Many-to-Many** relationship between Player and Ability uses join table `PlayerAbilities`
- **Table-Per-Hierarchy (TPH)** is used for Monster and Ability inheritance
- **Foreign Keys** are nullable to allow flexibility

Good luck with your final exam!

