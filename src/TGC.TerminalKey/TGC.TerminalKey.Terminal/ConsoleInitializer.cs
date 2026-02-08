using Spectre.Console;
using TGC.TerminalKey.Application.TerminalConfiguration;

namespace TGC.TerminalKey.Terminal;

public class ConsoleInitializer
{
	private readonly ITerminalConfigurationInitializaionService _terminalConfigurationInitializer;

	public ConsoleInitializer(ITerminalConfigurationInitializaionService terminalConfigurationInitializer)
	{
		_terminalConfigurationInitializer = terminalConfigurationInitializer;
	}

	public async Task Initialize(CancellationToken cts)
	{
		if (!await _terminalConfigurationInitializer.IsInitializedAsync(cts))
		{
			var utilizeOnlineBackup = AnsiConsole.Confirm("Do you want to enable [blue]online backup[/]? This can always be enabled later.");
			await _terminalConfigurationInitializer.InitializeAsync(cts);
			if (utilizeOnlineBackup)
			{
				AnsiConsole.MarkupLine("[red]Online backup is not yet supported. Skipping with no effec.[/]");
				// TODO: Implement online backup initialization here.
				//terminalConfigurationInitializer.InitializeOnlineBackupAsync(cts.Token);
			}
			else
			{
				AnsiConsole.MarkupLine("[grey]Skipping online backup.[/]");
			}
		}
	}
}