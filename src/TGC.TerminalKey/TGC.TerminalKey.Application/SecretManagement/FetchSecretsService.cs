using TGC.TerminalKey.Domain;

namespace TGC.TerminalKey.Application.SecretManagement;

public class FetchSecretsService : IFetchSecretsService
{
	public Task<SecretVault> GetSecretVaultAsync()
	{
		var something = new SecretVault
		{
			UserSecrets = new List<UserSecret>
			{
				new UserSecret
				{
					SecretName = "Somet",
					SecretValue = "Thing",
					PreviousVersions = Array.Empty<UserSecretRecord>()
				}
			}
		};
		return Task.FromResult(something);
	}
}
