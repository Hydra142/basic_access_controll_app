using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Dapper;
using SafeMessenge.Properties;
using SafeMessenge.Contracts.Services;

namespace SafeMessenge.Services;
public class SqliteConnectorService : ISqliteConnector
{
    const string FILENAME = "sqlite.db";
    readonly SqliteConnectionStringBuilder _connectionStringBuilder;

    public SqliteConnectorService()
    {

        _connectionStringBuilder = new()
        {
            DataSource = Path.Combine("D:\\LabsData\\TBD\\TBD_Redchych\\AppData", FILENAME),
            Mode = SqliteOpenMode.ReadWriteCreate,
            Cache = SqliteCacheMode.Default
        };
        InitDb();
    }

    private void InitDb()
    {
        using var con = new SqliteConnection(_connectionStringBuilder.ConnectionString);
        con.Open();
        var tablesCount = 0;
        try
        {

            var reader = con.ExecuteReader("SELECT count(name) FROM sqlite_schema where type='table'");
            reader.Read();
            tablesCount= reader.GetInt32(0);
        }
        catch { }
        finally
        {
            if (tablesCount == 0)
            {
                var command = new SqliteCommand(Resources.InsertTables, con);
                try
                {
                    command.ExecuteNonQuery();

                }
                catch (SqliteException e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }
        }
    }

    public async Task<IList<T>> Read<T>(string sql, object options)
    {
        using var connection = new SqliteConnection(_connectionStringBuilder.ConnectionString);
        connection.Open();
        return (await connection.QueryAsync<T>(sql, options)).ToList();
    }

    public Task<int> Write(string sql, object options)
    {
        using var connection = new SqliteConnection(_connectionStringBuilder.ConnectionString);
        connection.Open();
        return connection.ExecuteAsync(sql, options);
    }
}
