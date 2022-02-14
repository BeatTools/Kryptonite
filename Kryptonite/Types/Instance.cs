namespace Kryptonite.Types;

public class Instance
{
    public Instance(string name, GameVersion version, int _default)
    {
        Name = name;
        Version = version;
        Default = _default;
    }

    /// <summary>
    ///     The name of the instance
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     The version of the instance
    /// </summary>
    public GameVersion Version { get; }
    
    public int Default { get; }
}