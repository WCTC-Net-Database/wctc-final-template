# Final Exam Template - RPG World Builder

This template provides a **world-building RPG framework** where students create an immersive game experience with navigable rooms, character interactions, and visual map displays.

## Project Structure

The solution consists of two projects:

### 1. ConsoleRpg (Console Application)
- **Program.cs**: Entry point that sets up dependency injection
- **Startup.cs**: Configures services including logging, database context, and helpers
- **Services/GameEngine.cs**: Core game loop with dual modes (Exploration and Admin)
- **Helpers/MapManager.cs**: Visualizes the world map and room details using Spectre.Console
- **Helpers/MenuManager.cs**: Admin/Developer menu for CRUD operations
- **Helpers/OutputManager.cs**: Console output buffering

### 2. ConsoleRpgEntities (Entity Framework Library)
- **Data/GameContext.cs**: EF Core DbContext with all DbSets and relationships
- **Models/**: All entity models
  - **Characters/Player.cs**: Player entity with navigation
  - **Characters/Monsters/Monster.cs**: Abstract monster (TPH)
  - **Characters/Monsters/Goblin.cs**: Concrete monster implementation
  - **Abilities/PlayerAbilities/Ability.cs**: Abstract ability (TPH)
  - **Abilities/PlayerAbilities/ShoveAbility.cs**: Concrete ability
  - **Equipments/Item.cs**: Weapons and armor
  - **Equipments/Equipment.cs**: Equipment linking
  - **Rooms/Room.cs**: **Rooms with directional navigation (N, S, E, W)**
- **Migrations/**: Pre-configured database migrations and seed data

## Game Architecture

### Dual-Mode Design

The game operates in **two distinct modes**:

#### 1. **Exploration Mode** (Default)
This is where the game "comes alive":
- Visual map display showing all rooms (3x3 grid by default)
- Current room highlighted
- Room description and atmosphere
- Available exits (North, South, East, West)
- Monsters and other players in the current room
- Context-sensitive actions (can only attack if monsters are present)
- Navigation between rooms
- Real-time world interaction

#### 2. **Admin/Developer Mode**
Accessed by pressing 'X' in Exploration Mode:
- CRUD operations for characters
- Template methods for exam requirements
- Database management
- Room creation and configuration
- Advanced queries and reporting

## Models Overview

### Player
- **Properties**: Id, Name, Health, Experience
- **Foreign Keys**: EquipmentId, RoomId
- **Navigation**: Equipment, Room, Abilities (many-to-many)
- **Methods**: Attack(ITargetable), UseAbility(IAbility, ITargetable)

### Room **★ Key Component for World Building**
- **Properties**: Id, Name, Description
- **Directional Navigation**:
  - NorthRoomId, SouthRoomId, EastRoomId, WestRoomId (nullable FKs)
  - Self-referencing relationships
- **Navigation Properties**:
  - NorthRoom, SouthRoom, EastRoom, WestRoom (Room references)
  - Players (collection of players in this room)
  - Monsters (collection of monsters in this room)

### Monster (Abstract - TPH)
- **Properties**: Id, Name, Health, AggressionLevel, MonsterType (discriminator)
- **Foreign Key**: RoomId
- **Concrete Implementation**: Goblin (adds Sneakiness)

### Ability (Abstract - TPH)
- **Properties**: Id, Name, Description, AbilityType (discriminator)
- **Many-to-many**: Linked to Players via PlayerAbilities join table
- **Concrete Implementation**: ShoveAbility (adds Damage, Distance)

### Equipment & Item
- **Equipment**: Links weapons and armor to players
- **Item**: Actual weapon/armor data (Attack, Defense, Weight, Value)

## World Map Visualization

The template includes **Spectre.Console** for rich terminal UI:

### MapManager Features:
1. **Visual World Map**: 3x3 grid showing all rooms
2. **Current Location Highlight**: Player's position marked in green
3. **Room Details Panel**: Description, exits, inhabitants
4. **Context-Sensitive Actions**: Different options based on room state

### Example Map Display:
```
╔═══════════════════════════ World Map ═══════════════════════════╗
║  Town Square      Dark Forest       Ancient Temple             ║
║  Mountain Path    >>> Crystal Cave  Abandoned Mine             ║
║  Riverside Camp   Goblin Lair       Wizard Tower               ║
╚═════════════════════════════════════════════════════════════════╝

┌─ Crystal Cave ───────────────────────────────────────────────┐
│ A sparkling cave filled with luminescent crystals that cast  │
│ an eerie blue glow.                                          │
└──────────────────────────────────────────────────────────────┘

┌─ Exits ──────────────────────────────────────────────────────┐
│ North: Dark Forest                                           │
│ South: Goblin Lair                                           │
│ East: Abandoned Mine                                         │
│ West: Mountain Path                                          │
└──────────────────────────────────────────────────────────────┘

┌─ Inhabitants ────────────────────────────────────────────────┐
│ Monsters:                                                     │
│   • Goblin Scout (Health: 50, Aggression: 3)                │
└──────────────────────────────────────────────────────────────┘

┌─ Actions ────────────────────────────────────────────────────┐
│ M - View Map | I - View Stats | N - Go North | S - Go South │
│ E - Go East | W - Go West | A - Attack Monster | X - Admin  │
└──────────────────────────────────────────────────────────────┘
```

## Database Configuration

**Database**: SQL Server LocalDB
**Connection String** (in `ConsoleRpgEntities/appsettings.json`):
```
Server=(localdb)\\mssqllocaldb;Database=GameDatabase;Trusted_Connection=True;
```

**Migrations** (Pre-configured):
1. `InitialCreate` - Creates base tables
2. `InitialSeedData` - Seeds players, monsters, abilities
3. `AddEquipment` - Adds equipment tables
4. `SeedEquipment` - Seeds weapons and armor
5. `AddRoomModel` - Creates Room table with Player/Monster relationships
6. `AddRoomNavigation` - Adds directional navigation (N, S, E, W)
7. `SeedRooms` - **Seeds a 3x3 world grid with connected rooms**

## Getting Started

### 1. Prerequisites
- .NET 6.0 SDK or later
- SQL Server LocalDB
- Visual Studio or VS Code

### 2. Setup Database
```bash
# Navigate to project root
cd /path/to/wctc-final-template

# Apply all migrations and seed data
dotnet ef database update --project ConsoleRpgEntities --startup-project ConsoleRpg
```

### 3. Build and Run
```bash
# Build the solution
dotnet build

# Run the game
dotnet run --project ConsoleRpg
```

### 4. First Run Experience
When you run the application:
1. You'll start in **Exploration Mode** in the Town Square
2. The map will show all 9 rooms in a 3x3 grid
3. You can navigate using N, S, E, W commands
4. Press **X** to access the Admin Menu for CRUD operations
5. Press **0** in Admin Menu to return to Exploration

## Exam Requirements & Implementation Guide

### ✅ Basic Required Functionality (ALREADY IMPLEMENTED)

The following are working examples:
1. **Add a new Character** - Admin Menu Option 1
2. **Edit an existing Character** - Admin Menu Option 2
3. **Display all Characters** - Admin Menu Option 3 (now with Room location!)
4. **Search for a Character by name** - Admin Menu Option 4
5. **Logging** - All operations logged automatically

### 📝 "C" Level Requirements (405/500 points)

**Required:** All basic features above

**Features to Implement:**

1. **Add Abilities to a Character** (Admin Menu Option 5)
   - Display list of existing characters
   - Display available abilities from database
   - Associate ability with character (many-to-many)
   - Save and confirm
   - Use Spectre.Console for nice tables

2. **Display Character Abilities** (Admin Menu Option 6)
   - Select a character
   - Retrieve with `.Include(p => p.Abilities)`
   - Display character info and all abilities
   - Show ability properties (Name, Description, Damage, Distance)
   - Handle characters with no abilities

3. **Execute an ability during an attack** (Exploration Mode)
   - Implement `AttackMonster()` in GameEngine.cs:230
   - Implement `UseAbilityOnMonster()` in GameEngine.cs:239
   - Only available when monsters are in the room
   - Apply damage using `ability.Activate(player, monster)`
   - Update health in database
   - Display results using Spectre.Console

### 📝 "B" Level Requirements (445/500 points)

**Required:** All basic and "C" level features

**Features to Implement:**

1. **Add new Room** (Admin Menu Option 8)
   - Prompt for name and description
   - Optionally set directional connections (NorthRoomId, etc.)
   - Create Room entity
   - Save to database
   - Students can expand the 3x3 grid!

2. **Display details of a Room** (Admin Menu Option 9)
   - List all rooms
   - Allow selection
   - Use `.Include(r => r.Players).Include(r => r.Monsters)`
   - Show room properties, exits, and inhabitants
   - Can reuse MapManager methods!

3. **Navigate the Rooms** (**ALREADY IMPLEMENTED** in Exploration Mode!)
   - Character navigation via N, S, E, W is working
   - Updates player's RoomId automatically
   - **BONUS COMPLETED**: Map visualization using Spectre.Console!
   - Students can enhance with additional features

### 📝 "A" Level Requirements (475/500 points)

**Required:** All basic, "C", and "B" level features

**Advanced Features (Admin-style):**

1. **List characters in room by attribute** (Admin Menu Option 11)
   - Display list of rooms
   - Select a room
   - Choose filter attribute (Health, Experience, Name)
   - Apply LINQ filter: `Where(p => p.RoomId == roomId && p.Health > value)`
   - Display matching characters in Spectre.Console table

2. **List all Rooms with characters** (Admin Menu Option 12)
   - Query all rooms with `.Include(r => r.Players)`
   - Group and display by room
   - Use Spectre.Console panels for each room
   - Handle empty rooms gracefully

3. **Find equipment location** (Admin Menu Option 13)
   - Prompt for item name
   - Query: `_context.Players.Include(p => p.Equipment).ThenInclude(e => e.Weapon).Include(p => p.Room)`
   - Find character with matching weapon/armor
   - Display character name and room location
   - Handle not found cases

## World-Building Philosophy

### This is NOT Just a CRUD Application

The exam evaluates your ability to create an **immersive game world**. Students should:

✅ **Expand the world**: Add more rooms beyond the 3x3 grid
✅ **Create atmosphere**: Write compelling room descriptions
✅ **Design encounters**: Place monsters strategically
✅ **Build connections**: Create interesting navigation paths
✅ **Add depth**: Implement meaningful ability interactions
✅ **Polish presentation**: Use Spectre.Console for rich UI

### Evaluation Criteria

The instructor will assess:
1. **Functionality**: Do all required features work correctly?
2. **World Design**: Is the world interesting and well-connected?
3. **Code Quality**: Clean, well-organized, properly commented?
4. **Error Handling**: Graceful handling of edge cases?
5. **User Experience**: Is the game enjoyable to play?
6. **Creativity**: Did you go beyond the minimum requirements?

## Implementation Tips

### Working with Room Navigation

```csharp
// Create a new room with exits
var room = new Room
{
    Name = "Haunted Crypt",
    Description = "A dark crypt filled with ancient tombs...",
    NorthRoomId = 5,  // Connects to Crystal Cave (room 5)
    WestRoomId = 8    // Connects to Goblin Lair (room 8)
};
_context.Rooms.Add(room);
_context.SaveChanges();
```

### Using Spectre.Console

```csharp
// Create a nice table
var table = new Table();
table.AddColumn("Name");
table.AddColumn("Health");
table.AddColumn("Location");

foreach (var player in players)
{
    table.AddRow(player.Name, player.Health.ToString(), player.Room?.Name ?? "Unknown");
}

AnsiConsole.Write(table);
```

### LINQ Queries for Advanced Features

```csharp
// Filter characters in a room by health
var healthyPlayers = _context.Players
    .Where(p => p.RoomId == roomId && p.Health > 50)
    .ToList();

// Group rooms with characters
var roomsWithPlayers = _context.Rooms
    .Include(r => r.Players)
    .Where(r => r.Players.Any())
    .ToList();

// Find equipment location
var playerWithSword = _context.Players
    .Include(p => p.Equipment)
        .ThenInclude(e => e.Weapon)
    .Include(p => p.Room)
    .FirstOrDefault(p => p.Equipment.Weapon.Name.Contains("Sword"));
```

### MapManager Usage

The MapManager is already integrated! You can use its methods:

```csharp
// In your custom methods, you can call:
_mapManager.DisplayMap(allRooms, currentRoom);
_mapManager.DisplayRoomDetails(room);
_mapManager.DisplayAvailableActions(room, hasMonsters);
```

## Extending the Template

### Adding New Room Types
Students can enhance Room.cs with additional properties:
```csharp
public bool IsLocked { get; set; }
public string RequiredKey { get; set; }
public int DangerLevel { get; set; }
```

### Creating New Monster Types
Follow the TPH pattern:
```csharp
public class Orc : Monster
{
    public int Strength { get; set; }

    public override void Attack(ITargetable target)
    {
        // Orc-specific attack logic
    }
}

// Update GameContext.OnModelCreating
modelBuilder.Entity<Monster>()
    .HasValue<Goblin>("Goblin")
    .HasValue<Orc>("Orc");
```

### Custom Map Layouts
The MapManager's `BuildMapGrid()` method uses a 3x3 layout. Students can:
- Modify the grid size (4x4, 5x5, etc.)
- Create non-grid layouts
- Add special room indicators (locked, dangerous, etc.)

## Common Challenges & Solutions

### Challenge: Room navigation not updating
**Solution**: Ensure you're calling `_context.SaveChanges()` after updating `player.RoomId`

### Challenge: Monsters not appearing in room
**Solution**: Use `.Include(r => r.Monsters)` when loading the room

### Challenge: Map not displaying correctly
**Solution**: Check that room IDs match the expected grid positions (1-9)

### Challenge: Null reference when accessing room properties
**Solution**: Always check for null: `room?.NorthRoom?.Name ?? "No exit"`

## Testing Your Implementation

1. **Test navigation**: Can you move between all rooms?
2. **Test combat**: Do abilities work correctly on monsters?
3. **Test edge cases**: What happens in empty rooms? Invalid input?
4. **Test CRUD**: Do all admin operations work and persist?
5. **Test world coherence**: Do room connections make sense?

## Logging

All operations are automatically logged to:
- **Console**: Real-time logging during development
- **File**: `Logs/log.txt` for persistent logging

Use logging throughout your implementation:
```csharp
_logger.LogInformation("Player {Name} entered {Room}", player.Name, room.Name);
_logger.LogWarning("No monsters found in room {RoomId}", roomId);
_logger.LogError("Failed to load room: {Error}", ex.Message);
```

## Resources

- **Spectre.Console Docs**: https://spectreconsole.net/
- **EF Core Include/ThenInclude**: https://learn.microsoft.com/en-us/ef/core/querying/related-data/
- **LINQ Query Syntax**: https://learn.microsoft.com/en-us/dotnet/csharp/linq/

## Final Tips

🎯 **Start with the basics**: Make sure CRUD operations work first
🎯 **Test incrementally**: Don't write all features at once
🎯 **Use the debugger**: Step through your code to understand data flow
🎯 **Read the logs**: They'll help you track what's happening
🎯 **Be creative**: The template is a starting point, make it yours!
🎯 **Ask questions**: If requirements are unclear, ask the instructor

## Submission Checklist

Before submitting your exam:

- [ ] All basic CRUD operations work
- [ ] Character abilities can be added and displayed
- [ ] Combat system with abilities is implemented
- [ ] Rooms can be created and navigated
- [ ] Advanced queries return correct results
- [ ] World design is coherent and interesting
- [ ] Code is clean and well-commented
- [ ] Error handling is implemented
- [ ] Logging is used throughout
- [ ] README is updated with your world design notes

Good luck creating your RPG world!
