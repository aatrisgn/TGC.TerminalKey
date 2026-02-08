using TGC.TerminalKey.Domain;

namespace TGC.TerminalKey.Application.SecretManagement;

public interface IFetchSecretsService
{
	Task<SecretVault> GetSecretVaultAsync();
}
