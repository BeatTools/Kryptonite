namespace Kryptonite.Types;

public class Instance
{
    public Instance(string name, string version)
    {
        Name = name;
        Version = version;
    }

    /// <summary>
    ///     The name of the instance
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     The version of the instance
    /// </summary>
    public string Version { get; }

    /// <summary>
    /// Starts the instance
    /// </summary>
    ///  <param name="instance">The instance to start</param>
    ///  <returns>The result of the operation</returns>
    ///  <exception cref="Kryptonite.Exceptions.KryptoniteException">Thrown when the instance is not found</exception>
}