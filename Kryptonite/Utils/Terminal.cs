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
        Console.Write("> ");
        Console.ForegroundColor = ConsoleColor.Blue;
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
        if (!password)
            throw new ArgumentException("Password-less password prompt is not supported...?", nameof(password));
        if (clear) Reset();

        var pass = new SecureString();
        WriteLine(prompt);
        ConsoleKeyInfo key;

        Console.Write("> ");
        Console.ForegroundColor = ConsoleColor.Blue;

        do
        {
            key = Console.ReadKey(true);

            switch (char.IsControl(key.KeyChar))
            {
                case false:
                    pass.AppendChar(key.KeyChar);
                    Write("*");
                    break;
                default:
                {
                    if (key.Key != ConsoleKey.Backspace || pass.Length <= 0) continue;
                    pass.RemoveAt(pass.Length - 1);
                    Write("\b \b");
                    break;
                }
            }
        } while (key.Key != ConsoleKey.Enter);

        Write("\n");
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