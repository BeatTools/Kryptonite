using Kryptonite.Types;
using Kryptonite.Utils;

namespace Kryptonite.Managers;

/// <summary>
///     Manages all <see cref="GameVersion" /> objects.
/// </summary>
public interface VersionManager
{
    /// <summary>
    ///     Returns the <see cref="GameVersion" /> object for the given version.
    /// </summary>
    /// <param name="v">The version to get the <see cref="GameVersion" /> object for.</param>
    /// <returns>The <see cref="GameVersion" /> object for the given version.</returns>
    public static GameVersion? Get(string v)
    {
        var dbVersion = DatabaseManager.Versions.Get(v);

        if (dbVersion == null) return null;

        return new GameVersion(dbVersion["version"].ToString()!,
            dbVersion["manifest"].ToString()!);
    }

    /// <summary>
    ///     Returns a list of all <see cref="GameVersion" /> objects.
    /// </summary>
    /// <returns><see cref="List{GameVersion}" /> of all <see cref="GameVersion" /> objects.</returns>
    /// <exception cref="Exception">Thrown if there are no versions in the database.</exception>
    public static List<GameVersion>? List()
    {
        var versions = DatabaseManager.Versions.List();
        
        if (versions == null || versions.Count == 0) return null;

        return versions.Select(version => new GameVersion(version["version"].ToString()!,
            version["manifest"].ToString()!)).ToList();
    }
}