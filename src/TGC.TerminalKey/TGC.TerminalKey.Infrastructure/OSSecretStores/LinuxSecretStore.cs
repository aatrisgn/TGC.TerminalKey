using System.Diagnostics;
using System.Runtime.InteropServices;
using TGC.TerminalKey.Application.SecretManagement;

namespace TGC.TerminalKey.Infrastructure.OSSecretStores;

public class LinuxSecretStore : IOSSpecificStore
{
	public async Task SetAsync(string service, string account, string secret, CancellationToken ct = default)
	{
		// Store: attributes service=<service> account=<account>
		// Label helps visualize in GUI keyring apps
		var args = $"store --label={Esc($"{service} {account}")} service {Esc(service)} account {Esc(account)}";
		await RunSecretToolAsync(args, stdin: secret, ct);
	}

	public async Task<string?> GetAsync(string service, string account, CancellationToken ct = default)
	{
		// Lookup outputs the secret directly
		var (exit, stdout, stderr) =
			await RunSecretToolWithOutputAsync($"lookup service {Esc(service)} account {Esc(account)}", ct);
		if (exit == 0)
		{
			return stdout.TrimEnd();
		}
		if (exit == 1 || stderr.Contains("No secret was found", StringComparison.OrdinalIgnoreCase))
		{
			return null;
		}
		throw new InvalidOperationException($"secret-tool lookup failed: {stderr}");
	}

	public async Task DeleteAsync(string service, string account, CancellationToken ct = default)
	{
		// 'clear' removes item matching attributes
		var (exit, _, stderr) =
			await RunSecretToolWithOutputAsync($"clear service {Esc(service)} account {Esc(account)}", ct);
		if (exit == 0 || stderr.Contains("No secret was found", StringComparison.OrdinalIgnoreCase))
		{
			return;
		}
		throw new InvalidOperationException($"secret-tool clear failed: {stderr}");
	}

	private static string Esc(string v) => $"\"{v.Replace("\"", "\\\"")}\"";

	private static async Task RunSecretToolAsync(string args, string? stdin, CancellationToken ct)
	{
		var (exit, _, err) = await RunSecretToolWithOutputAsync(args, ct, stdin);
		if (exit != 0)
		{
			throw new InvalidOperationException($"secret-tool {args} failed: {err}");
		}
	}

	private static async Task<(int exit, string stdout, string stderr)> RunSecretToolWithOutputAsync(string args,
		CancellationToken ct, string? stdin = null)
	{
		var psi = new ProcessStartInfo
		{
			FileName = "secret-tool",
			Arguments = args,
			RedirectStandardOutput = true,
			RedirectStandardError = true,
			RedirectStandardInput = stdin is not null
		};
		using var p = Process.Start(psi)!;
		if (stdin is not null)
		{
			await p.StandardInput.WriteAsync(stdin);
			p.StandardInput.Close();
		}

		var stdout = await p.StandardOutput.ReadToEndAsync();
		var stderr = await p.StandardError.ReadToEndAsync();
		await p.WaitForExitAsync(ct);
		return (p.ExitCode, stdout, stderr);
	}

	public bool IsCompatible(OSPlatform platform)
	{
		return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
	}
}
