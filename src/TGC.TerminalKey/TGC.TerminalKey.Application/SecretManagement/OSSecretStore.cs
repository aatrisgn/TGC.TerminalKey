using System.Runtime.InteropServices;

namespace TGC.TerminalKey.Application.SecretManagement;

public class OSSecretStore : IOSSecretStore
{
	private readonly IEnumerable<IOSSpecificStore> _OSSpecificSecretStores;

	public OSSecretStore(IEnumerable<IOSSpecificStore> OSSpecificSecretStores)
	{
		_OSSpecificSecretStores = OSSpecificSecretStores;
	}

	public Task SetAsync(string service, string account, string secret, CancellationToken ct = default)
	{
		var store = GetOSSecretStore();
		store.SetAsync(service, account, secret, ct);
		return Task.CompletedTask;
	}

	public Task<string?> GetAsync(string service, string account, CancellationToken ct = default)
	{
		var store = GetOSSecretStore();
		return store.GetAsync(service, account, ct);
	}

	public Task DeleteAsync(string service, string account, CancellationToken ct = default)
	{
		var store = GetOSSecretStore();
		store.DeleteAsync(service, account, ct);
		return Task.CompletedTask;
	}

	private IOSSpecificStore GetOSSecretStore()
	{
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			return DetermineOSSecretStore(OSPlatform.Windows);
		}

		if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
		{
			return DetermineOSSecretStore(OSPlatform.OSX);
		}

		if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
		{
			return DetermineOSSecretStore(OSPlatform.Linux);
		}
		throw new PlatformNotSupportedException("Unsupported OS platform.");
	}

	private IOSSpecificStore DetermineOSSecretStore(OSPlatform platform)
	{
		var specificOSSecretStore = _OSSpecificSecretStores.SingleOrDefault(store => store.IsCompatible(platform));
		if (specificOSSecretStore is not null)
		{
			return specificOSSecretStore;
		}

		throw new PlatformNotSupportedException("Unsupported OS platform.");
	}
}
