using System.Text.Json;
using Microsoft.Extensions.Logging;
using TGC.TerminalKey.Application.TerminalConfiguration;

namespace TGC.TerminalKey.Infrastructure;

public class UserProfileAccessor : IUserProfileAccessor
{
	private readonly ILogger<UserProfileAccessor> _logger;

	private static string userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
	private static string UserProfileTerminalKeyFolder = Path.Combine(userProfilePath, "TerminalKey");
	private static string UserProfileTerminalKeyFile = Path.Combine(userProfilePath, "TerminalKey", "conf.json");

	public UserProfileAccessor(ILogger<UserProfileAccessor> logger)
	{
		_logger = logger;
	}

	/// <summary>
	/// Attempts to asynchronously retrieve and deserialize the user terminal configuration from the local profile file.
	/// </summary>
	/// <param name="ctsToken">A cancellation token to observe while waiting for the task to complete.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains the <see cref="UserTerminalConfiguration"/>
	/// if the file exists and is successfully deserialized; otherwise, <see langword="null"/>.
	/// </returns>
	public Task<UserTerminalConfiguration?> TryGetUserProfileFileAsync(CancellationToken ctsToken)
	{
		if (TerminalKeyFolderExists() && TerminalKeyConfigFileExists())
		{
			var fileContent = TryGetFileContent(UserProfileTerminalKeyFile);
			if (fileContent is not null)
			{
				try
				{
					var parsedConfiguration = JsonSerializer.Deserialize<UserTerminalConfiguration>(fileContent);
					return Task.FromResult(parsedConfiguration);
				}
				catch (JsonException exception)
				{
					_logger.LogError(exception, "Error parsing user profile configuration file as json. Potential invalid json.");
					return Task.FromResult<UserTerminalConfiguration?>(null);
				}
			}
		}
		return Task.FromResult<UserTerminalConfiguration?>(null);
	}

	public Task UpsertConfigurationFileAsync(UserTerminalConfiguration configuration, CancellationToken ctsToken)
	{
		if (!TerminalKeyFolderExists() && IsDirectoryWritable(UserProfileTerminalKeyFolder))
		{
			CreateTerminalKeyFolderIfNotExists();
		}

		var parsedConfiguration = JsonSerializer.Serialize(configuration);
		File.WriteAllText(UserProfileTerminalKeyFile, parsedConfiguration);
		return Task.CompletedTask;
	}

	private bool TerminalKeyFolderExists()
	{
		return Path.Exists(UserProfileTerminalKeyFolder);
	}

	private bool TerminalKeyConfigFileExists()
	{
		return Path.Exists(Path.Combine(UserProfileTerminalKeyFile));
	}

	private string? TryGetFileContent(string filePath)
	{
		try
		{
			var text = File.ReadAllText(filePath);
			return text;
		}
		catch (IOException)
		{
			// TODO: LOG SOMETHING
			_logger.LogError("Error reading user profile configuration file. Check permissions.");
			return null;
		}
	}

	public static bool IsDirectoryWritable(string directoryPath)
	{
		if (string.IsNullOrWhiteSpace(directoryPath))
		{
			return false;
		}

		try
		{
			Directory.CreateDirectory(directoryPath);

			string probeFile = Path.Combine(directoryPath, $".write_probe_{Guid.NewGuid():N}.tmp");

			// Use FileOptions.DeleteOnClose to auto-clean if possible.
			using (var fs = new FileStream(
					   probeFile,
					   FileMode.CreateNew,
					   FileAccess.Write,
					   FileShare.None,
					   bufferSize: 1,
					   FileOptions.DeleteOnClose))
			{
				// Minimal write to ensure actual write capability (some FS allow empty create w/o data).
				fs.WriteByte(0x00);
				fs.Flush(true);
			}

			// If DeleteOnClose was ignored by FS, try explicit delete (best-effort).
			if (File.Exists(probeFile))
			{
				File.Delete(probeFile);
			}
			return true;
		}
		catch (UnauthorizedAccessException)
		{
			return false;
		}
		catch (DirectoryNotFoundException)
		{
			return false;
		}
		catch (IOException)
		{
			// Includes read-only filesystem, file sharing issues, quota/full conditions, etc.
			return false;
		}
		catch (NotSupportedException)
		{
			// Malformed path, unsupported file system semantics, etc.
			return false;
		}
		// Let other exceptions bubble if you prefer, or return false.
	}

	public static bool CanCreateOrOverwriteFile(string filePath)
	{
		if (string.IsNullOrWhiteSpace(filePath))
		{
			return false;
		}

		try
		{
			string? dir = Path.GetDirectoryName(filePath);
			if (string.IsNullOrEmpty(dir))
			{
				return false;
			}

			Directory.CreateDirectory(dir);

			// Open with FileMode.Create (create or overwrite), then Write and close.
			using var fs = new FileStream(
				filePath,
				FileMode.Create,
				FileAccess.Write,
				FileShare.None);

			fs.WriteByte(0x00);
			fs.Flush(true);

			// Clean up: if this is a probe, delete; if not, omit this line.
			File.Delete(filePath);

			return true;
		}
		catch (UnauthorizedAccessException)
		{
			return false;
		}
		catch (DirectoryNotFoundException)
		{
			return false;
		}
		catch (IOException)
		{
			return false;
		}
		catch (NotSupportedException)
		{
			return false;
		}
	}

	private void CreateTerminalKeyFolderIfNotExists()
	{
		Directory.CreateDirectory(UserProfileTerminalKeyFolder);
	}
}
