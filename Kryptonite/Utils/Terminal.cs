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