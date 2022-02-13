using System.Diagnostics;
using System.Net;
using Kryptonite.Types;

namespace Kryptonite.Utils;

public static class DownloadClient
{
    public static void Download(Instance instance, GameVersion version, User user)
    {
        var beatSaberPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Kryptonite\\Instances\\{instance.Name}\\Beat Saber";

        try
        {
            Terminal.Log($"Downloading Beat Saber v{version.Version} for {instance.Name}...");
            using var download = Process.Start("dotnet", $"DepotDownloader\\DepotDownloader.dll -app 620980 -depot 620981 -manifest {version.Manifest} -username {user.Name} -password {new NetworkCredential(string.Empty, user.Password).Password} -dir \"{beatSaberPath}\" -validate");
            download.WaitForExit();
            Terminal.Log(
                download.ExitCode == 0
                    ? $"Successfully downloaded Beat Saber v{version.Version}\nPress any key to return to the main menu."
                    : $"Error: Received exit code {download.ExitCode} from Depot Downloader.\nPress any key to return to the main menu.",
                hold: true);
        }
        catch (Exception e)
        {
            Terminal.Log("Error downloading Beat Saber: " + e.Message, hold: true);
        }
    }
}