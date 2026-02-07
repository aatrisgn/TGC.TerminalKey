using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace TGC.TerminalKey.Terminal.Commands;

public class ListCommand : Command<ListCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("-s|--search")]
        [Description("The substring to search for.")]
        public string? Search { get; init; }
    }

    public override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(settings.Search))
        {
            AnsiConsole.MarkupLine("[yellow]Listing all items...[/]");
        }
        else
        {
            AnsiConsole.MarkupLine($"[yellow]Searching for items containing:[/] [blue]'{settings.Search}'[/]");
        }

        // Placeholder for actual logic
        var table = new Table();
        table.AddColumn("Name");
        table.AddColumn("Status");
        table.AddRow("Example Item 1", "[green]Online[/]");
        table.AddRow("Another Item", "[red]Offline[/]");
        
        AnsiConsole.Write(table);

        return 0;
    }
}
