using Kryptonite.Utils;

namespace Kryptonite;

internal static class Program
{
    private static void Main()
    {
        Initialize.Start(); // Start
        
        // Main loop
        while (true) Prompts.MainMenu();
    }
}