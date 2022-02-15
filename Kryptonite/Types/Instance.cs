using System.Diagnostics;
using Kryptonite.Utils;

namespace Kryptonite.Types;

public class Instance
{
    public Instance(string name, GameVersion version, long _default)
    {
        Name = name;
        Version = version;
        Default = _default;
    }

    /// <summary>
    ///     The name of the instance
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     The version of the instance
    /// </summary>
    public GameVersion Version { get; }

    public long Default { get; }

    public static Task Launch(Instance instance, string? args = null)
    {
        if (instance == null) throw new Exception("Instance does not exist");
        args ??= "-vrmode oculus";

        var p = new Process
        {
            StartInfo = new ProcessStartInfo(
                $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\Kryptonite\\Instances\\{instance.Name}\\Beat Saber\\Beat Saber.exe",
                args)
            {
                UseShellExecute = false,
                WorkingDirectory =
                    $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\Kryptonite\\Instances\\{instance.Name}\\Beat Saber"
            }
        };
        try
        {
            p.StartInfo.Environment["SteamAppId"] = "620980";
            p.Start();
        }
        catch (Exception e)
        {
            Terminal.Log($"Failed to launch {instance.Name}", true, true);
        }

        return Task.CompletedTask;
    }
}