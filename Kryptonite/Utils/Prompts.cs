using Kryptonite.Types;

namespace Kryptonite.Utils;

internal static class Prompts
{
    public static void MainMenu()
    {
        Terminal.Log("Welcome to Kryptonite!", true);

        Terminal.Log(@"1. Create a new instance for Beat Saber");
        Terminal.Log(@"2. Select an existing instance for Beat Saber");
        Terminal.Log(@"3. Exit");

        var option = Terminal.Prompt("Choose an option above: ");

        switch (option)
        {
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

    private static void CreateInstance()
    {
        var name = Terminal.Prompt("Enter a name for the new instance: ");
        var version = Terminal.Prompt("Enter the version of Beat Saber to use: ");

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(version))
        {
            Terminal.Log("Invalid name or version entered! Press any key to return to the main menu.", hold: true);
            return;
        }

        if (Database.GetInstance($"{name}") != null)
        {
            Terminal.Log("An instance with that name already exists! Press any key to return to the main menu.",
                hold: true);
            return;
        }

        // Create the new instance
        try
        {
            Instance.Create(name, version);
            Terminal.Log($"Created instance {name} for Beat Saber {version}! Press any key to continue...", hold: true);
        }
        catch (Exception e)
        {
            Terminal.Log($"An error occurred while creating the instance: {e.Message}", hold: true);
        }
    }

    private static void SelectInstance()
    {
        var instances = Database.ListInstances();

        if (instances == null || instances.Count == 0)
        {
            Terminal.Log("No instances found!");
            return;
        }

        foreach (var instance in instances) Terminal.Log($"{instance.name}: {instance.version}");

        var name = Terminal.Prompt("Enter the name of the instance to select: ");

        if (name == null)
        {
            Terminal.Log("Invalid name entered! Press enter to return to the main menu.", hold: true);
            return;
        }

        var dbInstance = Database.GetInstance(name);

        if (dbInstance == null)
        {
            Terminal.Log("Instance not found! Press enter to return to the main menu.", hold: true);
            return;
        }

        InstanceMenu(dbInstance);
    }

    private static void InstanceMenu(Instance instance)
    {
        Terminal.Log($"Selected instance: {instance.name}", true);

        Terminal.Log(@"1. Launch Beat Saber");
        Terminal.Log(@"2. Change the version of Beat Saber");
        Terminal.Log(@"3. Delete the instance");
        Terminal.Log(@"4. Exit");

        var option = Terminal.Prompt("Choose an option above: ");

        switch (option)
        {
            case "1":
                Launcher.Play(instance);
                Terminal.Log("Successfully launched Beat Saber! Press enter to return to the main menu.", hold: true);
                break;
            case "2":
                Terminal.Log("Not implemented yet! Press enter to return to the main menu.", hold: true);
                break;
            case "3":
                var confirm =
                    Terminal.Prompt(
                        "Are you sure you want to delete this instance? This will delete all files associated with it. (y/n)");
                if (confirm == "y")
                {
                    Instance.Delete(instance.name);
                    Terminal.Log("Successfully deleted instance! Press enter to return to the main menu.", hold: true);
                }
                else
                {
                    Terminal.Log("Instance deletion cancelled. Press enter to return to the main menu.", hold: true);
                }

                break;
            case "4":
                Exit();
                break;
            default:
                Terminal.Log("Invalid option entered!", true);
                break;
        }
    }

    private static void Exit(int code = 0)
    {
        Environment.Exit(code);
    }
}