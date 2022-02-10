using Kryptonite.Utils;

namespace Kryptonite.Types;

public class Instance
{
    public Instance(string? name, string? version)
    {
        this.name = name;
        this.version = version;
    }

    public string? name { get; init; }
    public string? version { get; init; }

    public static string ConnUri { get; } =
        $"Data Source = {Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Kryptonite\\storage.db";

    public static Instance Create(string? name, string? version)
    {
        if (Database.GetInstance(name) != null) throw new Exception("Instance already exists");

        var instance = Database.CreateInstance(name, version);

        var instancePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Kryptonite", "instances", name);
        Directory.CreateDirectory(instancePath);

        return instance;
    }

    public static void Delete(string? name)
    {
        var instance = Database.GetInstance(name);
        if (instance == null) throw new Exception("Instance does not exist");

        Database.DeleteInstance(name);

        var instancePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Kryptonite", "instances", name);
        Directory.Delete(instancePath, true);
    }
}