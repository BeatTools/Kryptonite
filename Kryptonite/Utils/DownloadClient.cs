using System.Diagnostics;
using System.Net;
using Kryptonite.Types;
using Kryptonite.Types.Exceptions;

namespace Kryptonite.Utils;

/// <summary>
///     This class contains methods for downloading Beat Saber files.
/// </summary>
public static class DownloadClient
{
    /// <summary>
    ///     Downloads Beat Saber with the given version.
    /// </summary>
    /// <param name="name">The Kryptonite instance.</param>
    /// <param name="version">The version of Beat Saber to download.</param>
    /// <param name="user">The user to download Beat Saber as.</param>
    /// <param name="updateOnly">Whether to only update Beat Saber.</param>
    public static Task Download(string name, GameVersion version, User user, bool updateOnly = false)
    {
        var beatSaberPath =
            $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\Kryptonite\\Instances\\{name}\\Beat Saber";
        var downloadsPath =
            $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\Kryptonite\\Downloads\\{name}";

        try
        {
            Directory.CreateDirectory(downloadsPath);
            Terminal.Log($"Downloading Beat Saber v{version.Version} for {name}...");
            using var download = Process.Start("dotnet",
                $"DepotDownloader\\DepotDownloader.dll -app 620980 -depot 620981 -manifest {version.Manifest} -username {user.Name} -password {new NetworkCredential(string.Empty, user.Password).Password} -dir \"{beatSaberPath}\" -validate");
            download.WaitForExit();
            if (download.ExitCode != 0) throw new KryptoniteException("Failed to download Beat Saber.");

            if (updateOnly) return Task.CompletedTask;

            Directory.CreateDirectory(beatSaberPath);
            Directory.Move($"{downloadsPath}", $"{beatSaberPath}\\Beat Saber");

            return Task.CompletedTask;
        }
        catch (KryptoniteException e)
        {
            Terminal.Log($"Error downloading Beat Saber: {e.Message}\nPress any key to return to the main menu.",
                hold: true);
        }

        return Task.FromResult(Task.CompletedTask);
    }
}