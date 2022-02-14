namespace Kryptonite.Utils;

internal static class Initialize
{
    public static void Start()
    {
        var globalStorage = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var storage = Path.Combine(globalStorage, "Kryptonite");
        var instancesDirectory = Path.Combine(storage + "\\Instances");

        Terminal.Log("[-] Initializing Kryptonite...");

        Terminal.Log("[-] Checking for AppData folder...");
        Directory.CreateDirectory(storage);

        Terminal.Log("[-] Checking for instances folder...");
        Directory.CreateDirectory(instancesDirectory);

        Terminal.Log("[-] Initializing DatabaseManager (this might take a second)...");
        DatabaseManager.Initialize();
    }
}