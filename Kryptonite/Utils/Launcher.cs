using System.Diagnostics;
using Kryptonite.Types;

namespace Kryptonite.Utils;

internal static class Launcher
{
    public static void Play(Instance instance)
    {
        var instancesPath =
            $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Kryptonite\\Instances\\";
        var instancePath = instancesPath + instance.name;
        if (!Directory.Exists(instancePath)) throw new Exception("Instance directory not found.");

        var beatsaberPath = instancePath + "\\Beat Saber";
        var p = new Process
        {
            StartInfo = new ProcessStartInfo($"{beatsaberPath}\\Beat Saber.exe", "")
            {
                UseShellExecute = false,
                WorkingDirectory = $"{beatsaberPath}"
            }
        };

        try
        {
            p.StartInfo.Environment["SteamAppId"] = "620980";
            p.Start();
        }
        catch (Exception ex)
        {
            Terminal.Log(ex.Message);
        }
    }
}