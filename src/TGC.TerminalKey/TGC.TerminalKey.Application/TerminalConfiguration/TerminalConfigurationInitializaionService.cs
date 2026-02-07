namespace TGC.TerminalKey.Application.TerminalConfiguration;

public class TerminalConfigurationInitializaionService : ITerminalConfigurationInitializaionService
{
	private readonly IUserProfileAccessor _userProfileAcessor;
	private readonly UserTerminalConfiguration _terminalConfiguration;

	public TerminalConfigurationInitializaionService(IUserProfileAccessor userProfileAccessor, UserTerminalConfiguration terminalConfiguration)
	{
		_userProfileAcessor = userProfileAccessor;
		_terminalConfiguration = terminalConfiguration;
	}

	public async Task<bool> IsInitializedAsync(CancellationToken ctsToken)
	{
		var userProfileConfigurationFile = await _userProfileAcessor.TryGetUserProfileFileAsync(ctsToken);

		if (userProfileConfigurationFile is null)
		{
			return false;
		}

		var isValid = await ValidateUserConfigurationFileAsync(userProfileConfigurationFile);

		_terminalConfiguration.SetConfiguration(userProfileConfigurationFile);

		return isValid;
	}

	public async Task InitializeAsync(CancellationToken ctsToken)
	{
		var terminakConfiguration = new UserTerminalConfiguration();
		await _userProfileAcessor.UpsertConfigurationFileAsync(terminakConfiguration, ctsToken);
	}

	private Task<bool> ValidateUserConfigurationFileAsync(UserTerminalConfiguration userConfiguration)
	{
		return Task.FromResult(true);
	}
}
