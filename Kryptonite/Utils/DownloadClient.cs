using System.Diagnostics;
using System.Net;
using Kryptonite.Types;
using Version = Kryptonite.Types.Version;

namespace Kryptonite.Utils;

public static class DownloadClient
{
    public static void Download(Instance instance, Version version, User user)
    {
        var beatSaberPath =
            $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Kryptonite\\Instances\\{instance.name}\\Beat Saber";

        try
        {
            Terminal.Log($"Downloading Beat Saber v{version.version}");
            var download = Process.Start("dotnet",
                $"DepotDownloader\\DepotDownloader.dll -app 620980 -depot 620981 -manifest {version.manifest} -username {user.name} -password {new NetworkCredential(string.Empty, user.password).Password} -dir \"{beatSaberPath}\" -validate");

            download.WaitForExit();
            Terminal.Log(
                download.ExitCode == 0
                    ? $"Successfully downloaded Beat Saber v{version.version}\nPress any key to return to the main menu."
                    : $"Error: Received exit code {download.ExitCode} from Depot Downloader.\nPress any key to return to the main menu.",
                hold: true);
        }
        catch (Exception e)
        {
            Terminal.Log("Error downloading Beat Saber: " + e.Message, hold: true);
        }
    }
}