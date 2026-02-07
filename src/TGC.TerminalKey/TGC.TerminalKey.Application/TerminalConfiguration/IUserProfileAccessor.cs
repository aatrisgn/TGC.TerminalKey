namespace TGC.TerminalKey.Application.TerminalConfiguration;

public interface IUserProfileAccessor
{
	Task<UserTerminalConfiguration?> TryGetUserProfileFileAsync(CancellationToken ctsToken);
	Task UpsertConfigurationFileAsync(UserTerminalConfiguration configuration, CancellationToken ctsToken);
}
