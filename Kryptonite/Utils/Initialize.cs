using Kryptonite.Managers;

namespace Kryptonite.Utils;

internal static class Initialize
{
    public static async Task Start()
    {
        var globalStorage = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var storage = Path.Combine(globalStorage, "Kryptonite");
        var downloadsDirectory = Path.Combine(storage, "Downloads");
        var instancesDirectory = Path.Combine(storage + "\\Instances");

        Terminal.Log("[-] Initializing Kryptonite...");

        Terminal.Log("[-] Cleaning up old Kryptonite folders...");
        if (Directory.Exists(downloadsDirectory)) Directory.Delete(downloadsDirectory, true);
        Directory.CreateDirectory(downloadsDirectory);

        Terminal.Log("[-] Checking for AppData folder...");
        Directory.CreateDirectory(storage);

        Terminal.Log("[-] Checking for instances folder...");
        Directory.CreateDirectory(instancesDirectory);

        Terminal.Log("[-] Initializing DatabaseManager (this might take a second)...");
        await DatabaseManager.Initialize();

        Terminal.Log("[+] Kryptonite is ready to go!", true);
    }
}