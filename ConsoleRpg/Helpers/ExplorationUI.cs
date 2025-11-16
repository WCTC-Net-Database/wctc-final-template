using ConsoleRpgEntities.Models.Rooms;
using Spectre.Console;

namespace ConsoleRpg.Helpers;

/// <summary>
/// Manages the exploration mode UI layout and rendering
/// Separated from GameEngine to follow Single Responsibility Principle
/// </summary>
public class ExplorationUI
{
    private readonly MapManager _mapManager;
    private readonly List<string> _messageLog = new List<string>();
    private const int MaxMessages = 8;

    public ExplorationUI(MapManager mapManager)
    {
        _mapManager = mapManager;
    }

    /// <summary>
    /// Renders the exploration mode UI and gets player selection
    /// </summary>
    /// <param name="allRooms">All rooms in the game world</param>
    /// <param name="currentRoom">The player's current room</param>
    /// <returns>The action selected by the player</returns>
    public string RenderAndGetAction(IEnumerable<Room> allRooms, Room currentRoom)
    {
        AnsiConsole.Clear();

        // Create the layout
        var layout = BuildLayout(allRooms, currentRoom);

        // Display the layout
        AnsiConsole.Write(layout);

        // Get available actions and let user select
        var actions = _mapManager.GetAvailableActions(currentRoom);

        var selectedAction = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[white]What would you like to do?[/]")
                .PageSize(12)
                .AddChoices(actions));

        return selectedAction;
    }

    /// <summary>
    /// Builds the UI layout with all panels
    /// Customize this method to experiment with different layouts
    /// </summary>
    private Layout BuildLayout(IEnumerable<Room> allRooms, Room currentRoom)
    {
        // LAYOUT STRUCTURE - Easy to modify and experiment with!
        // ┌──────────┬────────────┐
        // │          │ Room Info  │
        // │   Map    ├────────────┤
        // │          │ Messages   │
        // └──────────┴────────────┘

        var layout = new Layout("Root")
            .SplitColumns(
                new Layout("Map"),
                new Layout("Right")
            );

        // Split right side into rows (Room Details top, Messages bottom)
        layout["Right"].SplitRows(
            new Layout("RoomDetails"),
            new Layout("Messages")
        );

        // Configure panels
        layout["Map"].Update(_mapManager.GetCompactMapPanel(allRooms, currentRoom));
        layout["Right"]["RoomDetails"].Update(_mapManager.GetCompactRoomDetailsPanel(currentRoom));
        layout["Right"]["Messages"].Update(GetMessagePanel());

        return layout;
    }

    /// <summary>
    /// Adds a message to the scrolling message log
    /// </summary>
    public void AddMessage(string message)
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
            Border = BoxBorder.Rounded,
            Padding = new Padding(1, 0, 1, 0)
        };
    }

    /// <summary>
    /// Clears the message log
    /// </summary>
    public void ClearMessages()
    {
        _messageLog.Clear();
    }
}
