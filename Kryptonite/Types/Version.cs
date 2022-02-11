namespace Kryptonite.Types;

public class Version
{
    public Version(string version, string manifest)
    {
        this.version = version;
        this.manifest = manifest;
    }

    public string version { get; }
    public string manifest { get; }
}