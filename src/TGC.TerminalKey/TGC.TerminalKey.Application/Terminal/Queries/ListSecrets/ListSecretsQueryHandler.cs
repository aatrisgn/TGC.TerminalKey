using TGC.HomeAutomation.Application.Abstractions;
using TGC.TerminalKey.Application.SecretManagement;
using TGC.WebApi.Communication.Mediator;

namespace TGC.TerminalKey.Application.Terminal.Queries.ListSecrets;

public class ListSecretsQueryHandler : BaseQueryHandler<ListSecretsQuery>, IQueryHandler
{
	private readonly IFetchSecretsService _fetchSecretsService;
	private readonly IOSSecretStore _osSecretStore;

	public ListSecretsQueryHandler(IFetchSecretsService fetchSecretsService, IOSSecretStore osSecretStore)
	{
		_fetchSecretsService = fetchSecretsService;
		_osSecretStore = osSecretStore;
	}

	public async Task<IResult<IQueryResponse>> Handle<TQuery>(TQuery query) where TQuery : IQuery
	{
		var castedQuery = GetTypedQuery(query);

		var secretVault = await _fetchSecretsService.GetSecretVaultAsync();

		await _osSecretStore.SetAsync("somet", "thing", "testing");

		var some = await _osSecretStore.GetAsync("somet", "thing");

		var response = new ListSecretsResponse
		{
			SecretVault = secretVault
		};

		return Result<ListSecretsResponse>.Success(response);
	}
}
