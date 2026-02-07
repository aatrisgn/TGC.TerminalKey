namespace TGC.TerminalKey.Application.TerminalConfiguration;

public interface ITerminalConfigurationInitializaionService
{
	Task<bool> IsInitializedAsync(CancellationToken ctsToken);
	Task InitializeAsync(CancellationToken ctsToken);
}