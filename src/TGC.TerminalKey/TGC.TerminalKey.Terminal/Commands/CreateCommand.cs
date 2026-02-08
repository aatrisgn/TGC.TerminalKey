using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace TGC.TerminalKey.Terminal.Commands;

public class CreateCommand : AsyncCommand<CreateCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("-n|--name")]
        [Description("The name of the new item.")]
        public string Name { get; init; } = string.Empty;

        [CommandOption("-p|--password")]
        [Description("The password for the new item. If omitted, one will be auto-generated.")]
        public string? Password { get; init; }

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
        string password = settings.Password ?? Guid.NewGuid().ToString("N").Substring(0, 12);
        bool isGenerated = string.IsNullOrEmpty(settings.Password);

        AnsiConsole.MarkupLine($"[green]Created item:[/] [blue]'{settings.Name}'[/]");
        if (isGenerated)
        {
            AnsiConsole.MarkupLine($"[yellow]Auto-generated password:[/] [blue]'{password}'[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[yellow]Password stored successfully.[/]");
        }

        return Task.FromResult(0);
    }
}
