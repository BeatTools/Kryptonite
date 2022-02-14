using Kryptonite.Managers;
using Kryptonite.Utils;

namespace Kryptonite;

internal static class Program
{
    private static void Main()
    {
        Initialize.Start();
        while (true) Prompts.MainMenu();
    }
}