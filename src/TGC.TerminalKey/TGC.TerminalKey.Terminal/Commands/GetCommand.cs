using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace TGC.TerminalKey.Terminal.Commands;

public class GetCommand : AsyncCommand<GetCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("-n|--name")]
        [Description("The exact name of the item to retrieve.")]
        public string Name { get; init; } = string.Empty;

        public override ValidationResult Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return ValidationResult.Error("Name is required.");
            }
            return base.Validate();
        }
    }

    public override Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        AnsiConsole.MarkupLine($"[yellow]Retrieving item:[/] [blue]'{settings.Name}'[/]");

        // Placeholder for actual logic
        var table = new Table();
        table.AddColumn("Name");
        table.AddColumn("Password");
        table.AddRow(settings.Name, "********");

        AnsiConsole.Write(table);

        return Task.FromResult(0);
    }
}
