using Microsoft.Extensions.DependencyInjection;
using TGC.TerminalKey.Application.TerminalConfiguration;
using TGC.TerminalKey.Infrastructure;

namespace TGC.TerminalKey.Application;

public static class LibraryInstaller
{
	public static IServiceCollection InjectTerminalInfrastructure(this IServiceCollection services)
	{
		services.AddScoped<IUserProfileAccessor, UserProfileAccessor>();
		return services;
	}
}
