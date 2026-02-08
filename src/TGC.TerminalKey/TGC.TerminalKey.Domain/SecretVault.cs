namespace TGC.TerminalKey.Domain;

public class SecretVault
{
	public string VaultName { get; set; }
	public IEnumerable<UserSecret> UserSecrets { get; set; }
	public DateTime LastUpdated { get; set; }
	public DateTime Created { get; set; }
}