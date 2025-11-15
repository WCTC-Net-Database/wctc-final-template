using ConsoleRpgEntities.Models.Rooms;
using Spectre.Console;

namespace ConsoleRpg.Helpers;

public class MapManager
{
    /// <summary>
    /// Displays a visual map of the game world showing all rooms and their connections
    /// The current room is highlighted
    /// </summary>
    public void DisplayMap(IEnumerable<Room> allRooms, Room currentRoom)
    {
        var panel = new Panel(BuildMapGrid(allRooms, currentRoom))
        {
            Header = new PanelHeader(" [yellow]World Map[/] "),
            Border = BoxBorder.Double,
            BorderStyle = new Style(Color.Yellow)
        };

        AnsiConsole.Write(panel);
    }

    /// <summary>
    /// Builds a grid representation of the map
    /// This is a simple 3x3 grid that students can customize
    /// </summary>
    private Table BuildMapGrid(IEnumerable<Room> allRooms, Room currentRoom)
    {
        var table = new Table();
        table.Border(TableBorder.Square);
        table.HideHeaders();

        // Add 3 columns for 3x3 grid
        table.AddColumn(new TableColumn("").Width(20).Centered());
        table.AddColumn(new TableColumn("").Width(20).Centered());
        table.AddColumn(new TableColumn("").Width(20).Centered());

        var roomsList = allRooms.ToList();

        // Build rows for 3x3 grid (assuming IDs 1-9)
        // Row 1: Rooms 1, 2, 3
        table.AddRow(
            FormatRoomCell(roomsList.FirstOrDefault(r => r.Id == 1), currentRoom),
            FormatRoomCell(roomsList.FirstOrDefault(r => r.Id == 2), currentRoom),
            FormatRoomCell(roomsList.FirstOrDefault(r => r.Id == 3), currentRoom)
        );

        // Row 2: Rooms 4, 5, 6
        table.AddRow(
            FormatRoomCell(roomsList.FirstOrDefault(r => r.Id == 4), currentRoom),
            FormatRoomCell(roomsList.FirstOrDefault(r => r.Id == 5), currentRoom),
            FormatRoomCell(roomsList.FirstOrDefault(r => r.Id == 6), currentRoom)
        );

        // Row 3: Rooms 7, 8, 9
        table.AddRow(
            FormatRoomCell(roomsList.FirstOrDefault(r => r.Id == 7), currentRoom),
            FormatRoomCell(roomsList.FirstOrDefault(r => r.Id == 8), currentRoom),
            FormatRoomCell(roomsList.FirstOrDefault(r => r.Id == 9), currentRoom)
        );

        return table;
    }

    /// <summary>
    /// Formats a single room cell for the map
    /// Current room is highlighted
    /// </summary>
    private string FormatRoomCell(Room room, Room currentRoom)
    {
        if (room == null)
            return "[dim]???[/]";

        var isCurrentRoom = room.Id == currentRoom?.Id;
        var color = isCurrentRoom ? "green" : "cyan";
        var marker = isCurrentRoom ? ">>> " : "";

        return $"[{color}]{marker}{room.Name}[/]";
    }

    /// <summary>
    /// Displays the current room details including description, exits, and inhabitants
    /// </summary>
    public void DisplayRoomDetails(Room room)
    {
        AnsiConsole.WriteLine();

        // Room name and description
        var descriptionPanel = new Panel($"[white]{room.Description}[/]")
        {
            Header = new PanelHeader($" [bold yellow]{room.Name}[/] "),
            Border = BoxBorder.Rounded,
            BorderStyle = new Style(Color.Yellow)
        };
        AnsiConsole.Write(descriptionPanel);

        AnsiConsole.WriteLine();

        // Display exits
        DisplayExits(room);

        AnsiConsole.WriteLine();

        // Display inhabitants
        DisplayInhabitants(room);

        AnsiConsole.WriteLine();
    }

    /// <summary>
    /// Displays available exits from the current room
    /// </summary>
    private void DisplayExits(Room room)
    {
        var exitsPanel = new Panel(BuildExitsMarkup(room))
        {
            Header = new PanelHeader(" [cyan]Exits[/] "),
            Border = BoxBorder.Rounded,
            BorderStyle = new Style(Color.Cyan)
        };
        AnsiConsole.Write(exitsPanel);
    }

    /// <summary>
    /// Builds markup text showing available exits
    /// </summary>
    private string BuildExitsMarkup(Room room)
    {
        var exits = new List<string>();

        if (room.NorthRoomId.HasValue)
            exits.Add($"[green]North[/]: {room.NorthRoom?.Name ?? "Unknown"}");
        if (room.SouthRoomId.HasValue)
            exits.Add($"[green]South[/]: {room.SouthRoom?.Name ?? "Unknown"}");
        if (room.EastRoomId.HasValue)
            exits.Add($"[green]East[/]: {room.EastRoom?.Name ?? "Unknown"}");
        if (room.WestRoomId.HasValue)
            exits.Add($"[green]West[/]: {room.WestRoom?.Name ?? "Unknown"}");

        return exits.Count > 0
            ? string.Join("\n", exits)
            : "[dim]No exits available[/]";
    }

    /// <summary>
    /// Displays players and monsters in the current room
    /// </summary>
    private void DisplayInhabitants(Room room)
    {
        var markup = BuildInhabitantsMarkup(room);

        var inhabitantsPanel = new Panel(markup)
        {
            Header = new PanelHeader(" [magenta]Inhabitants[/] "),
            Border = BoxBorder.Rounded,
            BorderStyle = new Style(Color.Magenta)
        };
        AnsiConsole.Write(inhabitantsPanel);
    }

    /// <summary>
    /// Builds markup text showing room inhabitants
    /// </summary>
    private string BuildInhabitantsMarkup(Room room)
    {
        var lines = new List<string>();

        // Display players
        if (room.Players != null && room.Players.Any())
        {
            lines.Add("[yellow]Players:[/]");
            foreach (var player in room.Players)
            {
                lines.Add($"  • {player.Name} (Health: {player.Health}, Exp: {player.Experience})");
            }
        }

        // Display monsters
        if (room.Monsters != null && room.Monsters.Any())
        {
            if (lines.Count > 0) lines.Add("");
            lines.Add("[red]Monsters:[/]");
            foreach (var monster in room.Monsters)
            {
                lines.Add($"  • {monster.Name} (Health: {monster.Health}, Aggression: {monster.AggressionLevel})");
            }
        }

        return lines.Count > 0
            ? string.Join("\n", lines)
            : "[dim]The room is empty[/]";
    }

    /// <summary>
    /// Displays a simple menu of available actions based on the current room state
    /// </summary>
    public void DisplayAvailableActions(Room room, bool hasMonsters)
    {
        var actions = new List<string>
        {
            "[cyan]M[/] - View Map",
            "[cyan]I[/] - View Inventory",
            "[cyan]S[/] - View Stats"
        };

        // Add navigation options
        if (room.NorthRoomId.HasValue) actions.Add("[green]N[/] - Go North");
        if (room.SouthRoomId.HasValue) actions.Add("[green]S[/] - Go South");
        if (room.EastRoomId.HasValue) actions.Add("[green]E[/] - Go East");
        if (room.WestRoomId.HasValue) actions.Add("[green]W[/] - Go West");

        // Combat actions only available if monsters present
        if (hasMonsters)
        {
            actions.Add("[red]A[/] - Attack Monster");
            actions.Add("[yellow]B[/] - Use Ability");
        }

        // Admin/Menu actions
        actions.Add("[white]X[/] - Admin Menu");
        actions.Add("[dim]Q[/] - Quit");

        var actionsPanel = new Panel(string.Join(" | ", actions))
        {
            Header = new PanelHeader(" [white]Actions[/] "),
            Border = BoxBorder.Rounded
        };

        AnsiConsole.Write(actionsPanel);
        AnsiConsole.WriteLine();
    }
}
