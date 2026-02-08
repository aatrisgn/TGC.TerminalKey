using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using Microsoft.Extensions.Hosting;
using Spectre.Console;
using TGC.HomeAutomation.API;
using TGC.HomeAutomation.Application.Abstractions;
using TGC.TerminalKey.Application;
using TGC.TerminalKey.Infrastructure;
using TGC.TerminalKey.Terminal;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.InjectTerminalInfrastructure();
builder.Services.InjectTerminalApplication();

builder.Services.AddScoped<ConsoleInitializer>();
builder.Services.AddScoped<IMediator, Mediator>();

var registrar = new ConsoleTypeRegistrar(builder.Services);

try
{
    var cancellationTokenSource = new CancellationTokenSource();

    Console.CancelKeyPress += (_, e) =>
    {
        e.Cancel = true;
        cancellationTokenSource.Cancel();
        AnsiConsole.MarkupLine("Cancellation requested...");
    };

    var app = new CommandApp(registrar);
    app.RegisterCommands();

    return await app.RunAsync(args, cancellationTokenSource.Token).ConfigureAwait(false);
}
catch (Exception ex)
{
    AnsiConsole.WriteException(ex);
    return 1;
}