namespace Kryptonite.Types;

public class GameVersion
{
    public GameVersion(string version, string manifest)
    {
        Version = version;
        Manifest = manifest;
    }

    public string Version { get; }
    public string Manifest { get; }
}