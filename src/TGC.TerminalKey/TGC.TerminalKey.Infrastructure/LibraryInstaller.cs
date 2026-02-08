using Microsoft.Extensions.DependencyInjection;
using TGC.TerminalKey.Application.SecretManagement;
using TGC.TerminalKey.Application.TerminalConfiguration;
using TGC.TerminalKey.Infrastructure.OSSecretStores;

namespace TGC.TerminalKey.Infrastructure;

public static class LibraryInstaller
{
	public static IServiceCollection InjectTerminalInfrastructure(this IServiceCollection services)
	{
		services.AddScoped<IUserProfileAccessor, UserProfileAccessor>();

		services.AddTransient<IOSSpecificStore, LinuxSecretStore>();
		services.AddTransient<IOSSpecificStore, WindowsSecretStore>();
		services.AddTransient<IOSSpecificStore, OSXSecretStore>();

		return services;
	}
}
