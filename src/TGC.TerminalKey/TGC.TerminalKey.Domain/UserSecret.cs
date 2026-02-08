namespace TGC.TerminalKey.Domain;

public class UserSecret
{
	public string SecretName { get; set; }
	public string SecretValue { get; set; }
	public IEnumerable<UserSecretRecord> PreviousVersions { get; set; }
}