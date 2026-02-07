namespace TGC.TerminalKey.Application.TerminalConfiguration;

public class UserTerminalConfiguration
{
	public bool UseOnlineBackup => _useOnlineBackup;
	private bool _useOnlineBackup;

	public DateTime LastUpdated => _lastUpdated;
	private DateTime _lastUpdated = DateTime.Now;

	public DateTime LastBackup => _lastBackup;
	private DateTime _lastBackup = DateTime.MinValue;

	public string Version => _version;
	private string _version = "0.0.1";

	public void SetConfiguration(UserTerminalConfiguration userProfileConfigurationFile)
	{
		ArgumentNullException.ThrowIfNull(userProfileConfigurationFile);

		_useOnlineBackup = userProfileConfigurationFile.UseOnlineBackup;
		_lastUpdated = userProfileConfigurationFile.LastUpdated;
		_lastBackup = userProfileConfigurationFile.LastBackup;
		_version = userProfileConfigurationFile.Version;
	}
}