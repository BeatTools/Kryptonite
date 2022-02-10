using Kryptonite.Utils;

namespace Kryptonite;

internal static class Initialize
{
    public static void Start()
    {
        var globalStorage = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var storage = Path.Combine(globalStorage, "Kryptonite");
        var instancesDirectory = Path.Combine(storage + "\\Instances");

        Terminal.Log("Checking for AppData folder...");
        Directory.CreateDirectory(storage);

        Terminal.Log("Checking for instances folder...");
        Directory.CreateDirectory(instancesDirectory);

        Database.Initialize();
    }
}