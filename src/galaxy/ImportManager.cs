using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace Galaxy
{
    public class ImportManager
    {
        public DatabaseManager DatabaseManager;
        private Regex _re = new Regex(@"\((.*\))", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private long _totalRowsCopied = 0;

        public ImportManager(DatabaseManager dbMgr)
        {
            DatabaseManager = dbMgr;
        }

        public void ImportSystemsWithCoordinates()
        {
            long rowsRead = 0;
            const string SystemsWithCoordinates = @"d:\Data\Downloads\systemsWithCoordinates.json";
            Console.WriteLine("Importing systems, please wait");
            var readJsonTotal = new Stopwatch();
            var readJsonOneRow = new Stopwatch();
            var writeTotal = new Stopwatch();
            var write10Rows = new Stopwatch();
            writeTotal.Start();
            DatabaseManager.Batch.SqlRowsCopied += Batch_SqlRowsCopied;
            using (var sr = new StreamReader(File.OpenRead(SystemsWithCoordinates)))
            using (var jr = new JsonTextReader(sr) { CloseInput = false, SupportMultipleContent = true })
            {
                readJsonTotal.Start();
                writeTotal.Start();
                var serializer = new JsonSerializer();
                sr.ReadLine();
                var duplicates = new List<DataRow>();
                var jsonDuplicates = new List<EdsmSystem>();
                try
                {
                    while (jr.Read())
                    {
                        rowsRead++;
                        var sys = serializer.Deserialize<EdsmSystem>(jr);
                        DatabaseManager.ImportDataTable.Rows.Add(sys.Id, sys.Id64, sys.Name, sys.Coords.Value.X, sys.Coords.Value.Y, sys.Coords.Value.Z, sys.Date);
                        DatabaseManager.ImportDataTable.AcceptChanges();
                        if (DatabaseManager.ImportDataTable.Rows.Count > 1000)
                        {
                            try
                            {
                                DatabaseManager.Batch.WriteToServer(DatabaseManager.ImportDataTable);
                            }
                            catch (SqlException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                            DatabaseManager.ImportDataTable.Rows.Clear();
                        }

                        if (rowsRead % 10000 == 0 && rowsRead > 0)
                        {
                            Console.WriteLine($"{rowsRead} rows read");
                            Console.WriteLine($"{_totalRowsCopied} rows copied");
                        }
                    }
                }
                catch (JsonReaderException ex)
                {
                    if (jr.TokenType == JsonToken.EndArray)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                writeTotal.Stop();
                Console.WriteLine($"Total import time:{writeTotal.ElapsedMilliseconds}ms");

                Console.WriteLine($"Rows read:{rowsRead}");
                Console.WriteLine($"Rows copied:{_totalRowsCopied}");
            }
        }

        private void Batch_SqlRowsCopied(object sender, SqlRowsCopiedEventArgs e)
        {
            _totalRowsCopied += e.RowsCopied;
        }

        public void ImportStations()
        {
            Int64 i = 0;
            const string SystemsWithCoordinates = @"d:\Data\Downloads\Stations.json";
            Console.WriteLine("Importing systems, please wait");
            var readJsonTotal = new Stopwatch();
            var readJsonOneRow = new Stopwatch();
            var writeTotal = new Stopwatch();
            var write10Rows = new Stopwatch();
            long rowsDeserialized = 0;
            long rowsWritten = 0;
            long rowsWithErrors = 0;

            writeTotal.Start();
            using (var sr = new StreamReader(File.OpenRead(SystemsWithCoordinates)))
            using (var jr = new JsonTextReader(sr) { CloseInput = false, SupportMultipleContent = true })
            {
                readJsonTotal.Start();
                writeTotal.Start();
                var serializer = new JsonSerializer();
                sr.ReadLine();
                var duplicates = new List<DataRow>();
                var jsonDuplicates = new List<EdsmStation>();
                try
                {
                    while (jr.Read())
                    {
                        i++;
                        //if (i < DatabaseManager.RowsAdded)
                        //{
                        //    continue;
                        //}

                        var station = serializer.Deserialize<EdsmStation>(jr);
                        rowsDeserialized++;
                        Console.WriteLine(station.SystemName);
                        if (station.Name != "Levchenko Enterprise")
                        {
                            continue;
                        }

                        DatabaseManager.ImportDataTable.Rows.Add(station.Id, station.MarketId, station.Type, station.Name, station.DistanceToArrival, station.Allegiance, station.Government, station.Economy, station.SecondEconomy, station.HaveMarket, station.HaveShipyard, station.HaveOutfitting, station.OtherServices, station.SystemId, station.SystemId64, station.SystemName);
                        DatabaseManager.ImportDataTable.AcceptChanges();
                        if (DatabaseManager.ImportDataTable.Rows.Count > 500)
                        {
                            try
                            {
                                DatabaseManager.Batch.WriteToServer(DatabaseManager.ImportDataTable);
                            }
                            catch (SqlException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                            rowsWritten += DatabaseManager.ImportDataTable.Rows.Count;
                            var errorRows = DatabaseManager.ImportDataTable.GetErrors();
                            rowsWithErrors += errorRows.Length;
                            DatabaseManager.ImportDataTable.Rows.Clear();
                        }
                    }
                }
                catch (JsonReaderException ex)
                {
                    if (jr.TokenType == JsonToken.EndArray)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    else
                    {
                        throw;
                    }
                }

                if (DatabaseManager.ImportDataTable.Rows.Count > 0)
                {
                    DatabaseManager.Batch.WriteToServer(DatabaseManager.ImportDataTable);
                }

                Console.WriteLine($"{rowsDeserialized} rows deserialized");
                Console.WriteLine($"{rowsWithErrors} rows had errors");
                //Console.WriteLine("Shrinking the database, this could take a while");
                //using (var command = new SqlCommand())
                //{
                //    command.CommandText = "ALTER DATABASE EDSystems SET RECOVERY SIMPLE; DBCC SHRINKFILE(EDSystems, 1); SET RECOVERY FULL WITH ROLLBACK IMMEDIATE";
                //    command.CommandTimeout = (int)TimeSpan.FromMinutes(15).TotalMinutes;
                //    command.CommandType = CommandType.Text;
                //    command.ExecuteNonQuery();
                //}
            }
        }

        private object GetSystemName(string message)
        {
            return _re.Match(message).Groups[1].Value;
        }
    }
}
