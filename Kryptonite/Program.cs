using Kryptonite.Utils;

namespace Kryptonite;

internal static class Program
{
    private static async Task Main()
    {
        await Initialize.Start();
        while (true) Prompts.MainMenu();
    }
}