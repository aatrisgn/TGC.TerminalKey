namespace TGC.TerminalKey.Domain;

public record UserSecretRecord
{
	public string SecretName { get; set; }
	public string SecretValue { get; set; }
	public DateTime Created { get; set; }
	public int Version { get; set; }
}