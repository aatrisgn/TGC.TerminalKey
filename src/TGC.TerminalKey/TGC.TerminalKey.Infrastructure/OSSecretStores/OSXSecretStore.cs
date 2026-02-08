using System.Diagnostics;
using System.Runtime.InteropServices;
using TGC.TerminalKey.Application.SecretManagement;

namespace TGC.TerminalKey.Infrastructure.OSSecretStores;

public class OSXSecretStore : IOSSpecificStore
{
	public async Task SetAsync(string service, string account, string secret, CancellationToken ct = default)
	{
		// Upsert: -U updates if item exists
		await RunSecurityAsync($"add-generic-password -a {Esc(account)} -s {Esc(service)} -w {Esc(secret)} -U", ct);
	}

	public async Task<string?> GetAsync(string service, string account, CancellationToken ct = default)
	{
		var (exitCode, output, error) =
			await RunSecurityWithOutputAsync($"find-generic-password -a {Esc(account)} -s {Esc(service)} -w", ct);
		if (exitCode == 0)
		{
			return output.TrimEnd();
		}
		if (error.Contains("could not be found", StringComparison.OrdinalIgnoreCase))
		{
			return null;
		}
		throw new InvalidOperationException($"security find failed: {error}");
	}

	public async Task DeleteAsync(string service, string account, CancellationToken ct = default)
	{
		var (exitCode, _, error) =
			await RunSecurityWithOutputAsync($"delete-generic-password -a {Esc(account)} -s {Esc(service)}", ct);
		if (exitCode == 0)
		{
			return;
		}
		if (error.Contains("could not be found", StringComparison.OrdinalIgnoreCase))
		{
			return;
		}
		throw new InvalidOperationException($"security delete failed: {error}");
	}

	private static string Esc(string v) => $"\"{v.Replace("\"", "\\\"")}\"";

	private static async Task RunSecurityAsync(string args, CancellationToken ct)
	{
		var (exit, _, err) = await RunSecurityWithOutputAsync(args, ct);
		if (exit != 0)
		{
			throw new InvalidOperationException($"security {args} failed: {err}");
		}
	}

	private static async Task<(int exit, string stdout, string stderr)> RunSecurityWithOutputAsync(string args,
		CancellationToken ct)
	{
		var psi = new ProcessStartInfo
		{
			FileName = "/usr/bin/security",
			Arguments = args,
			RedirectStandardOutput = true,
			RedirectStandardError = true
		};
		using var p = Process.Start(psi)!;
		var stdout = await p.StandardOutput.ReadToEndAsync();
		var stderr = await p.StandardError.ReadToEndAsync();
		await p.WaitForExitAsync(ct);
		return (p.ExitCode, stdout, stderr);
	}

	public bool IsCompatible(OSPlatform platform)
	{
		return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
	}
}
