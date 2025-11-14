using ConsoleRpg.Helpers;
using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models.Characters;
using Microsoft.Extensions.Logging;

namespace ConsoleRpg.Services;

public class GameEngine
{
    private readonly GameContext _context;
    private readonly MenuManager _menuManager;
    private readonly OutputManager _outputManager;
    private readonly ILogger<GameEngine> _logger;

    public GameEngine(GameContext context, MenuManager menuManager, OutputManager outputManager, ILogger<GameEngine> logger)
    {
        _menuManager = menuManager;
        _outputManager = outputManager;
        _context = context;
        _logger = logger;
    }

    public void Run()
    {
        _logger.LogInformation("Game engine started");
        _menuManager.ShowMainMenu(MainMenuChoice);
    }

    private void MainMenuChoice(string choice)
    {
        switch (choice)
        {
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
                AttackWithAbility();
                break;

            // B-Level Features
            case "8":
                AddRoom();
                break;
            case "9":
                DisplayRoomDetails();
                break;
            case "10":
                NavigateRooms();
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

            case "0":
                _logger.LogInformation("User exited the application");
                _outputManager.WriteLine("Exiting game...", ConsoleColor.Red);
                _outputManager.Display();
                Environment.Exit(0);
                break;
            default:
                _outputManager.WriteLine("Invalid selection.", ConsoleColor.Red);
                _outputManager.Display();
                break;
        }

        // Show menu again after action completes
        _menuManager.ShowMainMenu(MainMenuChoice);
    }

    private void AddCharacter()
    {
        _logger.LogInformation("User selected Add Character");
        _outputManager.WriteLine("=== Add New Character ===", ConsoleColor.Yellow);

        _outputManager.WriteLine("Enter character name:");
        _outputManager.Display();
        var name = Console.ReadLine();

        _outputManager.WriteLine("Enter health:");
        _outputManager.Display();
        if (!int.TryParse(Console.ReadLine(), out int health))
        {
            _outputManager.WriteLine("Invalid health value.", ConsoleColor.Red);
            _outputManager.Display();
            return;
        }

        _outputManager.WriteLine("Enter experience:");
        _outputManager.Display();
        if (!int.TryParse(Console.ReadLine(), out int experience))
        {
            _outputManager.WriteLine("Invalid experience value.", ConsoleColor.Red);
            _outputManager.Display();
            return;
        }

        var player = new Player
        {
            Name = name,
            Health = health,
            Experience = experience
        };

        _context.Players.Add(player);
        _context.SaveChanges();

        _logger.LogInformation("Character {Name} added to database with Id {Id}", name, player.Id);
        _outputManager.WriteLine($"Character '{name}' added successfully!", ConsoleColor.Green);
        _outputManager.Display();
        Thread.Sleep(1000);
    }

    private void EditCharacter()
    {
        _logger.LogInformation("User selected Edit Character");
        _outputManager.WriteLine("=== Edit Character ===", ConsoleColor.Yellow);

        _outputManager.WriteLine("Enter character ID to edit:");
        _outputManager.Display();
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            _outputManager.WriteLine("Invalid ID.", ConsoleColor.Red);
            _outputManager.Display();
            return;
        }

        var player = _context.Players.Find(id);
        if (player == null)
        {
            _logger.LogWarning("Character with Id {Id} not found", id);
            _outputManager.WriteLine($"Character with ID {id} not found.", ConsoleColor.Red);
            _outputManager.Display();
            return;
        }

        _outputManager.WriteLine($"Editing: {player.Name}");
        _outputManager.WriteLine("Enter new name (or press Enter to keep current):");
        _outputManager.Display();
        var name = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(name))
        {
            player.Name = name;
        }

        _outputManager.WriteLine("Enter new health (or press Enter to keep current):");
        _outputManager.Display();
        var healthInput = Console.ReadLine();
        if (int.TryParse(healthInput, out int health))
        {
            player.Health = health;
        }

        _outputManager.WriteLine("Enter new experience (or press Enter to keep current):");
        _outputManager.Display();
        var expInput = Console.ReadLine();
        if (int.TryParse(expInput, out int experience))
        {
            player.Experience = experience;
        }

        _context.SaveChanges();

        _logger.LogInformation("Character {Name} (Id: {Id}) updated", player.Name, player.Id);
        _outputManager.WriteLine($"Character '{player.Name}' updated successfully!", ConsoleColor.Green);
        _outputManager.Display();
        Thread.Sleep(1000);
    }

    private void DisplayAllCharacters()
    {
        _logger.LogInformation("User selected Display All Characters");
        _outputManager.WriteLine("=== All Characters ===", ConsoleColor.Yellow);

        var players = _context.Players.ToList();

        if (!players.Any())
        {
            _outputManager.WriteLine("No characters found.", ConsoleColor.Red);
        }
        else
        {
            foreach (var player in players)
            {
                _outputManager.WriteLine($"ID: {player.Id} | Name: {player.Name} | Health: {player.Health} | Experience: {player.Experience}", ConsoleColor.Cyan);
            }
        }

        _outputManager.Display();
        _outputManager.WriteLine("\nPress any key to continue...");
        _outputManager.Display();
        Console.ReadKey();
    }

    private void SearchCharacterByName()
    {
        _logger.LogInformation("User selected Search Character");
        _outputManager.WriteLine("=== Search Character ===", ConsoleColor.Yellow);

        _outputManager.WriteLine("Enter character name to search:");
        _outputManager.Display();
        var searchName = Console.ReadLine();

        var players = _context.Players
            .Where(p => p.Name.ToLower().Contains(searchName.ToLower()))
            .ToList();

        if (!players.Any())
        {
            _logger.LogInformation("No characters found matching '{SearchName}'", searchName);
            _outputManager.WriteLine($"No characters found matching '{searchName}'.", ConsoleColor.Red);
        }
        else
        {
            _logger.LogInformation("Found {Count} character(s) matching '{SearchName}'", players.Count, searchName);
            foreach (var player in players)
            {
                _outputManager.WriteLine($"ID: {player.Id} | Name: {player.Name} | Health: {player.Health} | Experience: {player.Experience}", ConsoleColor.Cyan);
            }
        }

        _outputManager.Display();
        _outputManager.WriteLine("\nPress any key to continue...");
        _outputManager.Display();
        Console.ReadKey();
    }

    #region C-Level Requirements

    // TODO: Implement this method
    // Requirements:
    // - Prompt the user to select an existing character (by ID or name)
    // - Display a list of available abilities (you may need to create abilities first or use existing ones)
    // - Prompt user to select an ability to add
    // - Associate the ability with the character using the many-to-many relationship
    // - Save changes to the database
    // - Display confirmation message with the character name and ability name
    // - Log the operation
    private void AddAbilityToCharacter()
    {
        _logger.LogInformation("User selected Add Ability to Character");
        _outputManager.WriteLine("=== Add Ability to Character ===", ConsoleColor.Yellow);

        // TODO: Implement this method
        _outputManager.WriteLine("This feature is not yet implemented.", ConsoleColor.Red);
        _outputManager.WriteLine("TODO: Allow users to add abilities to existing characters.", ConsoleColor.Yellow);

        _outputManager.Display();
        Thread.Sleep(2000);
    }

    // TODO: Implement this method
    // Requirements:
    // - Prompt the user to select a character (by ID or name)
    // - Retrieve the character and their abilities from the database (use Include or lazy loading)
    // - Display the character's name and basic info
    // - Display all abilities associated with that character
    // - For each ability, show: Name, Description, and any other relevant properties (e.g., Damage, Distance for ShoveAbility)
    // - Handle the case where the character has no abilities
    // - Log the operation
    private void DisplayCharacterAbilities()
    {
        _logger.LogInformation("User selected Display Character Abilities");
        _outputManager.WriteLine("=== Display Character Abilities ===", ConsoleColor.Yellow);

        // TODO: Implement this method
        _outputManager.WriteLine("This feature is not yet implemented.", ConsoleColor.Red);
        _outputManager.WriteLine("TODO: Display all abilities for a selected character.", ConsoleColor.Yellow);

        _outputManager.Display();
        Thread.Sleep(2000);
    }

    // TODO: Implement this method
    // Requirements:
    // - Prompt user to select an attacking character
    // - Prompt user to select a target (another character or monster)
    // - Display available abilities for the attacker
    // - Prompt user to select an ability to use
    // - Execute the ability's Activate method (this should apply damage/effects)
    // - Display the results of the attack (damage dealt, effects applied, etc.)
    // - Update health values in the database
    // - Log the operation
    private void AttackWithAbility()
    {
        _logger.LogInformation("User selected Attack with Ability");
        _outputManager.WriteLine("=== Attack with Ability ===", ConsoleColor.Yellow);

        // TODO: Implement this method
        _outputManager.WriteLine("This feature is not yet implemented.", ConsoleColor.Red);
        _outputManager.WriteLine("TODO: Allow character to attack using an ability.", ConsoleColor.Yellow);

        _outputManager.Display();
        Thread.Sleep(2000);
    }

    #endregion

    #region B-Level Requirements

    // TODO: Implement this method
    // Requirements:
    // - Prompt user for room name
    // - Prompt user for room description
    // - Optionally prompt to add a character/monster to the room (by ID)
    // - Create a new Room entity
    // - Save to the database
    // - Display confirmation with room details
    // - Log the operation
    private void AddRoom()
    {
        _logger.LogInformation("User selected Add Room");
        _outputManager.WriteLine("=== Add New Room ===", ConsoleColor.Yellow);

        // TODO: Implement this method
        _outputManager.WriteLine("This feature is not yet implemented.", ConsoleColor.Red);
        _outputManager.WriteLine("TODO: Allow users to create new rooms.", ConsoleColor.Yellow);

        _outputManager.Display();
        Thread.Sleep(2000);
    }

    // TODO: Implement this method
    // Requirements:
    // - Prompt user to select a room (by ID or name)
    // - Retrieve room from database with related data (Include Players and Monsters)
    // - Display room name and description
    // - Display list of all players in the room (or message if none)
    // - Display list of all monsters in the room (or message if none)
    // - Handle case where room is empty gracefully
    // - Log the operation
    private void DisplayRoomDetails()
    {
        _logger.LogInformation("User selected Display Room Details");
        _outputManager.WriteLine("=== Display Room Details ===", ConsoleColor.Yellow);

        // TODO: Implement this method
        _outputManager.WriteLine("This feature is not yet implemented.", ConsoleColor.Red);
        _outputManager.WriteLine("TODO: Display detailed information about a room and its inhabitants.", ConsoleColor.Yellow);

        _outputManager.Display();
        Thread.Sleep(2000);
    }

    // TODO: Implement this method
    // Requirements:
    // - Display list of all available rooms
    // - Prompt user to select a character that will navigate
    // - Allow user to move character between rooms
    // - Update character's RoomId in the database
    // - Display room details upon entering (name, description, inhabitants)
    // - Provide option to move to another room or exit navigation
    // - BONUS: Use Spectre.Console or another library to display a map
    // - Log the operation
    private void NavigateRooms()
    {
        _logger.LogInformation("User selected Navigate Rooms");
        _outputManager.WriteLine("=== Navigate Rooms ===", ConsoleColor.Yellow);

        // TODO: Implement this method
        _outputManager.WriteLine("This feature is not yet implemented.", ConsoleColor.Red);
        _outputManager.WriteLine("TODO: Allow characters to move between rooms.", ConsoleColor.Yellow);
        _outputManager.WriteLine("BONUS: Consider adding a map visualization using Spectre.Console.", ConsoleColor.Magenta);

        _outputManager.Display();
        Thread.Sleep(2000);
    }

    #endregion

    #region A-Level Requirements

    // TODO: Implement this method
    // Requirements:
    // - Prompt user to select a room
    // - Prompt user to select an attribute to search by (Health, Attack/Equipment, Name, etc.)
    // - Prompt user for search criteria (e.g., "Health > 50", "Name contains 'Bob'")
    // - Query the database for characters in that room matching the criteria
    // - Display matching characters with relevant details
    // - Handle case where no characters match
    // - Log the operation
    private void ListCharactersInRoomByAttribute()
    {
        _logger.LogInformation("User selected List Characters in Room by Attribute");
        _outputManager.WriteLine("=== List Characters in Room by Attribute ===", ConsoleColor.Yellow);

        // TODO: Implement this method
        _outputManager.WriteLine("This feature is not yet implemented.", ConsoleColor.Red);
        _outputManager.WriteLine("TODO: Find characters in a room matching specific criteria.", ConsoleColor.Yellow);

        _outputManager.Display();
        Thread.Sleep(2000);
    }

    // TODO: Implement this method
    // Requirements:
    // - Query database for all rooms
    // - For each room, retrieve all characters (Players) in that room
    // - Display in a formatted list grouped by room
    // - Show room name and description
    // - Under each room, list all characters with their details
    // - Handle rooms with no characters gracefully
    // - Consider using a formatted table or grouped display
    // - Log the operation
    private void ListAllRoomsWithCharacters()
    {
        _logger.LogInformation("User selected List All Rooms with Characters");
        _outputManager.WriteLine("=== List All Rooms with Characters ===", ConsoleColor.Yellow);

        // TODO: Implement this method
        _outputManager.WriteLine("This feature is not yet implemented.", ConsoleColor.Red);
        _outputManager.WriteLine("TODO: Group and display all characters by their rooms.", ConsoleColor.Yellow);

        _outputManager.Display();
        Thread.Sleep(2000);
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
    // - Log the operation
    private void FindEquipmentLocation()
    {
        _logger.LogInformation("User selected Find Equipment Location");
        _outputManager.WriteLine("=== Find Equipment Location ===", ConsoleColor.Yellow);

        // TODO: Implement this method
        _outputManager.WriteLine("This feature is not yet implemented.", ConsoleColor.Red);
        _outputManager.WriteLine("TODO: Find which character has specific equipment and where they are located.", ConsoleColor.Yellow);

        _outputManager.Display();
        Thread.Sleep(2000);
    }

    #endregion
}
