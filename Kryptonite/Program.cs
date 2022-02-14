using Kryptonite.Managers;
using Kryptonite.Utils;

namespace Kryptonite;

internal static class Program
{
    private static void Main()
    {
        Initialize.Start();
        var args = Environment.GetCommandLineArgs();
        // find -quickstart if it exists
        var quickstart = args.Contains("-quickstart");

        Terminal.Log(InstanceManager.TestInstanceName(
            "Kryptonite123423r1234jr24/rb243rtrK#!$FG#QRF@#$,/eMd23d,wsde2fmrp234jrt423h5rt24897et83t213WER@#R@#"));
        
        while (true) Prompts.MainMenu();
    }
}