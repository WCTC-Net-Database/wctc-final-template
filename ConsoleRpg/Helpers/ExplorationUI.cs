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
    private readonly List<string> _outputLog = new List<string>();
    private const int MaxMessages = 8;
    private const int MaxOutputLines = 15;

    public ExplorationUI(MapManager mapManager)
    {
        _mapManager = mapManager;
    }

    public string RenderAndGetAction(IEnumerable<Room> allRooms, Room currentRoom)
    {
        AnsiConsole.Clear();

        // Render map panel
        var mapPanel = _mapManager.GetCompactMapPanel(allRooms, currentRoom);
        AnsiConsole.Write(mapPanel);

        // Render room details panel
        var roomPanel = _mapManager.GetCompactRoomDetailsPanel(currentRoom);
        AnsiConsole.Write(roomPanel);

        AnsiConsole.WriteLine();

        // Render output log (if any)
        if (_outputLog.Any())
        {
            foreach (var line in _outputLog)
                AnsiConsole.MarkupLine(line);
            _outputLog.Clear();
        }

        // Get available actions
        var actions = _mapManager.GetAvailableActions(currentRoom);

        // Present actions as a numbered list below the panels
        for (int i = 0; i < actions.Count; i++)
        {
            AnsiConsole.MarkupLine($"[cyan]{i + 1}[/]. {actions[i]}");
        }
        int choice = AnsiConsole.Ask<int>("[white]Enter the number of your action:[/]", 1);

        // Clamp choice to valid range
        if (choice < 1 || choice > actions.Count)
            choice = 1;

        return actions[choice - 1];
    }



    private Layout BuildLayout(IEnumerable<Room> allRooms, Room currentRoom)
    {
        var layout = new Layout("Root")
            .SplitColumns(
                new Layout("Left").Size(50), // Adjust width as needed
                new Layout("Right").Ratio(1)
            );

        // Only map on the left, only room details on the right
        layout["Left"].Update(_mapManager.GetCompactMapPanel(allRooms, currentRoom));
        layout["Right"].Update(_mapManager.GetCompactRoomDetailsPanel(currentRoom));

        return layout;
    }

    private Panel GetActionsPanel(List<string> actions)
    {
        var sb = new System.Text.StringBuilder();
        for (int i = 0; i < actions.Count; i++)
        {
            sb.AppendLine($"[cyan]{i + 1}[/]. {actions[i]}");
        }
        return new Panel(sb.ToString().TrimEnd())
        {
            Header = new PanelHeader("[green]Available Actions[/]"),
            Border = BoxBorder.Rounded,
            Padding = new Padding(1, 0, 1, 0)
        };
    }

    /// <summary>
    /// Adds a succinct message to the summary message log (right side)
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
    /// Adds detailed output to the full output area (left side)
    /// </summary>
    public void AddOutput(string output)
    {
        _outputLog.Add(output);

        // Keep only the last N lines
        if (_outputLog.Count > MaxOutputLines)
        {
            _outputLog.RemoveAt(0);
        }
    }

    /// <summary>
    /// Gets the output panel for detailed information display
    /// </summary>
    private Panel GetOutputPanel()
    {
        var content = _outputLog.Any()
            ? string.Join("\n", _outputLog)
            : "[dim]Action output will appear here...[/]";

        return new Panel(content)
        {
            Header = new PanelHeader("[yellow]Output[/]"),
            Border = BoxBorder.Rounded,
            Padding = new Padding(1, 0, 1, 0)
        };
    }

    /// <summary>
    /// Gets the message panel for succinct summary display
    /// </summary>
    private Panel GetMessagePanel()
    {
        var content = _messageLog.Any()
            ? string.Join("\n", _messageLog)
            : "[dim]Summary messages...[/]";

        return new Panel(content)
        {
            Header = new PanelHeader("[yellow]Summary Log[/]"),
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

    /// <summary>
    /// Clears the output log
    /// </summary>
    public void ClearOutput()
    {
        _outputLog.Clear();
    }
}
