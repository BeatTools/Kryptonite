using System.Diagnostics;
using Kryptonite.Utils;

namespace Kryptonite.Types;

public class Instance
{
    public Instance(string name, string version)
    {
        Name = name;
        Version = version;
    }

    public string Name { get; }
    public string Version { get; }

    private readonly string _path =
        $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Kryptonite\\Instances";

    public static Instance Create(string name, string version)
    {
        if (Database.GetInstance(name) != null) throw new Exception("Instance already exists");

        var instance = Database.CreateInstance(name, version);
        Directory.CreateDirectory($"{instance._path}\\{name}");

        return instance;
    }

    public static void Delete(string name)
    {
        var instance = Database.GetInstance(name);
        if (instance == null) throw new Exception("Instance does not exist");

        try
        {
            Database.DeleteInstance(name);
            Directory.Delete($"{instance._path}\\{name}", true);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static void Launch(Instance instance, string? args = null)
    {
        if (instance == null) throw new Exception("Instance does not exist");
        args ??= "";

        var p = new Process
        {
            StartInfo = new ProcessStartInfo($"{instance._path}\\{instance.Name}\\Beat Saber\\Beat Saber.exe", args)
            {
                UseShellExecute = false,
                WorkingDirectory = $"{instance._path}\\{instance.Name}\\Beat Saber\\{instance.Name}",
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