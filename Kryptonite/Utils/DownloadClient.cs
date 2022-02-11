using System.Diagnostics;
using System.Security;
using Kryptonite.Types;
using Version = Kryptonite.Types.Version;

namespace Kryptonite.Utils;

public static class DownloadClient
{
    public static void Download(Instance instance, Version version, User user)
    {
        var beatSaberPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Kryptonite\\Instances\\{instance.name}\\Beat Saber";
        
        try
        {
            Terminal.Log($"Downloading Beat Saber v{version.version}");
            var download = Process.Start("dotnet", $"DepotDownloader\\DepotDownloader.dll -app 620980 -depot 620981 -manifest {version.manifest} -username {user.name} -password {new System.Net.NetworkCredential(string.Empty, user.password).Password} -dir \"Beat Saber\" -validate");
            
            download.WaitForExit();
            if (download.ExitCode != 0)
            {
                Terminal.Log("Download failed");
                return;
            }
            Terminal.Log("Download complete, moving files to correct location");
            Directory.Move("Beat Saber", $"{beatSaberPath}");
            Terminal.Log("Beat Saber moved to correct location");
            
            Terminal.Log($"Successfully downloaded Beat Saber v{version.version}\nPress any key to continue",
                hold: true);
        }
        catch (Exception e)
        {
            Terminal.Log("Error downloading Beat Saber: " + e.Message);
        }
    }
}