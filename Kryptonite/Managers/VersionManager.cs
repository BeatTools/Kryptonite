using Kryptonite.Types;
using Kryptonite.Types.Exceptions;
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
    /// <param name="version">The version to get the <see cref="GameVersion" /> object for.</param>
    /// <returns>The <see cref="GameVersion" /> object for the given version.</returns>
    public static GameVersion GetVersion(string version)
    {
        var dbVersion = DatabaseManager.GetVersion(version);
        if (dbVersion == null) throw new KryptoniteException("Version not found");

        return new GameVersion(dbVersion["version"], dbVersion["manifest"]);
    }

    /// <summary>
    ///     Returns a list of all <see cref="GameVersion" /> objects.
    /// </summary>
    /// <returns><see cref="List{GameVersion}" /> of all <see cref="GameVersion" /> objects.</returns>
    /// <exception cref="Exception">Thrown if there are no versions in the database.</exception>
    public static List<GameVersion> ListVersions()
    {
        var versions = DatabaseManager.ListVersions();
        if (versions == null) throw new KryptoniteException("No versions found in database");

        return (from version in versions
            select new GameVersion(version["version"], version["manifest"])).ToList();
    }
}