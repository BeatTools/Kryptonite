using System.Data;
using System.Data.SQLite;
using System.Net;
using Kryptonite.Types;
using Kryptonite.Types.Exceptions;

namespace Kryptonite.Utils;

internal static class DatabaseManager
{
    private static readonly string ConnUri =
        $"Data Source = {Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Kryptonite\\storage.db";

    /// <summary>
    ///     Executes a query on the database.
    /// </summary>
    /// <param name="query">The query to execute.</param>
    /// <param name="args">The arguments to pass to the query.</param>
    /// <returns>The amount of rows affected.</returns>
    private static int ExecuteWrite(string query, Dictionary<string, object>? args = null)
    {
        using var connection = new SQLiteConnection(ConnUri);
        connection.Open();

        using var command = new SQLiteCommand(query, connection);
        args ??= new Dictionary<string, object>();
        foreach (var (key, value) in args) command.Parameters.AddWithValue(key, value);

        return command.ExecuteNonQuery();
    }

    /// <summary>
    ///     Execute a query and return the result as a DataTable.
    /// </summary>
    /// <param name="query">The query to execute.</param>
    /// <param name="args">The arguments to pass to the query.</param>
    /// <returns>The result of the query as a DataTable.</returns>
    private static DataTable? Execute(string query, Dictionary<string, object>? args = null)
    {
        if (string.IsNullOrWhiteSpace(query)) return null;

        using var connection = new SQLiteConnection(ConnUri);
        connection.Open();

        using var command = new SQLiteCommand(query, connection);
        args ??= new Dictionary<string, object>();
        foreach (var (key, value) in args) command.Parameters.AddWithValue(key, value);

        var dataAdapter = new SQLiteDataAdapter(command);
        var dataTable = new DataTable();

        dataAdapter.Fill(dataTable);

        dataAdapter.Dispose();
        return dataTable;
    }

    public static async void Initialize()
    {
        Terminal.Log("[-] Dropping versions table...");
        ExecuteWrite("drop table if exists versions");
        Terminal.Log("[-] Creating instances table...");
        ExecuteWrite(@"create table if not exists instances
        (
            name      TEXT not null,
            version   TEXT not null,
            _default integer default 0 not null
        );");

        Terminal.Log("[-] Creating versions table...");
        ExecuteWrite(@"create table if not exists versions
        (
            version  TEXT not null
                constraint versions_pk
                    primary key,
            manifest TEXT not null
        );");

        Terminal.Log("[-] Getting versions...");
        using var client = new HttpClient();
        using var versions =
            await client.GetAsync(
                "https://gist.githubusercontent.com/ChecksumDev/ca69ccd781e37f3e5a2afe9e2bb1ed69/raw/270eed764c897ba46d4db17961b05910db090a3f/beatsaber_versions.sql");
        var versionsSql = await versions.Content.ReadAsStringAsync();

        if (versions.StatusCode == HttpStatusCode.OK && versionsSql.StartsWith("INSERT INTO"))
            ExecuteWrite(versionsSql);
        else
            throw new KryptoniteException("Failed to download versions table from GitHub.");
    }

    // Versions
    public static List<Dictionary<string, string>>? ListVersions()
    {
        const string query = "SELECT * FROM versions";

        var dataTable = Execute(query);
        if (dataTable == null) throw new KryptoniteException("Failed to get versions.");

        var versions = (from DataRow row in dataTable.Rows
            select new Dictionary<string, string>
                {{"version", row["version"].ToString()}, {"manifest", row["manifest"].ToString()}}).ToList();
    }

    public static Dictionary<string, string>? GetVersion(string version)
    {
        const string query = "SELECT * FROM versions WHERE version = $version";

        var args = new Dictionary<string, object> {{"$version", version}};
        var dataTable = Execute(query, args);

        if (dataTable == null || dataTable.Rows.Count == 0) return null;

        var row = dataTable.Rows[0];
        return new Dictionary<string, string>
        {
            {"version", row["version"].ToString()},
            {"manifest", row["manifest"].ToString()}
        };
    }

    // Instances
    public static List<Dictionary<string, string>>? ListInstances()
    {
        const string query = "SELECT * FROM instances";

        var dataTable = Execute(query);
        if (dataTable == null || dataTable.Rows.Count == 0) return null;

        return (from DataRow dataRow in dataTable.Rows
            select new Dictionary<string, string>
            {
                {"name", dataRow["name"].ToString()},
                {"version", dataRow["version"].ToString()},
                {"default", dataRow["_default"].ToString()}
            }).ToList();
    }

    public static Dictionary<string, string> CreateInstance(string name, string version)
    {
        const string query = "insert into instances (name, version) values ($name, $version);";

        var args = new Dictionary<string, object> {{"$name", name}, {"$version", version}};

        var result = ExecuteWrite(query, args);
        if (result == 0) throw new KryptoniteException("Failed to create instance");

        return new Dictionary<string, string> {{"name", name}, {"version", version}};
    }

    public static Dictionary<string, string>? GetInstance(string name)
    {
        const string query = "SELECT name, version FROM instances WHERE name = $name";

        var args = new Dictionary<string, object> {{"$name", name}};

        var dataTable = Execute(query, args);

        if (dataTable == null || dataTable.Rows.Count == 0) return null;

        return new Dictionary<string, string>
        {
            {"name", dataTable.Rows[0].Field<string>("name")!}, {"version", dataTable.Rows[0].Field<string>("version")!}
        };
    }

    public static int SetDefaultInstance(Instance instance)
    {
        // There can only be one default instance, so we need to unset the current default if there is one
        const string query = "UPDATE instances SET _default = 0 WHERE _default = 1";
        ExecuteWrite(query);

        // Set the new default
        const string query2 = "UPDATE instances SET _default = 1 WHERE name = $name";
        var args = new Dictionary<string, object> {{"$name", instance.Name}};

        return ExecuteWrite(query2, args);
    }

    public static Dictionary<string, string>? GetDefaultInstance()
    {
        const string query = "SELECT name, version FROM instances WHERE _default = 1";
        var dataTable = Execute(query);

        if (dataTable == null || dataTable.Rows.Count == 0) return null;
        return new Dictionary<string, string>
        {
            {"name", dataTable.Rows[0].Field<string>("name")!},
            {"version", dataTable.Rows[0].Field<string>("version")!}
        };
    }

    public static bool DeleteInstance(string name)
    {
        const string query = "DELETE FROM instances WHERE name = $name";

        var args = new Dictionary<string, object>
        {
            {"$name", name}
        };

        var result = ExecuteWrite(query, args);
        return result != 0;
    }
}