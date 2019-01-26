using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Galaxy
{
    public class DatabaseManager: IDisposable
    {
        public DirectoryInfo DatabasePath { get; private set; }
        public static long RowsAdded = 0;
        private SqlConnection _connection;

        public DatabaseManager(string databasePath)
        {
            if (!Directory.Exists(databasePath))
            {
                Directory.CreateDirectory(databasePath);
            }

            DatabasePath = new DirectoryInfo(databasePath);
            _connection = new SqlConnection(@"Data Source=(LocalDB)\EDMaster;Initial Catalog=EDSystems");
            _connection.Open();
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
            str += $"SIZE = 100Mb, MAXSIZE = 10Gb, FILEGROWTH = 50%) LOG ON (NAME = {databaseName}_Log, ";
            str += $@" FILENAME = '{DatabasePath}\{databaseName}.ldf', SIZE = 10MB, MAXSIZE = 500MB, FILEGROWTH = 20%)";

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
            using (var command = new SqlCommand("[dbo].[prcAdd10SystemsWithCoordinates]", _connection))
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
            using (var command = new SqlCommand("[dbo].[prcAddSystemWithCoordinates]", _connection))
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
            using (var command = new SqlCommand("[dbo].[prcUpdateProgress]", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("fileName", fileName);
                command.Parameters.AddWithValue("RowsProcessed", rowsProcessed);
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<Int64> GetProgress(string fileName)
        {
            using (var command = new SqlCommand("[dbo].[prcGetProgress]", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("FileName", fileName);
                int rowsProcessed = 0;
                command.Parameters.AddWithValue("RowsProcessed", rowsProcessed).Direction = ParameterDirection.Output;
                await command.ExecuteNonQueryAsync();
                return Convert.ToInt64(command.Parameters["RowsProcessed"].Value);
            }
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
            }
        }
    }
}
