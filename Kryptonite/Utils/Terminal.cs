using System.Security;

namespace Kryptonite.Utils;

internal static class Terminal
{
    private static void Logo()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"██╗  ██╗██████╗ ██╗   ██╗██████╗ ████████╗ ██████╗ ███╗   ██╗██╗████████╗███████╗
██║ ██╔╝██╔══██╗╚██╗ ██╔╝██╔══██╗╚══██╔══╝██╔═══██╗████╗  ██║██║╚══██╔══╝██╔════╝
█████╔╝ ██████╔╝ ╚████╔╝ ██████╔╝   ██║   ██║   ██║██╔██╗ ██║██║   ██║   █████╗  
██╔═██╗ ██╔══██╗  ╚██╔╝  ██╔═══╝    ██║   ██║   ██║██║╚██╗██║██║   ██║   ██╔══╝  
██║  ██╗██║  ██║   ██║   ██║        ██║   ╚██████╔╝██║ ╚████║██║   ██║   ███████╗
╚═╝  ╚═╝╚═╝  ╚═╝   ╚═╝   ╚═╝        ╚═╝    ╚═════╝ ╚═╝  ╚═══╝╚═╝   ╚═╝   ╚══════╝");
    }

    private static void WriteLine(string s)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(s);
        Console.ResetColor();
    }

    private static void Write(string s)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(s);
        Console.ResetColor();
    }

    private static string? Read(bool key = false)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("> ");
        var input = key ? Console.ReadKey(true).Key.ToString() : Console.ReadLine();
        Console.ResetColor();
        return string.IsNullOrWhiteSpace(input) ? null : input;
    }

    public static void Log(string s, bool clear = false, bool hold = false)
    {
        if (clear) Reset();
        WriteLine(s);
        if (hold) Console.ReadKey(true);
    }

    public static SecureString? Prompt(string prompt, bool password, bool clear = false)
    {
        if (prompt == null) throw new ArgumentNullException(nameof(prompt));
        if (clear) Reset();
        if (!password) return null;
        
        var pass = new SecureString();
        Console.Write(prompt);
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            switch (char.IsControl(key.KeyChar))
            {
                case false:
                    pass.AppendChar(key.KeyChar);
                    Console.Write("*");
                    break;
                default:
                {
                    if (key.Key != ConsoleKey.Backspace || pass.Length <= 0) continue;
                    pass.RemoveAt(pass.Length - 1);
                    Console.Write("\b \b");
                    break;
                }
            }
        } while (key.Key != ConsoleKey.Enter);

        return pass;

    }


    public static string? Prompt(string prompt = "", bool clear = false)
    {
        if (prompt == null) throw new ArgumentNullException(nameof(prompt));
        if (clear) Reset();

        WriteLine(prompt);
        var input = Read();
        return input ?? null;
    }

    private static void Reset()
    {
        Console.ResetColor();
        Console.Clear();
        Logo();
    }
}