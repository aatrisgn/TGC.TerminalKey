using Spectre.Console.Cli;
using TGC.TerminalKey.Terminal.Commands;

namespace TGC.TerminalKey.Terminal;

public static class CommandAppExtensions
{
	public static CommandApp RegisterCommands(this CommandApp app)
	{
		app.Configure(config =>
		{
			config.AddCommand<ListCommand>("List")
				.WithDescription("List all items or search for specific items.")
				.WithExample(["List"])
				.WithExample(["List", "-s", "searchterm"]);

			config.AddCommand<GetCommand>("get")
				.WithDescription("Retrieves a specific item based on its name.")
				.WithExample(["get", "-n", "itemname"]);

			config.AddCommand<CreateCommand>("create")
				.WithDescription("Creates a new item for a given name and stores or generates a password.")
				.WithExample(["create", "-n", "itemname", "-p", "password"])
				.WithExample(["create", "-n", "itemname"]);
		});

		return app;
	}
}