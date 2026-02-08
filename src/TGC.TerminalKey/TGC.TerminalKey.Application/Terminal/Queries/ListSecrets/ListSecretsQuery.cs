using TGC.HomeAutomation.Application.Abstractions;

namespace TGC.TerminalKey.Application.Terminal.Queries.ListSecrets;

public class ListSecretsQuery : BaseQuery
{
	public string SearchPattern { get; }

	public ListSecretsQuery(string searchPattern = "*")
	{
		SearchPattern = searchPattern;
	}

	public static ListSecretsQuery Empty() => new();
}
