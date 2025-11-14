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
            case "5":
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
}
