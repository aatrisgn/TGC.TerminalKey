namespace TGC.TerminalKey.Application.SecretManagement;

public interface IOSSecretStore
{
	Task SetAsync(string service, string account, string secret, CancellationToken ct = default);
	Task<string?> GetAsync(string service, string account, CancellationToken ct = default);
	Task DeleteAsync(string service, string account, CancellationToken ct = default);
}
