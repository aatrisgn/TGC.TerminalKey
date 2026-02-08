using Microsoft.Extensions.DependencyInjection;
using TGC.TerminalKey.Application.SecretManagement;
using TGC.TerminalKey.Application.Terminal.Queries.ListSecrets;
using TGC.TerminalKey.Application.TerminalConfiguration;
using TGC.WebApi.Communication.Mediator;

namespace TGC.TerminalKey.Application;

public static class LibraryInstaller
{
	public static IServiceCollection InjectTerminalApplication(this IServiceCollection services)
	{
		services.AddScoped<ITerminalConfigurationInitializaionService, TerminalConfigurationInitializaionService>();
		services.AddScoped<IFetchSecretsService, FetchSecretsService>();
		services.AddScoped<IOSSecretStore, OSSecretStore>();

		services.AddSingleton<UserTerminalConfiguration>();

		services.RegisterCommands();
		services.RegisterQueries();

		return services;
	}

	private static IServiceCollection RegisterCommands(this IServiceCollection services)
	{
		return services;
	}

	private static IServiceCollection RegisterQueries(this IServiceCollection services)
	{
		services.AddScoped<IQueryHandler, ListSecretsQueryHandler>();
		return services;
	}
}
