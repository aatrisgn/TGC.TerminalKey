using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using TGC.TerminalKey.Terminal.Commands;
using Microsoft.Extensions.Hosting;
using Spectre.Console;
using TGC.HomeAutomation.API;
using TGC.HomeAutomation.Application.Abstractions;
using TGC.TerminalKey.Application;
using TGC.TerminalKey.Terminal;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.InjectTerminalApplication();
builder.Services.InjectTerminalInfrastructure();
builder.Services.AddScoped<ConsoleInitializer>();
builder.Services.AddScoped<IMediator, Mediator>();

var registrar = new ConsoleTypeRegistrar(builder.Services);

// var host = builder.Build();

// var logger = host.Services.GetRequiredService<ILoggerFactory>()
//     .CreateLogger("Startup");

try
{
    // logger.LogInformation("Starting application");

    // Resolve and run your app logic
    // var appLifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
    // using var cts = CancellationTokenSource.CreateLinkedTokenSource(appLifetime.ApplicationStopping);
    // using var scope = host.Services.CreateScope();
    //
    // var terminalConfigurationInitializer = scope.ServiceProvider.GetRequiredService<ITerminalConfigurationInitializaionService>();
    //
    // if (!await terminalConfigurationInitializer.IsInitializedAsync(cts.Token))
    // {
    //     var utilizeOnlineBackup = AnsiConsole.Confirm("Do you want to enable [blue]online backup[/]? This can always be enabled later.");
    //     await terminalConfigurationInitializer.InitializeAsync(cts.Token);
    //     if (utilizeOnlineBackup)
    //     {
    //         AnsiConsole.MarkupLine("[red]Online backup is not yet supported. Skipping with no effec.[/]");
    //         // TODO: Implement online backup initialization here.
    //         //terminalConfigurationInitializer.InitializeOnlineBackupAsync(cts.Token);
    //     }
    //     else
    //     {
    //         AnsiConsole.MarkupLine("[grey]Skipping online backup.[/]");
    //     }
    // }

    var cancellationTokenSource = new CancellationTokenSource();

    // Wire up Console.CancelKeyPress to trigger cancellation
    Console.CancelKeyPress += (_, e) =>
    {
        e.Cancel = true; // Prevent immediate process termination
        cancellationTokenSource.Cancel();
        Console.WriteLine("Cancellation requested...");
    };

    var app = new CommandApp(registrar);

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

    return await app.RunAsync(args, cancellationTokenSource.Token);
}
catch (Exception ex)
{
    AnsiConsole.WriteException(ex);
    return 1;
}