using System.Runtime.InteropServices;

namespace TGC.TerminalKey.Application.SecretManagement;

public interface IOSSpecificStore : IOSSecretStore
{
	public bool IsCompatible(OSPlatform platform);
}
