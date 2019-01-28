using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Galaxy
{
    public class DatabaseManager : IDisposable
    {
        public DirectoryInfo DatabasePath { get; private set; }
        public static long RowsAdded = 0;
        public SqlConnection Connection;
        public SqlBulkCopy Batch;
        public DataTable ImportDataTable;

        public DatabaseManager(string databasePath, string tableName)
        {
            if (!Directory.Exists(databasePath))
            {
                Directory.CreateDirectory(databasePath);
            }

            DatabasePath = new DirectoryInfo(databasePath);
            Connection = new SqlConnection(@"Data Source=(LocalDB)\EDMaster;Initial Catalog=EDSystems");
            Connection.Open();
            Batch = new SqlBulkCopy(Connection);
            Batch.NotifyAfter = 500;
            Batch.DestinationTableName = tableName;
            ImportDataTable = GetDataTable(tableName);
        }

        private DataTable GetDataTable(string tableName)
        {
            if (tableName == "tblEDSystemsWithCoordinates")
            {
                var table = new DataTable(tableName);
                table.Columns.Add("Id", typeof(long));
                table.Columns.Add("Id64", typeof(long));
                table.Columns.Add("Name", typeof(string));
                table.Columns.Add("X", typeof(double));
                table.Columns.Add("Y", typeof(double));
                table.Columns.Add("Z", typeof(double));
                table.Columns.Add("Date", typeof(DateTime));
                return table;
            }
            else if (tableName == "tblEDStations")
            {
                var table = new DataTable(tableName);
                table.Columns.Add("Id", typeof(int));
                table.Columns.Add("MarketId", typeof(long));
                table.Columns.Add("Type", typeof(string));
                table.Columns.Add("Name", typeof(string));
                table.Columns.Add("DistanceToArrival", typeof(double));
                table.Columns.Add("Allegiance", typeof(string));
                table.Columns.Add("Government", typeof(string));
                table.Columns.Add("Economy", typeof(string));
                table.Columns.Add("SecondEconomy", typeof(string));
                table.Columns.Add("HaveMarket", typeof(bool));
                table.Columns.Add("HaveShipyard", typeof(bool));
                table.Columns.Add("HaveOutfitting", typeof(bool));
                table.Columns.Add("OtherServices", typeof(string));
                table.Columns.Add("SystemId", typeof(int));
                table.Columns.Add("SystemId64", typeof(long));
                table.Columns.Add("SystemName", typeof(string));
                return table;
            }

            return null;
        }

        /// <summary>
        /// C:\Program Files\Microsoft SQL Server\130\Tools\Binn\SqlLocalDB.exe
        /// </summary>
        /// <param name="databaseName"></param>
        /// <param name="deleteIfExists"></param>
        /// <returns></returns>
        public bool CreateDatabase(string databaseName, bool deleteIfExists)
        {
            var fileName = Path.Combine(DatabasePath.FullName, databaseName + ".mdf");
            if (File.Exists(fileName))
            {
                if (deleteIfExists)
                {
                    File.Delete(fileName);
                    var logFileName = Path.Combine(DatabasePath.FullName, databaseName + ".ldf");
                    if (File.Exists(logFileName))
                    {
                        File.Delete(logFileName);
                    }
                }
                else
                {
                    return true;
                }
            }

            var str = $@"CREATE DATABASE {databaseName} ON PRIMARY (NAME = {databaseName}, FILENAME = '{DatabasePath}\{databaseName}.mdf', ";
            str += $"SIZE = 500Mb, MAXSIZE = 10Gb, FILEGROWTH = 20%) LOG ON (NAME = {databaseName}_Log, ";
            str += $@" FILENAME = '{DatabasePath}\{databaseName}.ldf', SIZE = 10MB, MAXSIZE = 10Gb, FILEGROWTH = 20%)";

            using (var connection = new SqlConnection(@"Data Source=(LocalDB)\EDMaster;Initial Catalog=master"))
            using (var command = new SqlCommand(str, connection))
            {
                command.CommandTimeout = (int)TimeSpan.FromMinutes(5).TotalSeconds;
                connection.Open();
                command.ExecuteNonQuery();
            }

            return true;
        }

        public async Task Add10SystemsAsync(int workerId, List<EdsmSystem> systems)
        {
            Console.WriteLine($"Adding 10 systems from worker:{workerId}");
            using (var command = new SqlCommand("[dbo].[prcAdd10SystemsWithCoordinates]", Connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < 10; i++)
                {
                    command.Parameters.AddWithValue($"Id{i}", systems[i].Id);
                    command.Parameters.AddWithValue($"Id64{i}", systems[i].Id64);
                    command.Parameters.AddWithValue($"Name{i}", systems[i].Name);
                    if (systems[i].Coords.HasValue)
                    {
                        command.Parameters.AddWithValue($"X{i}", systems[i].Coords.Value.X);
                        command.Parameters.AddWithValue($"Y{i}", systems[i].Coords.Value.Y);
                        command.Parameters.AddWithValue($"Z{i}", systems[i].Coords.Value.Z);
                    }

                    command.Parameters.AddWithValue($"Date{i}", systems[i].Date);
                }

                await command.ExecuteNonQueryAsync();
                Interlocked.Add(ref RowsAdded, 10);
            }
        }

        public async Task AddSystemAsync(int workerId, EdsmSystem system)
        {
            using (var command = new SqlCommand("[dbo].[prcAddSystemWithCoordinates]", Connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("Id", system.Id);
                command.Parameters.AddWithValue("Id64", system.Id64);
                command.Parameters.AddWithValue("Name", system.Name);
                if (system.Coords.HasValue)
                {
                    command.Parameters.AddWithValue($"X", system.Coords.Value.X);
                    command.Parameters.AddWithValue($"Y", system.Coords.Value.Y);
                    command.Parameters.AddWithValue($"Z", system.Coords.Value.Z);
                }

                command.Parameters.AddWithValue($"Date", system.Date);
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateProgress(string fileName, Int64 rowsProcessed)
        {
            using (var command = new SqlCommand("[dbo].[prcUpdateProgress]", Connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("fileName", fileName);
                command.Parameters.AddWithValue("RowsProcessed", rowsProcessed);
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<Int64> GetProgress(string fileName)
        {
            using (var command = new SqlCommand("[dbo].[prcGetProgress]", Connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("FileName", fileName);
                int rowsProcessed = 0;
                command.Parameters.AddWithValue("RowsProcessed", rowsProcessed).Direction = ParameterDirection.Output;
                await command.ExecuteNonQueryAsync();
                return Convert.ToInt64(command.Parameters["RowsProcessed"].Value);
            }
        }

        public HashSet<int> GetAllNamesAsHashCode()
        {
            var ids = new HashSet<int>();
            long rowsRead = 0;
            using (var command = new SqlCommand("SELECT [Id],[Name] FROM [dbo].[tblEDSystemsWithCoordinates] WITH (NOLOCK)", Connection))
            {
                command.CommandType = CommandType.Text;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rowsRead++;
                        var id = (int)((IDataRecord)reader)["Id"];
                        var name = (string)((IDataReader)reader)["Name"];
                        ids.Add($"{id}_{name}".GetHashCode());
                    }
                }
            }

            Console.WriteLine($"rowsRead:{rowsRead} HashRows:{ids.Count}");
            return ids;
        }

        public void Dispose()
        {
            if (Batch != null)
            {
                Batch.Close();
                Batch = null;
            }

            if (Connection != null)
            {
                Connection.Close();
                Connection.Dispose();
                Connection = null;
            }
        }
    }
}
