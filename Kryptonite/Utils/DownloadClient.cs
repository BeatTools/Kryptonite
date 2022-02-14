using System.Diagnostics;
using System.Net;
using Kryptonite.Types;
using Kryptonite.Types.Exceptions;

namespace Kryptonite.Utils;

/// <summary>
/// This class contains methods for downloading Beat Saber files.
/// </summary>
public static class DownloadClient
{
    /// <summary>
    /// Downloads Beat Saber with the given version.
    /// </summary>
    /// <param name="instance">The Kryptonite instance.</param>
    /// <param name="version">The version of Beat Saber to download.</param>
    /// <param name="user">The user to download Beat Saber as.</param>
    public static void Download(Instance instance, GameVersion version, User user)
    {
        var beatSaberPath =
            $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Kryptonite\\Instances\\{instance.Name}\\Beat Saber";

        try
        {
            Terminal.Log($"Downloading Beat Saber v{version.Version} for {instance.Name}...");
            using var download = Process.Start("dotnet",
                $"DepotDownloader\\DepotDownloader.dll -app 620980 -depot 620981 -manifest {version.Manifest} -username {user.Name} -password {new NetworkCredential(string.Empty, user.Password).Password} -dir \"{beatSaberPath}\" -validate");
            download.WaitForExit();
            Terminal.Log(
                download.ExitCode == 0
                    ? $"Successfully downloaded Beat Saber v{version.Version}\nPress any key to return to the main menu."
                    : $"Error: Received exit code {download.ExitCode} from Depot Downloader.\nPress any key to return to the main menu.",
                hold: true);
        }
        catch (KryptoniteException e)
        {
            Terminal.Log($"Error downloading Beat Saber: {e.Message}\nPress any key to return to the main menu.",
                hold: true);
        }
    }
}