using Kryptonite.Types;

namespace Kryptonite.Utils;

public static class Prompts
{
    public static void Menu()
    {
        while (true)
        {
            Terminal.Log("Welcome to Kryptonite, a multi-instance BeatSaber versioning system.", true);

            Terminal.Log("1. Create a new instance");
            Terminal.Log("2. Delete an instance");
            Terminal.Log("3. Update an instance - (Not implemented yet)");
            Terminal.Log("4. Play an instance");
            Terminal.Log("5. Exit");
            var selection = Terminal.Prompt("Please select an option.");

            switch (selection)
            {
                case "1":
                    CreateInstance();
                    break;
                
                case "2":
                    DeleteInstance();
                    break;
                
                case "3":
                    // UpdateInstance();
                    Terminal.Prompt("Not implemented yet. Please select another option.", true);
                    break;
                
                case "4":
                    PlayInstance();
                    break;
                
                case "5":
                    Terminal.Log("Exiting...", true);
                    Environment.Exit(0);
                    break;

                default:
                    Terminal.Log("Invalid selection");
                    break;
            }

            break;
        }
    }

    private static void CreateInstance()
    {
        while (true)
        {
            var name = Terminal.Prompt("Please enter a name for your new instance.");
            var safe_name = name.Replace(" ", "_").ToLower().Trim();

            if (string.IsNullOrWhiteSpace(safe_name))
            {
                Terminal.Prompt("Invalid name, press enter to try again.");
                continue;
            }

            switch (safe_name.Length)
            {
                case > 20:
                    Terminal.Prompt("Name is too long, press enter to try again.");
                    continue;
                case < 3:
                    Terminal.Prompt("Name is too short, press enter to try again.");
                    continue;
            }

            if (Database.GetInstance(safe_name) != null)
            {
                Terminal.Prompt("Instance already exists, press enter to try again.");
                continue;
            }

            var version = Terminal.Prompt("Please enter the version of your BeatSaber instance.");
            if (Database.GetVersion(version) == null)
            {
                Terminal.Prompt("Version does not exist, press enter to try again.");
                continue;
            }

            var confirm =
                Terminal.Prompt($"You are creating a new instance named {name} (v{version}). Is this correct? (y/n)");

            switch (confirm.ToLower())
            {
                case "y":
                    Instance.Create(safe_name, version);
                    Terminal.Prompt($"Instance {name} (v{version}) created. Press enter to return to the main menu.");
                    break;
                default:
                    Terminal.Prompt("Instance creation cancelled, press enter to try again.");
                    continue;
            }

            break;
        }
    }
    
    private static void DeleteInstance()
    {
        while (true)
        {
            var instances = Database.ListInstances();
            Terminal.Log("Available instances:");
            foreach (var i in instances)
            {
                Terminal.Log($"{i.name} (v{i.version})");
            }
            
            var name = Terminal.Prompt("Please enter the name of the instance you wish to delete.");
            var safe_name = name.Replace(" ", "_").ToLower().Trim();

            if (string.IsNullOrWhiteSpace(safe_name))
            {
                Terminal.Prompt("Invalid name, press enter to try again.");
                continue;
            }

            var instance = Database.GetInstance(safe_name);
            if (instance == null)
            {
                Terminal.Prompt("Instance does not exist, press enter to try again.");
                continue;
            }

            var confirm =
                Terminal.Prompt($"You are deleting instance {name}. Is this correct? (y/n)");

            switch (confirm.ToLower())
            {
                case "y":
                    Instance.Delete(safe_name);
                    Terminal.Prompt($"Instance {name} deleted. Press enter to return to the main menu.");
                    break;
                default:
                    Terminal.Prompt("Instance deletion cancelled, press enter to try again.");
                    continue;
            }

            break;
        }
    }

    private static void PlayInstance()
    {
        while (true)
        {
            var instances = Database.ListInstances();
            if (instances.Count == 0)
            {
                Terminal.Prompt("No instances found, press enter to try again.");
                continue;
            }

            Terminal.Log("Available instances:");
            foreach (var i in instances)
            {
                Terminal.Log($"{i.name} (v{i.version})");
            }
            
            var name = Terminal.Prompt("Please enter the name of the instance you wish to play.");
            var safe_name = name.Replace(" ", "_").ToLower().Trim();
            
            if (string.IsNullOrWhiteSpace(safe_name))
            {
                Terminal.Prompt("Invalid name, press enter to try again.");
                continue;
            }
            
            var instance = Database.GetInstance(safe_name);
            
            if (instance == null)
            {
                Terminal.Prompt("Instance does not exist, press enter to try again.");
                continue;
            }
            
            var confirm =
                Terminal.Prompt($"You are playing instance {name}. Is this correct? (y/n)");
            
            switch (confirm.ToLower())
            {
                case "y":
                    Launcher.Play(instance);
                    Terminal.Prompt($"Instance {name} playing. Press enter to return to the main menu.");
                    break;
                default:
                    Terminal.Prompt("Instance play cancelled, press enter to try again.");
                    continue;
            }
            
            
        }
    }
}