namespace Kryptonite.Types;

public class GameVersion
{
    public GameVersion(string version, string manifest)
    {
        Version = version;
        Manifest = manifest;
    }

    /// <summary>
    ///     The version of Beat Saber.
    /// </summary>
    public string Version { get; }

    /// <summary>
    ///     The steam manifest hash for this version.
    /// </summary>
    public string Manifest { get; }
}