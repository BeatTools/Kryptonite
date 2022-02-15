using System.Text.RegularExpressions;
using Kryptonite.Types;
using Kryptonite.Utils;

namespace Kryptonite.Managers;

/// <summary>
///     This class is responsible for managing all instances of the
///     <see cref="Kryptonite.Types.Instance" /> class.
/// </summary>
public interface InstanceManager
{
    private static readonly string InstancePath =
        $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\Kryptonite\\Instances\\";

    /// <summary>
    ///     This method is responsible for sanitizing the name of an instance.
    /// </summary>
    /// <param name="name">The name of the instance to be sanitized.</param>
    /// <returns>The sanitized name of the instance.</returns>
    private static string SafeInstanceName(string name)
    {
        return Regex.Replace(name.ToLower(), @"[^a-zA-Z0-9]", "");
    }

    /// <summary>
    ///     Creates a new instance.
    /// </summary>
    /// <param name="name">The name of the instance.</param>
    /// <param name="version">The version of the instance.</param>
    /// <returns>The created instance.</returns>
    public static Instance? Create(string name, string version)
    {
        name = SafeInstanceName(name);

        if (string.IsNullOrWhiteSpace(name)) return null;

        var dbInstance = DatabaseManager.Instances.Get(name);
        if (dbInstance != null) return null;

        var dbVersion = VersionManager.Get(version);
        if (dbVersion == null) return null;

        try
        {
            DatabaseManager.Instances.Create(name, version);
        }
        catch (Exception e)
        {
            Terminal.Log($"Failed to create instance: {e.Message}");
            throw;
        }

        var instance = new Instance(name, dbVersion, 0);
        return instance;
    }

    public static Instance? Update(string name, string version)
    {
        name = SafeInstanceName(name);

        if (string.IsNullOrWhiteSpace(name)) return null;

        var dbInstance = DatabaseManager.Instances.Get(name);
        if (dbInstance == null) return null;

        var dbVersion = VersionManager.Get(version);
        if (dbVersion == null) return null;

        try
        {
            DatabaseManager.Instances.Update(name, version);
        }
        catch (Exception e)
        {
            Terminal.Log($"Failed to update instance: {e.Message}", hold: true);
            throw;
        }

        var instance = new Instance(name, dbVersion, (long) dbInstance["default"]);
        return instance;
    }

    /// <summary>
    ///     Gets an instance by name.
    /// </summary>
    /// <param name="name">The name of the instance.</param>
    /// <returns>The instance.</returns>
    public static Instance? Get(string name)
    {
        name = SafeInstanceName(name);

        var dbInstance = DatabaseManager.Instances.Get(name);
        if (dbInstance == null) return null;

        var dbVersion = DatabaseManager.Versions.Get(dbInstance["version"].ToString()!);
        if (dbVersion == null) return null;

        var version = new GameVersion(dbVersion["version"].ToString()!, dbVersion["manifest"].ToString()!);
        var instance = new Instance(dbInstance["name"].ToString()!, version, (int) dbInstance["default"]);

        return instance;
    }

    /// <summary>
    ///     Sets the default instance.
    /// </summary>
    /// <returns>The default instance.</returns>
    /// <exception cref="Exception">Thrown when the specified instance does not exist.</exception>
    public static bool SetDefault(string name)
    {
        var instance = Get(name);
        if (instance == null) throw new Exception("The specified instance does not exist.");

        return true;
    }

    /// <summary>
    ///     Gets the default instance.
    /// </summary>
    /// <returns>The default instance.</returns>
    /// <exception cref="Exception">Thrown when the specified instance does not exist.</exception>
    public static Instance? GetDefault()
    {
        var dbInstance = DatabaseManager.Instances.GetDefault();
        if (dbInstance == null) return null;

        var dbVersion = DatabaseManager.Versions.Get(dbInstance["version"].ToString()!);
        if (dbVersion == null) return null;

        var version = new GameVersion(dbVersion["version"].ToString()!, dbVersion["manifest"].ToString()!);
        var instance = new Instance(dbInstance["name"].ToString()!, version, (int) dbInstance["default"]);

        return instance;
    }

    /// <summary>
    ///     Gets all instances.
    /// </summary>
    /// <returns>A List of all instances.</returns>
    public static List<Instance>? List()
    {
        var dbInstances = DatabaseManager.Instances.List();
        return dbInstances == null
            ? null
            : (from dbInstance in dbInstances
                let dbVersion = DatabaseManager.Versions.Get(dbInstance["version"].ToString()!)
                where dbVersion != null
                let version = new GameVersion(dbVersion["version"].ToString()!, dbVersion["manifest"].ToString()!)
                select new Instance(dbInstance["name"].ToString()!, version, (long) dbInstance["default"])).ToList();
    }

    /// <summary>
    ///     Deletes an instance.
    /// </summary>
    /// <param name="name">The name of the instance.</param>
    /// <returns></returns>
    /// <exception cref="Exception">Instance does not exist</exception>
    public static bool Delete(string name)
    {
        name = SafeInstanceName(name);

        var dbInstance = DatabaseManager.Instances.Get(name);
        if (dbInstance == null) throw new Exception("Instance does not exist");

        return DatabaseManager.Instances.Delete(name);
    }
}