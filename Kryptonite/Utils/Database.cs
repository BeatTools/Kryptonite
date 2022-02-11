﻿using System.Data;
using System.Data.SQLite;
using Kryptonite.Types;
using Version = Kryptonite.Types.Version;

namespace Kryptonite.Utils;

internal static class Database
{
    private static readonly string ConnUri =
        $"Data Source = {Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Kryptonite\\storage.db";

    private static int ExecuteWrite(string query, Dictionary<string, object>? args = null)
    {
        using var connection = new SQLiteConnection(ConnUri);
        connection.Open();

        using var command = new SQLiteCommand(query, connection);
        args ??= new Dictionary<string, object>();
        foreach (var (key, value) in args) command.Parameters.AddWithValue(key, value);

        return command.ExecuteNonQuery();
    }

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
        ExecuteWrite("drop table if exists versions");
        ExecuteWrite(@"create table if not exists instances
        (
            name      TEXT not null,
            version   TEXT not null,
            _default integer default 0 not null
        );");

        ExecuteWrite(@"create table if not exists versions
        (
            version  TEXT not null
                constraint versions_pk
                    primary key,
            manifest TEXT not null
        );");

        using var client = new HttpClient();
        // Download the SQL for the versions table and execute it (This is to ensure that the versions table is always up to date)
        var versions =
            await client.GetAsync(
                "https://gist.githubusercontent.com/ChecksumDev/ca69ccd781e37f3e5a2afe9e2bb1ed69/raw/270eed764c897ba46d4db17961b05910db090a3f/beatsaber_versions.sql");
        var versionsSql = await versions.Content.ReadAsStringAsync();

        if (versionsSql.StartsWith("INSERT INTO"))
            ExecuteWrite(versionsSql);
        else
            throw new Exception("Failed to download versions table");
    }

    public static List<Version>? ListVersions()
    {
        const string query = "SELECT * FROM versions";

        var dataTable = Execute(query);
        if (dataTable == null || dataTable.Rows.Count == 0) return null;

        return (from DataRow dataRow in dataTable.Rows
            select new Version(dataRow["version"].ToString()!, dataRow["manifest"].ToString()!)).ToList();
    }

    public static Version? GetVersion(string version)
    {
        const string query = "SELECT * FROM versions WHERE version = $version";

        var args = new Dictionary<string, object>
        {
            {"$version", version}
        };

        var dataTable = Execute(query, args);

        if (dataTable == null || dataTable.Rows.Count == 0) return null;

        return new Version(dataTable.Rows[0].Field<string>("version")!,
            dataTable.Rows[0].Field<string>("manifest")!);
    }

    // Instances
    public static List<Instance>? ListInstances()
    {
        const string query = "SELECT * FROM instances";

        var dataTable = Execute(query);
        if (dataTable == null || dataTable.Rows.Count == 0) return null;

        return dataTable.AsEnumerable()
            .Select(row => new Instance(row.Field<string>("name"), row.Field<string>("version")))
            .ToList();
    }

    public static Instance CreateInstance(string name, string version)
    {
        const string query = "insert into instances (name, version) values ($name, $version);";

        var args = new Dictionary<string, object>
        {
            {"$name", name}, {"$version", version}
        };

        var result = ExecuteWrite(query, args);
        if (result == 0) throw new Exception("Failed to create instance");

        return new Instance(name, version);
    }

    public static Instance? GetInstance(string name)
    {
        const string query = "SELECT name, version FROM instances WHERE name = $name";

        var args = new Dictionary<string, object>
        {
            {"$name", name}
        };

        var dataTable = Execute(query, args);

        if (dataTable == null || dataTable.Rows.Count == 0) return null;
        return new Instance(
            dataTable.Rows[0].Field<string>("name")!,
            dataTable.Rows[0].Field<string>("version")!
        );
    }

    public static int DeleteInstance(string name)
    {
        const string query = "DELETE FROM instances WHERE name = $name";

        var args = new Dictionary<string, object>
        {
            {"$name", name}
        };

        var result = ExecuteWrite(query, args);
        if (result == 0) throw new Exception("Failed to delete instance");

        return result;
    }

    public static int SetDefaultInstance(string name)
    {
        // There can only be one default instance, so we need to unset the current default if there is one
        const string query = "UPDATE instances SET _default = 0 WHERE _default = 1";
        ExecuteWrite(query);

        // Set the new default
        const string query2 = "UPDATE instances SET _default = 1 WHERE name = $name";
        var args = new Dictionary<string, object> {{"$name", name}};

        return ExecuteWrite(query2, args);
    }
}