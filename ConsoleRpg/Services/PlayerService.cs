using ConsoleRpg.Helpers;
using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Rooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ConsoleRpg.Services;

/// <summary>
/// Handles all player-related actions and interactions
/// Separated from GameEngine to follow Single Responsibility Principle
/// </summary>
public class PlayerService
{
    private readonly GameContext _context;
    private readonly ExplorationUI _explorationUI;
    private readonly ILogger<PlayerService> _logger;

    public PlayerService(GameContext context, ExplorationUI explorationUI, ILogger<PlayerService> logger)
    {
        _context = context;
        _explorationUI = explorationUI;
        _logger = logger;
    }

    /// <summary>
    /// Move the player to a different room
    /// </summary>
    public Room MoveToRoom(Player player, Room currentRoom, int? roomId, string direction)
    {
        if (!roomId.HasValue)
        {
            _explorationUI.AddMessage($"[red]Cannot go {direction}[/]");
            _explorationUI.AddOutput($"[red]You cannot go {direction} from here - there is no exit in that direction.[/]");
            return currentRoom;
        }

        var newRoom = _context.Rooms
            .Include(r => r.Players)
            .Include(r => r.Monsters)
            .Include(r => r.NorthRoom)
            .Include(r => r.SouthRoom)
            .Include(r => r.EastRoom)
            .Include(r => r.WestRoom)
            .FirstOrDefault(r => r.Id == roomId.Value);

        if (newRoom == null)
        {
            _explorationUI.AddMessage($"[red]Room not found[/]");
            _explorationUI.AddOutput($"[red]Error: Room {roomId.Value} does not exist.[/]");
            return currentRoom;
        }

        // Update player's room
        player.RoomId = roomId.Value;
        _context.SaveChanges();

        _logger.LogInformation("Player {PlayerName} moved {Direction} to {RoomName}",
            player.Name, direction, newRoom.Name);

        _explorationUI.AddMessage($"[green]→ {direction}[/]");
        _explorationUI.AddOutput($"[green]You travel {direction} and arrive at {newRoom.Name}.[/]");

        return newRoom;
    }

    /// <summary>
    /// Show player character stats
    /// </summary>
    public void ShowCharacterStats(Player player)
    {
        _explorationUI.AddMessage($"[cyan]Viewing stats[/]");
        _explorationUI.AddOutput($"[yellow]Character:[/] {player.Name}\n" +
                                 $"[green]Health:[/] {player.Health}\n" +
                                 $"[cyan]Experience:[/] {player.Experience}");
    }

    /// <summary>
    /// Show player inventory and stats
    /// </summary>
    public void ShowInventory(Player player)
    {
        _explorationUI.AddMessage($"[magenta]Viewing inventory[/]");
        _explorationUI.AddOutput($"[magenta]Equipment:[/] {(player.Equipment != null ? "Equipped" : "None")}\n" +
                                 $"[blue]Abilities:[/] {player.Abilities?.Count ?? 0}");
    }

    /// <summary>
    /// TODO: Implement monster attack logic
    /// </summary>
    public void AttackMonster()
    {
        _explorationUI.AddMessage("[yellow]Attack (TODO)[/]");
        _explorationUI.AddOutput("[yellow]TODO: Implement attack logic - students will complete this feature.[/]");
        // Students will implement this
    }

    /// <summary>
    /// TODO: Implement ability usage logic
    /// </summary>
    public void UseAbilityOnMonster()
    {
        _explorationUI.AddMessage("[yellow]Ability (TODO)[/]");
        _explorationUI.AddOutput("[yellow]TODO: Implement ability usage - students will complete this feature.[/]");
        // Students will implement this
    }
}
