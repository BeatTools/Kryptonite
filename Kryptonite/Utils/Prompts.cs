using System.Diagnostics;
using Kryptonite.Managers;
using Kryptonite.Types;
using Kryptonite.Types.Exceptions;

namespace Kryptonite.Utils;

internal static class Prompts
{
    public static void MainMenu()
    {
        Terminal.Log("Welcome to Kryptonite!",
            true);

        Terminal.Log(@"0. Quickstart default instance");
        Terminal.Log(@"1. Create a new instance for Beat Saber");
        Terminal.Log(@"2. Select an existing instance for Beat Saber");
        Terminal.Log(@"3. Exit");

        var option = Terminal.Prompt("Choose an option above: ");

        switch (option)
        {
            case "0":
                Quickstart();
                break;
            case "1":
                CreateInstance();
                break;
            case "2":
                SelectInstance();
                break;
            case "3":
                Terminal.Log("Exiting...");
                Environment.Exit(0);
                break;
            default:
                Terminal.Log("Invalid option. Please try again.");
                break;
        }
    }

    private static void Quickstart()
    {
    }

    private static User Login()
    {
        var username = Terminal.Prompt("Please enter your steam username: ");
        var password = Terminal.Prompt("Please enter your steam password (will not be displayed): ",
            password: true);

        if (string.IsNullOrWhiteSpace(username))
            throw new KryptoniteException("Username cannot be empty.");

        var user = new User(username,
            password);
        return user;
    }

    private static void CreateInstance()
    {
        var name = Terminal.Prompt("Please enter a name for your new instance: ");
        if (string.IsNullOrWhiteSpace(name))
            throw new KryptoniteException("Instance name cannot be empty.");

        if (InstanceManager.Get(name) != null)
            throw new KryptoniteException("Instance already exists.");

        var versions = VersionManager.List();
        foreach (var v in versions) Terminal.Log($"{v.Version}: {v.Manifest}");

        var version = Terminal.Prompt("Please enter the version of Beat Saber you want to use: ");
        if (string.IsNullOrWhiteSpace(version))
            throw new KryptoniteException("Version cannot be empty.");

        if (versions.All(v => v.Version != version))
            throw new KryptoniteException("Version does not exist.");

        var user = Login();

        try
        {
            DownloadClient.Download(name,
                versions.First(v => v.Version == version),
                user);
        }
        catch (Exception e)
        {
            Terminal.Log(e.Message);
            return;
        }

        InstanceManager.Create(name,
            version);
        Terminal.Log($"Successfully created instance {name}.\nPress any key to continue...",
            hold: true);
    }

    private static void SelectInstance()
    {
        var instances = InstanceManager.List();
        if (instances.Count == 0)
        {
            Terminal.Log("No instances found. Please create one first.",
                hold: true);
            return;
        }

        foreach (var i in instances) Terminal.Log($"{i.Name} ({i.Version.Version})");

        var name = Terminal.Prompt("Please enter the name of the instance you want to use: ");
        if (string.IsNullOrWhiteSpace(name))
            throw new KryptoniteException("Instance name cannot be empty.");

        if (instances.All(i => i.Name != name))
            throw new KryptoniteException("Instance does not exist.");

        var instance = instances.First(i => i.Name == name);

        InstanceMenu(instance);
    }

    private static async Task InstanceMenu(Instance instance)
    {
        while (true)
        {
            Terminal.Log($"Selected instance: {instance.Name}",
                true);

            Terminal.Log("1. Launch Beat Saber");
            Terminal.Log("2. Change the version of Beat Saber");
            Terminal.Log("3. Delete the instance");
            Terminal.Log("4. Open instance folder");
            Terminal.Log("5. Go back to the main menu");

            var option = Terminal.Prompt("Choose an option above: ");

            switch (option)
            {
                case "1": // Launch Beat Saber
                    await Instance.Launch(instance);
                    Terminal.Log("Successfully launched Beat Saber! Press enter to return to the main menu.",
                        hold: true);
                    break;


                case "2": // Change version
                    var versions = VersionManager.List();
                    foreach (var v in versions) Terminal.Log($"{v.Version}: {v.Manifest}");

                    var version =
                        Terminal.Prompt(
                            "Please enter the version of Beat Saber you want to change to, or type exit to go back: ");
                    if (string.IsNullOrWhiteSpace(version))
                        throw new KryptoniteException("Version cannot be empty.");
                    if (version == "exit")
                        break;

                    if (versions.All(v => v.Version != version))
                        throw new KryptoniteException("Version does not exist.");

                    var user = Login();

                    try
                    {
                        await DownloadClient.Download(instance.Name,
                            versions.First(v => v.Version == version),
                            user,
                            true);
                    }
                    catch (Exception e)
                    {
                        Terminal.Log(e.Message,
                            true);
                        continue;
                    }

                    InstanceManager.Update(instance.Name,
                        version);

                    Terminal.Log(
                        $"Successfully updated instance {instance.Name} to version {version}.\nPress any key to continue...",
                        hold: true);
                    MainMenu();
                    break;


                case "3": // Delete
                    var delete = Terminal.Prompt("Are you sure you want to delete this instance? (y/n): ");
                    delete ??= "n";

                    if (delete.ToLower() == "y")
                    {
                        InstanceManager.Delete(instance.Name);
                        Terminal.Log("Successfully deleted instance. Press enter to return to the main menu.",
                            hold: true);
                    }
                    else
                    {
                        Terminal.Log("Instance deletion cancelled. Press enter to return to the main menu.",
                            hold: true);
                    }

                    break;

                case "4":
                    Process.Start("explorer.exe",
                        $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}" +
                        $"\\Kryptonite\\Beat Saber\\Instances\\{instance.Name}");
                    continue;

                case "5":
                    break;

                default:
                    Terminal.Log("Invalid option. Press enter to return to the main menu.",
                        hold: true);
                    break;
            }

            break;
        }
    }
}