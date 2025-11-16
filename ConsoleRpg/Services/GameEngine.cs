using ConsoleRpg.Helpers;
using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Rooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace ConsoleRpg.Services;

public class GameEngine
{
    private readonly GameContext _context;
    private readonly MenuManager _menuManager;
    private readonly OutputManager _outputManager;
    private readonly MapManager _mapManager;
    private readonly ILogger<GameEngine> _logger;

    private Player _currentPlayer;
    private Room _currentRoom;
    private GameMode _currentMode = GameMode.Exploration;
    private List<string> _messageLog = new List<string>();
    private const int MaxMessages = 8;

    public GameEngine(GameContext context, MenuManager menuManager, OutputManager outputManager, MapManager mapManager, ILogger<GameEngine> logger)
    {
        _context = context;
        _menuManager = menuManager;
        _outputManager = outputManager;
        _mapManager = mapManager;
        _logger = logger;
    }

    public void Run()
    {
        _logger.LogInformation("Game engine started");

        // Initialize game - get or create first player
        InitializeGame();

        // Main game loop
        while (true)
        {
            if (_currentMode == GameMode.Exploration)
            {
                ExplorationMode();
            }
            else
            {
                AdminMode();
            }
        }
    }

    /// <summary>
    /// Initialize the game by getting the first player or creating one
    /// </summary>
    private void InitializeGame()
    {
        // Try to get the first player
        _currentPlayer = _context.Players
            .Include(p => p.Room)
            .Include(p => p.Equipment)
            .Include(p => p.Abilities)
            .FirstOrDefault();

        if (_currentPlayer == null)
        {
            AnsiConsole.MarkupLine("[yellow]No players found! Please create a character first.[/]");
            _currentMode = GameMode.Admin;
            return;
        }

        // Get current room or default to first room
        _currentRoom = _currentPlayer.Room ?? _context.Rooms.Include(r => r.Players).Include(r => r.Monsters).FirstOrDefault();

        if (_currentRoom == null)
        {
            AnsiConsole.MarkupLine("[red]No rooms found! Database may not be properly seeded.[/]");
            _currentMode = GameMode.Admin;
            return;
        }

        _logger.LogInformation("Game initialized with player {PlayerName} in room {RoomName}",
            _currentPlayer.Name, _currentRoom.Name);
    }

    #region Exploration Mode

    /// <summary>
    /// Main exploration mode - this is where the player navigates the world
    /// </summary>
    private void ExplorationMode()
    {
        AnsiConsole.Clear();

        // Reload room with all related data
        _currentRoom = _context.Rooms
            .Include(r => r.Players)
            .Include(r => r.Monsters)
            .Include(r => r.NorthRoom)
            .Include(r => r.SouthRoom)
            .Include(r => r.EastRoom)
            .Include(r => r.WestRoom)
            .FirstOrDefault(r => r.Id == _currentRoom.Id);

        // Get all rooms for map
        var allRooms = _context.Rooms.ToList();
        bool hasMonsters = _currentRoom.Monsters != null && _currentRoom.Monsters.Any();

        // Create layout: Left (Map + Actions) | Right (Room Details + Messages)
        var layout = new Layout("Root")
            .SplitColumns(
                new Layout("Left"),
                new Layout("Right")
            );

        // Split left side into rows (Map top, Actions bottom)
        layout["Left"].SplitRows(
            new Layout("Map").Ratio(2),
            new Layout("Actions").Ratio(3)
        );

        // Split right side into rows (Room Details top, Messages bottom)
        layout["Right"].SplitRows(
            new Layout("RoomDetails").Ratio(3),
            new Layout("Messages").Ratio(2)
        );

        // Configure left side
        layout["Left"]["Map"].Update(_mapManager.GetCompactMapPanel(allRooms, _currentRoom));

        // Configure right side
        layout["Right"]["RoomDetails"].Update(_mapManager.GetCompactRoomDetailsPanel(_currentRoom));
        layout["Right"]["Messages"].Update(GetMessagePanel());

        // Display the layout
        AnsiConsole.Write(layout);
        AnsiConsole.WriteLine();

        // Get available actions and let user select
        var actions = _mapManager.GetAvailableActions(_currentRoom);

        var selectedAction = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[white]What would you like to do?[/]")
                .PageSize(12)
                .AddChoices(actions));

        HandleExplorationAction(selectedAction, hasMonsters);
    }

    /// <summary>
    /// Adds a message to the scrolling message log
    /// </summary>
    private void AddMessage(string message)
    {
        _messageLog.Add($"[dim]{DateTime.Now:HH:mm:ss}[/] {message}");

        // Keep only the last N messages
        if (_messageLog.Count > MaxMessages)
        {
            _messageLog.RemoveAt(0);
        }
    }

    /// <summary>
    /// Gets the message panel for display
    /// </summary>
    private Panel GetMessagePanel()
    {
        var content = _messageLog.Any()
            ? string.Join("\n", _messageLog)
            : "[dim]Messages will appear here...[/]";

        return new Panel(content)
        {
            Header = new PanelHeader("[yellow]Messages[/]"),
            Border = BoxBorder.Rounded
        };
    }

    /// <summary>
    /// Handles player action selection during exploration mode
    /// </summary>
    private void HandleExplorationAction(string action, bool hasMonsters)
    {
        switch (action)
        {
            case "Go North":
                MoveToRoom(_currentRoom.NorthRoomId, "North");
                break;
            case "Go South":
                MoveToRoom(_currentRoom.SouthRoomId, "South");
                break;
            case "Go East":
                MoveToRoom(_currentRoom.EastRoomId, "East");
                break;
            case "Go West":
                MoveToRoom(_currentRoom.WestRoomId, "West");
                break;
            case "View Map":
                AddMessage("[cyan]Viewing map...[/]");
                break;
            case "View Inventory":
                ShowInventory();
                break;
            case "View Character Stats":
                ShowCharacterStats();
                break;
            case "Attack Monster":
                AttackMonster();
                break;
            case "Use Ability":
                UseAbilityOnMonster();
                break;
            case "Return to Main Menu":
                _currentMode = GameMode.Admin;
                AddMessage("[yellow]Switched to Admin Mode[/]");
                break;
            default:
                AddMessage($"[red]Unknown action: {action}[/]");
                break;
        }
    }

    /// <summary>
    /// Move the player to a different room
    /// </summary>
    private void MoveToRoom(int? roomId, string direction)
    {
        if (!roomId.HasValue)
        {
            AddMessage($"[red]Cannot go {direction} - no exit![/]");
            return;
        }

        _currentRoom = _context.Rooms
            .Include(r => r.Players)
            .Include(r => r.Monsters)
            .Include(r => r.NorthRoom)
            .Include(r => r.SouthRoom)
            .Include(r => r.EastRoom)
            .Include(r => r.WestRoom)
            .FirstOrDefault(r => r.Id == roomId.Value);

        // Update player's room
        _currentPlayer.RoomId = roomId.Value;
        _context.SaveChanges();

        _logger.LogInformation("Player {PlayerName} moved {Direction} to {RoomName}",
            _currentPlayer.Name, direction, _currentRoom.Name);

        AddMessage($"[green]Traveled {direction} to {_currentRoom.Name}[/]");
    }

    /// <summary>
    /// Show player character stats
    /// </summary>
    private void ShowCharacterStats()
    {
        AddMessage($"[yellow]{_currentPlayer.Name}[/] - HP:[green]{_currentPlayer.Health}[/] XP:[cyan]{_currentPlayer.Experience}[/]");
    }

    /// <summary>
    /// Show player inventory and stats
    /// </summary>
    private void ShowInventory()
    {
        AddMessage($"[magenta]Inventory:[/] Equipment: {(_currentPlayer.Equipment != null ? "Equipped" : "None")}, Abilities: {_currentPlayer.Abilities?.Count ?? 0}");
    }

    /// <summary>
    /// TODO: Implement monster attack logic
    /// </summary>
    private void AttackMonster()
    {
        AddMessage("[yellow]TODO: Implement attack logic - students will complete this[/]");
        // Students will implement this
    }

    /// <summary>
    /// TODO: Implement ability usage logic
    /// </summary>
    private void UseAbilityOnMonster()
    {
        AddMessage("[yellow]TODO: Implement ability usage - students will complete this[/]");
        // Students will implement this
    }

    #endregion

    #region Admin Mode

    /// <summary>
    /// Admin mode - provides access to CRUD operations and template methods
    /// </summary>
    private void AdminMode()
    {
        _menuManager.ShowMainMenu(AdminMenuChoice);
    }

    private void AdminMenuChoice(string choice)
    {
        switch (choice?.ToUpper())
        {
            // World Exploration / Return to Exploration Mode
            case "E":
            case "0":
                ExploreWorld();
                break;

            // Basic Features
            case "1":
                AddCharacter();
                break;
            case "2":
                EditCharacter();
                break;
            case "3":
                DisplayAllCharacters();
                break;
            case "4":
                SearchCharacterByName();
                break;

            // C-Level Features
            case "5":
                AddAbilityToCharacter();
                break;
            case "6":
                DisplayCharacterAbilities();
                break;
            case "7":
                // Attack with ability - redirect to exploration mode
                AnsiConsole.MarkupLine("[yellow]Please use this feature in Exploration Mode[/]");
                PressAnyKey();
                break;

            // B-Level Features
            case "8":
                AddRoom();
                break;
            case "9":
                DisplayRoomDetails();
                break;
            case "10":
                // Navigate rooms - redirect to exploration mode
                AnsiConsole.MarkupLine("[yellow]Please use this feature in Exploration Mode[/]");
                PressAnyKey();
                break;

            // A-Level Features
            case "11":
                ListCharactersInRoomByAttribute();
                break;
            case "12":
                ListAllRoomsWithCharacters();
                break;
            case "13":
                FindEquipmentLocation();
                break;

            default:
                AnsiConsole.MarkupLine("[red]Invalid selection.[/]");
                PressAnyKey();
                break;
        }
    }

    #endregion

    #region Basic CRUD Operations
    #region World Exploration

    private void ExploreWorld()
    {
        _logger.LogInformation("User selected Explore World - switching to Exploration Mode");

        // Simply switch to exploration mode
        _currentMode = GameMode.Exploration;
        AddMessage("[green]Welcome to the world! Use keyboard commands to navigate.[/]");
    }

    #endregion

    private void AddCharacter()
    {
        _logger.LogInformation("User selected Add Character");
        AnsiConsole.MarkupLine("[yellow]=== Add New Character ===[/]");

        var name = AnsiConsole.Ask<string>("Enter character [green]name[/]:");
        var health = AnsiConsole.Ask<int>("Enter [green]health[/]:");
        var experience = AnsiConsole.Ask<int>("Enter [green]experience[/]:");

        var player = new Player
        {
            Name = name,
            Health = health,
            Experience = experience
        };

        _context.Players.Add(player);
        _context.SaveChanges();

        _logger.LogInformation("Character {Name} added to database with Id {Id}", name, player.Id);
        AnsiConsole.MarkupLine($"[green]Character '{name}' added successfully![/]");
        Thread.Sleep(1000);
    }

    private void EditCharacter()
    {
        _logger.LogInformation("User selected Edit Character");
        AnsiConsole.MarkupLine("[yellow]=== Edit Character ===[/]");

        var id = AnsiConsole.Ask<int>("Enter character [green]ID[/] to edit:");

        var player = _context.Players.Find(id);
        if (player == null)
        {
            _logger.LogWarning("Character with Id {Id} not found", id);
            AnsiConsole.MarkupLine($"[red]Character with ID {id} not found.[/]");
            PressAnyKey();
            return;
        }

        AnsiConsole.MarkupLine($"Editing: [cyan]{player.Name}[/]");

        if (AnsiConsole.Confirm("Update name?"))
        {
            player.Name = AnsiConsole.Ask<string>("Enter new [green]name[/]:");
        }

        if (AnsiConsole.Confirm("Update health?"))
        {
            player.Health = AnsiConsole.Ask<int>("Enter new [green]health[/]:");
        }

        if (AnsiConsole.Confirm("Update experience?"))
        {
            player.Experience = AnsiConsole.Ask<int>("Enter new [green]experience[/]:");
        }

        _context.SaveChanges();

        _logger.LogInformation("Character {Name} (Id: {Id}) updated", player.Name, player.Id);
        AnsiConsole.MarkupLine($"[green]Character '{player.Name}' updated successfully![/]");
        Thread.Sleep(1000);
    }

    private void DisplayAllCharacters()
    {
        _logger.LogInformation("User selected Display All Characters");
        AnsiConsole.MarkupLine("[yellow]=== All Characters ===[/]");

        var players = _context.Players.Include(p => p.Room).ToList();

        if (!players.Any())
        {
            AnsiConsole.MarkupLine("[red]No characters found.[/]");
        }
        else
        {
            var table = new Table();
            table.AddColumn("ID");
            table.AddColumn("Name");
            table.AddColumn("Health");
            table.AddColumn("Experience");
            table.AddColumn("Location");

            foreach (var player in players)
            {
                table.AddRow(
                    player.Id.ToString(),
                    player.Name,
                    player.Health.ToString(),
                    player.Experience.ToString(),
                    player.Room?.Name ?? "Unknown"
                );
            }

            AnsiConsole.Write(table);
        }

        PressAnyKey();
    }

    private void SearchCharacterByName()
    {
        _logger.LogInformation("User selected Search Character");
        AnsiConsole.MarkupLine("[yellow]=== Search Character ===[/]");

        var searchName = AnsiConsole.Ask<string>("Enter character [green]name[/] to search:");

        var players = _context.Players
            .Include(p => p.Room)
            .Where(p => p.Name.ToLower().Contains(searchName.ToLower()))
            .ToList();

        if (!players.Any())
        {
            _logger.LogInformation("No characters found matching '{SearchName}'", searchName);
            AnsiConsole.MarkupLine($"[red]No characters found matching '{searchName}'.[/]");
        }
        else
        {
            _logger.LogInformation("Found {Count} character(s) matching '{SearchName}'", players.Count, searchName);

            var table = new Table();
            table.AddColumn("ID");
            table.AddColumn("Name");
            table.AddColumn("Health");
            table.AddColumn("Experience");
            table.AddColumn("Location");

            foreach (var player in players)
            {
                table.AddRow(
                    player.Id.ToString(),
                    player.Name,
                    player.Health.ToString(),
                    player.Experience.ToString(),
                    player.Room?.Name ?? "Unknown"
                );
            }

            AnsiConsole.Write(table);
        }

        PressAnyKey();
    }

    #endregion

    #region C-Level Requirements

    // TODO: Implement this method
    // Requirements:
    // - Display a list of existing characters
    // - Prompt user to select a character (by ID)
    // - Display a list of available abilities from the database
    // - Prompt user to select an ability to add
    // - Associate the ability with the character using the many-to-many relationship
    // - Save changes to the database
    // - Display confirmation message with the character name and ability name
    // - Log the operation
    private void AddAbilityToCharacter()
    {
        _logger.LogInformation("User selected Add Ability to Character");
        AnsiConsole.MarkupLine("[yellow]=== Add Ability to Character ===[/]");

        // TODO: Implement this method
        AnsiConsole.MarkupLine("[red]This feature is not yet implemented.[/]");
        AnsiConsole.MarkupLine("[yellow]TODO: Allow users to add abilities to existing characters.[/]");

        PressAnyKey();
    }

    // TODO: Implement this method
    // Requirements:
    // - Prompt the user to select a character (by ID or name)
    // - Retrieve the character and their abilities from the database (use Include or lazy loading)
    // - Display the character's name and basic info
    // - Display all abilities associated with that character in a formatted table
    // - For each ability, show: Name, Description, and any other relevant properties (e.g., Damage, Distance for ShoveAbility)
    // - Handle the case where the character has no abilities
    // - Log the operation
    private void DisplayCharacterAbilities()
    {
        _logger.LogInformation("User selected Display Character Abilities");
        AnsiConsole.MarkupLine("[yellow]=== Display Character Abilities ===[/]");

        // TODO: Implement this method
        AnsiConsole.MarkupLine("[red]This feature is not yet implemented.[/]");
        AnsiConsole.MarkupLine("[yellow]TODO: Display all abilities for a selected character.[/]");

        PressAnyKey();
    }

    #endregion

    #region B-Level Requirements

    // TODO: Implement this method
    // Requirements:
    // - Prompt user for room name
    // - Prompt user for room description
    // - Optionally prompt for navigation (which rooms connect in which directions)
    // - Create a new Room entity
    // - Save to the database
    // - Display confirmation with room details
    // - Log the operation
    private void AddRoom()
    {
        _logger.LogInformation("User selected Add Room");
        AnsiConsole.MarkupLine("[yellow]=== Add New Room ===[/]");

        // TODO: Implement this method
        AnsiConsole.MarkupLine("[red]This feature is not yet implemented.[/]");
        AnsiConsole.MarkupLine("[yellow]TODO: Allow users to create new rooms and connect them to the world.[/]");

        PressAnyKey();
    }

    // TODO: Implement this method
    // Requirements:
    // - Display a list of all rooms
    // - Prompt user to select a room (by ID or name)
    // - Retrieve room from database with related data (Include Players and Monsters)
    // - Display room name, description, and exits
    // - Display list of all players in the room (or message if none)
    // - Display list of all monsters in the room (or message if none)
    // - Handle case where room is empty gracefully
    // - Log the operation
    private void DisplayRoomDetails()
    {
        _logger.LogInformation("User selected Display Room Details");
        AnsiConsole.MarkupLine("[yellow]=== Display Room Details ===[/]");

        // TODO: Implement this method
        AnsiConsole.MarkupLine("[red]This feature is not yet implemented.[/]");
        AnsiConsole.MarkupLine("[yellow]TODO: Display detailed information about a room and its inhabitants.[/]");

        PressAnyKey();
    }

    #endregion

    #region A-Level Requirements

    // TODO: Implement this method
    // Requirements:
    // - Display list of all rooms
    // - Prompt user to select a room
    // - Display a menu of attributes to filter by (Health, Name, Experience, etc.)
    // - Prompt user for filter criteria
    // - Query the database for characters in that room matching the criteria
    // - Display matching characters with relevant details in a formatted table
    // - Handle case where no characters match
    // - Log the operation
    private void ListCharactersInRoomByAttribute()
    {
        _logger.LogInformation("User selected List Characters in Room by Attribute");
        AnsiConsole.MarkupLine("[yellow]=== List Characters in Room by Attribute ===[/]");

        // TODO: Implement this method
        AnsiConsole.MarkupLine("[red]This feature is not yet implemented.[/]");
        AnsiConsole.MarkupLine("[yellow]TODO: Find characters in a room matching specific criteria.[/]");

        PressAnyKey();
    }

    // TODO: Implement this method
    // Requirements:
    // - Query database for all rooms
    // - For each room, retrieve all characters (Players) in that room
    // - Display in a formatted list grouped by room
    // - Show room name and description
    // - Under each room, list all characters with their details
    // - Handle rooms with no characters gracefully
    // - Consider using Spectre.Console panels or tables for nice formatting
    // - Log the operation
    private void ListAllRoomsWithCharacters()
    {
        _logger.LogInformation("User selected List All Rooms with Characters");
        AnsiConsole.MarkupLine("[yellow]=== List All Rooms with Characters ===[/]");

        // TODO: Implement this method
        AnsiConsole.MarkupLine("[red]This feature is not yet implemented.[/]");
        AnsiConsole.MarkupLine("[yellow]TODO: Group and display all characters by their rooms.[/]");

        PressAnyKey();
    }

    // TODO: Implement this method
    // Requirements:
    // - Prompt user for equipment/item name to search for
    // - Query the database to find which character has this equipment
    // - Use Include to load Equipment -> Weapon/Armor -> Item
    // - Also load the character's Room information
    // - Display the character's name who has the equipment
    // - Display the room/location where the character is located
    // - Handle case where equipment is not found
    // - Handle case where equipment exists but isn't equipped by anyone
    // - Use Spectre.Console for nice formatting
    // - Log the operation
    private void FindEquipmentLocation()
    {
        _logger.LogInformation("User selected Find Equipment Location");
        AnsiConsole.MarkupLine("[yellow]=== Find Equipment Location ===[/]");

        // TODO: Implement this method
        AnsiConsole.MarkupLine("[red]This feature is not yet implemented.[/]");
        AnsiConsole.MarkupLine("[yellow]TODO: Find which character has specific equipment and where they are located.[/]");

        PressAnyKey();
    }

    #endregion

    #region Helper Methods

    private void PressAnyKey()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Markup("[dim]Press any key to continue...[/]");
        Console.ReadKey(true);
    }

    #endregion
}

/// <summary>
/// Represents the current game mode
/// </summary>
public enum GameMode
{
    Exploration,
    Admin
}
