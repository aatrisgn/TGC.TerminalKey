using TGC.TerminalKey.Domain;
using TGC.WebApi.Communication.Mediator;

namespace TGC.TerminalKey.Application.Terminal.Queries.ListSecrets;

public class ListSecretsResponse : BaseResponse
{
	public SecretVault SecretVault { get; set; } = new();
}
