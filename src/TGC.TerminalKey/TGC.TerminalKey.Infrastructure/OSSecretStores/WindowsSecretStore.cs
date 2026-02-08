using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using TGC.TerminalKey.Application.SecretManagement;

namespace TGC.TerminalKey.Infrastructure.OSSecretStores;

public class WindowsSecretStore : IOSSpecificStore
{
	public Task SetAsync(string service, string account, string secret, CancellationToken ct = default)
	{
		var target = $"{service}/{account}";
		var bytes = Encoding.UTF8.GetBytes(secret);
		var cred = new NativeMethods.CREDENTIAL
		{
			Type = 1, // CRED_TYPE_GENERIC
			TargetName = target,
			CredentialBlob = secret,
			CredentialBlobSize = (uint)bytes.Length,
			Persist = 1, // CRED_PERSIST_LOCAL_MACHINE or use 1 for session, 2 for local machine, 3 for enterprise
			AttributeCount = 0,
			Attributes = IntPtr.Zero,
			Comment = null,
			TargetAlias = null,
			UserName = account
		};

		if (!NativeMethods.CredWrite(ref cred, 0))
		{
			ThrowLastWin32();
		}

		return Task.CompletedTask;
	}

	public Task<string?> GetAsync(string service, string account, CancellationToken ct = default)
	{
		var target = $"{service}/{account}";
		if (NativeMethods.CredRead(target, 1, 0, out var pcred))
		{
			try
			{
				var cred = Marshal.PtrToStructure<NativeMethods.PCREDENTIAL>(pcred);
				return Task.FromResult(cred.CredentialBlob ?? null);
			}
			finally
			{
				NativeMethods.CredFree(pcred);
			}
		}

		// Not found
		var error = Marshal.GetLastWin32Error();
		if (error != 1168 /*ERROR_NOT_FOUND*/)
		{
			ThrowLastWin32();
			return Task.FromResult<string?>(null);
		}

		return Task.FromResult<string?>(null);
	}

	public Task DeleteAsync(string service, string account, CancellationToken ct = default)
	{
		var target = $"{service}/{account}";
		if (!NativeMethods.CredDelete(target, 1, 0))
		{
			var err = Marshal.GetLastWin32Error();
			if (err == 1168 /*ERROR_NOT_FOUND*/)
			{
				return Task.CompletedTask;
			}
			ThrowLastWin32();
		}

		return Task.CompletedTask;
	}

	private static void ThrowLastWin32() =>
		throw new Win32Exception(Marshal.GetLastWin32Error());

	private static class NativeMethods
	{
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CREDENTIAL
		{
			public uint Flags;
			public uint Type;
			public string TargetName;
			public string? Comment;
			public System.Runtime.InteropServices.ComTypes.FILETIME LastWritten;
			public uint CredentialBlobSize;
			public string CredentialBlob; // This marshals as LPWStr
			public uint Persist;
			public uint AttributeCount;
			public IntPtr Attributes;
			public string? TargetAlias;
			public string? UserName;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct PCREDENTIAL
		{
			public uint Flags;
			public uint Type;
			public string TargetName;
			public string? Comment;
			public System.Runtime.InteropServices.ComTypes.FILETIME LastWritten;
			public uint CredentialBlobSize;
			public string CredentialBlob;
			public uint Persist;
			public uint AttributeCount;
			public IntPtr Attributes;
			public string? TargetAlias;
			public string? UserName;
		}

		[DllImport("Advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		internal static extern bool CredWrite([In] ref CREDENTIAL userCredential, [In] uint flags);

		[DllImport("Advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		internal static extern bool CredRead(string target, uint type, uint reservedFlag, out IntPtr credentialPtr);

		[DllImport("Advapi32.dll", SetLastError = true)]
		internal static extern bool CredDelete(string target, uint type, uint flags);

		[DllImport("Advapi32.dll", SetLastError = true)]
		internal static extern void CredFree([In] IntPtr buffer);
	}

	public bool IsCompatible(OSPlatform platform)
	{
		return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
	}
}
