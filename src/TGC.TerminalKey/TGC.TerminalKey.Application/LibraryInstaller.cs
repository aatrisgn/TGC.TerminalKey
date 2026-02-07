using Microsoft.Extensions.DependencyInjection;
using TGC.TerminalKey.Application.TerminalConfiguration;

namespace TGC.TerminalKey.Application;

public static class LibraryInstaller
{
	public static IServiceCollection InjectTerminalApplication(this IServiceCollection services)
	{
		services.AddScoped<ITerminalConfigurationInitializaionService, TerminalConfigurationInitializaionService>();

		services.AddSingleton<UserTerminalConfiguration>();
		return services;
	}
}
