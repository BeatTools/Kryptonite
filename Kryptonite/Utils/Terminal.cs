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
        Console.ResetColor();
        return key ? Console.ReadKey().Key.ToString() : Console.ReadLine();
        
    }

    public static void Log(string s, bool clear = false)
    {
        if (clear) Reset();
        WriteLine(s);
    }

    public static string? Prompt(string prompt, bool clear = false)
    {
        if (string.IsNullOrWhiteSpace(prompt)) return null;
        if (clear) Reset();

        WriteLine(prompt);
        Write("> ");
        Console.ForegroundColor = ConsoleColor.Blue;
        var input = Read();
        return input;
    }

    private static void Reset()
    {
        Console.ResetColor();
        Console.Clear();
        Logo();
    }
}