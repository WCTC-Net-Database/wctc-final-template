namespace ConsoleRpg.Helpers;

public class MenuManager
{
    private readonly OutputManager _outputManager;

    public MenuManager(OutputManager outputManager)
    {
        _outputManager = outputManager;
    }

    public void ShowMainMenu(Action<string> handleChoice)
    {
        _outputManager.Clear();
        _outputManager.WriteLine("=================================", ConsoleColor.Yellow);
        _outputManager.WriteLine("    RPG Character Manager", ConsoleColor.Yellow);
        _outputManager.WriteLine("=================================", ConsoleColor.Yellow);
        _outputManager.WriteLine("");
        _outputManager.WriteLine("1. Add New Character", ConsoleColor.Cyan);
        _outputManager.WriteLine("2. Edit Character", ConsoleColor.Cyan);
        _outputManager.WriteLine("3. Display All Characters", ConsoleColor.Cyan);
        _outputManager.WriteLine("4. Search Character by Name", ConsoleColor.Cyan);
        _outputManager.WriteLine("5. Exit", ConsoleColor.Cyan);
        _outputManager.WriteLine("");
        _outputManager.WriteLine("Select an option:", ConsoleColor.White);
        _outputManager.Display();

        var input = Console.ReadLine();
        handleChoice(input);
    }
}
