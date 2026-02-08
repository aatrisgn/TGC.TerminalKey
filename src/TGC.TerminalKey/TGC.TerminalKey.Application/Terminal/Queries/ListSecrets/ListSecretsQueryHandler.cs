using TGC.HomeAutomation.Application.Abstractions;
using TGC.TerminalKey.Application.SecretManagement;
using TGC.WebApi.Communication.Mediator;

namespace TGC.TerminalKey.Application.Terminal.Queries.ListSecrets;

public class ListSecretsQueryHandler : BaseQueryHandler<ListSecretsQuery>, IQueryHandler
{
	private readonly IFetchSecretsService _fetchSecretsService;

	public ListSecretsQueryHandler(IFetchSecretsService fetchSecretsService)
	{
		_fetchSecretsService = fetchSecretsService;
	}

	public async Task<IResult<IQueryResponse>> Handle<TQuery>(TQuery query) where TQuery : IQuery
	{
		var castedQuery = GetTypedQuery(query);

		var secretVault = await _fetchSecretsService.GetSecretVaultAsync();

		var response = new ListSecretsResponse
		{
			SecretVault = secretVault
		};

		return Result<ListSecretsResponse>.Success(response);
	}
}
