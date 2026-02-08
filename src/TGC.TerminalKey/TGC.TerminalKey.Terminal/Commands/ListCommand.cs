using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;
using TGC.HomeAutomation.Application.Abstractions;
using TGC.TerminalKey.Application.Terminal.Queries.ListSecrets;
using TGC.TerminalKey.Domain;

namespace TGC.TerminalKey.Terminal.Commands;

public class ListCommand : AsyncCommand<ListCommand.Settings>
{
    private readonly IMediator _terminalActionResolver;
    private readonly ConsoleInitializer _consoleInitializer;
    public ListCommand(IMediator terminalActionResolver, ConsoleInitializer consoleInitializer)
    {
        _terminalActionResolver = terminalActionResolver;
        _consoleInitializer = consoleInitializer;
    }

    public class Settings : CommandSettings
    {
        [CommandOption("-s|--search")]
        [Description("The substring to search for.")]
        public string? Search { get; init; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        await _consoleInitializer.Initialize(cancellationToken);

        if (string.IsNullOrEmpty(settings.Search))
        {
            AnsiConsole.MarkupLine("[yellow]Listing all items...[/]");

            var query = ListSecretsQuery.Empty();
            var results = await _terminalActionResolver.HandleQueryAsync<ListSecretsQuery, ListSecretsResponse>(query);

            ListSecrets(results.SecretVault);
        }
        else
        {
            AnsiConsole.MarkupLine($"[yellow]Searching for items containing:[/] [blue]'{settings.Search}'[/]");

            var query = new ListSecretsQuery(settings.Search);
            var results = await _terminalActionResolver.HandleQueryAsync<ListSecretsQuery, ListSecretsResponse>(query);

            ListSecrets(results.SecretVault);
        }
        return 0;
    }

    private static void ListSecrets(SecretVault secretVault)
    {
        var table = new Table();
        table.AddColumn("Name");
        table.AddColumn("Value");

        foreach (var secrets in secretVault.UserSecrets)
        {
            table.AddRow(secrets.SecretName, "*****");
        }

        AnsiConsole.Write(table);
    }
}
