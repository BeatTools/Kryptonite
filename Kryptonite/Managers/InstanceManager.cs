using System.Text.RegularExpressions;
using Kryptonite.Types;
using Kryptonite.Types.Exceptions;
using Kryptonite.Utils;

namespace Kryptonite.Managers;

/// <summary>
///     This class is responsible for managing all instances of the
///     <see cref="Kryptonite.Types.Instance" /> class.
/// </summary>
public interface InstanceManager
{
    private static readonly string InstancePath =
        $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Kryptonite\\Instances\\";

    /// <summary>
    ///     This method is responsible for sanitizing the name of an instance.
    /// </summary>
    /// <param name="name">The name of the instance to be sanitized.</param>
    /// <returns>The sanitized name of the instance.</returns>
    private static string SafeInstanceName(string name)
    {
        return Regex.Replace(name.ToLower(), @"[^a-zA-Z0-9]", "");
    }
    
    public static string TestInstanceName(string name)
    {
        return SafeInstanceName(name);
    }

    /// <summary>
    ///     Creates a new instance.
    /// </summary>
    /// <param name="name">The name of the instance.</param>
    /// <param name="version">The version of the instance.</param>
    /// <returns>The created instance.</returns>
    public static Instance CreateInstance(string name, string version)
    {
        name = SafeInstanceName(name);
        if (DatabaseManager.GetInstance(name) != null) throw new KryptoniteException("Instance already exists");
        var instancePath = $"{InstancePath}{name}";
        if (!Directory.Exists(instancePath)) Directory.CreateDirectory(instancePath);

        var instance = DatabaseManager.CreateInstance(name, version);
        DownloadClient.Download(
        return new Instance(instance["name"], instance["version"]);
    }

    /// <summary>
    ///     Gets an instance by name.
    /// </summary>
    /// <param name="name">The name of the instance.</param>
    /// <returns>The instance.</returns>
    public static Instance GetInstance(string name)
    {
        var instance = DatabaseManager.GetInstance(name);
        if (instance == null) throw new KryptoniteException("Instance does not exist");

        return new Instance(instance["name"], instance["version"]);
    }

    /// <summary>
    ///     Sets the default instance.
    /// </summary>
    /// <returns>The default instance.</returns>
    /// <exception cref="Exception">Thrown when the specified instance does not exist.</exception>
    public static Instance SetDefaultInstance()
    {
        var instance = DatabaseManager.GetDefaultInstance();
        if (instance == null) throw new KryptoniteException("Instance does not exist");

        return new Instance(instance["name"], instance["version"]);
    }

    /// <summary>
    ///     Gets the default instance.
    /// </summary>
    /// <returns>The default instance.</returns>
    /// <exception cref="Exception">Thrown when the specified instance does not exist.</exception>
    public static Instance GetDefaultInstance()
    {
        var instance = DatabaseManager.GetDefaultInstance();
        if (instance == null) throw new KryptoniteException("Instance does not exist");

        return new Instance(instance["name"], instance["version"]);
    }

    /// <summary>
    ///     Gets all instances.
    /// </summary>
    /// <returns>A List of all instances.</returns>
    public static List<Instance> ListInstances()
    {
        var instances = DatabaseManager.ListInstances();
        return instances.Select(instance => new Instance(instance["name"], instance["version"])).ToList();
    }

    /// <summary>
    ///     Deletes an instance.
    /// </summary>
    /// <param name="name">The name of the instance.</param>
    /// <returns></returns>
    /// <exception cref="Exception">Instance does not exist</exception>
    public static bool DeleteInstance(string name)
    {
        var instance = DatabaseManager.GetInstance(name);
        if (instance == null) throw new KryptoniteException("Instance does not exist");
        var instancePath = $"{InstancePath}{name}";
        if (Directory.Exists(instancePath)) Directory.Delete(instancePath, true);

        return DatabaseManager.DeleteInstance(name);
    }
}