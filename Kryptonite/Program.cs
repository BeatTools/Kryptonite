using Kryptonite.Utils;

namespace Kryptonite;

internal static class Program
{
    private static void Main()
    {
        Initialize.Start(); // Start

        while (true) Prompts.MainMenu();
    }
}