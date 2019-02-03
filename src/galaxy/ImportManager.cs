using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

        public HashSet<int> ImportSystemsWithCoordinates(HashSet<int> existingRows)
        {
            long rowsRead = 0;
            const string SystemsWithCoordinates = @"d:\Data\Downloads\systemsWithCoordinates.json";
            Console.WriteLine("Importing systems, please wait");
            var readJsonTotal = new Stopwatch();
            var readJsonOneRow = new Stopwatch();
            var writeTotal = new Stopwatch();
            var write10Rows = new Stopwatch();
            writeTotal.Start();
            var ids = new HashSet<int>();
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
                        var hashCode = $"{sys.Id}_{sys.Name}".GetHashCode();
                        ids.Add(hashCode);
                        if (existingRows.Contains(hashCode))
                        {
                            continue;
                        }

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

                if (DatabaseManager.ImportDataTable.Rows.Count > 0)
                {
                    DatabaseManager.Batch.WriteToServer(DatabaseManager.ImportDataTable);
                }

                writeTotal.Stop();
                Console.WriteLine($"Total import time:{writeTotal.ElapsedMilliseconds}ms");

                Console.WriteLine($"Rows read:{rowsRead}");
                return ids;
            }
        }


        public void ImportBodies()
        {
            const string Bodies = @"d:\Data\Downloads\bodies.json";
            var readJsonTotal = new Stopwatch();
            var readJsonOneRow = new Stopwatch();
            var writeTotal = new Stopwatch();
            var write10Rows = new Stopwatch();
            long rowsDeserialized = 0;
            long rowsWithErrors = 0;

            writeTotal.Start();
            using (var sr = new StreamReader(File.OpenRead(Bodies)))
            using (var jr = new JsonTextReader(sr) { CloseInput = false, SupportMultipleContent = true })
            using (var db1 = new SqlConnection(@"Data Source = (LocalDB)\EDMaster; Initial Catalog = ED_Body1"))
            using (var db2 = new SqlConnection(@"Data Source = (LocalDB)\EDMaster; Initial Catalog = ED_Body2"))
            using (var db3 = new SqlConnection(@"Data Source = (LocalDB)\EDMaster; Initial Catalog = ED_Body3"))
            using (var db4 = new SqlConnection(@"Data Source = (LocalDB)\EDMaster; Initial Catalog = ED_Body4"))
            {
                db1.Open();
                db2.Open();
                db3.Open();
                db4.Open();
                var batch1 = new SqlBulkCopy(db1);
                var batch2 = new SqlBulkCopy(db2);
                var batch3 = new SqlBulkCopy(db3);
                var batch4 = new SqlBulkCopy(db4);

                batch1.DestinationTableName = "tblEDBody";
                batch2.DestinationTableName = "tblEDBody";
                batch3.DestinationTableName = "tblEDBody";
                batch4.DestinationTableName = "tblEDBody";

                var importTable1 = DatabaseManager.GetDataTable("tblEDBody");
                var importTable2 = DatabaseManager.GetDataTable("tblEDBody");
                var importTable3 = DatabaseManager.GetDataTable("tblEDBody");
                var importTable4 = DatabaseManager.GetDataTable("tblEDBody");

                readJsonTotal.Start();
                writeTotal.Start();
                var serializer = new JsonSerializer();
                sr.ReadLine();
                try
                {
                    while (jr.Read())
                    {
                        var body = serializer.Deserialize<EdsmBody>(jr);
                        rowsDeserialized++;
                        var bodyNumber = Math.Abs(body.SystemName.GetHashCode() % 4) + 1;
                        if (bodyNumber == 1)
                        {
                            importTable1.Rows.Add(body.Id, body.Id64, body.BodyId, body.Name, body.Type, body.SubType, body.SystemId, body.SystemId64, body.SystemName);
                        }
                        else if (bodyNumber == 2)
                        {
                            importTable2.Rows.Add(body.Id, body.Id64, body.BodyId, body.Name, body.Type, body.SubType, body.SystemId, body.SystemId64, body.SystemName);
                        }
                        else if (bodyNumber == 3)
                        {
                            importTable3.Rows.Add(body.Id, body.Id64, body.BodyId, body.Name, body.Type, body.SubType, body.SystemId, body.SystemId64, body.SystemName);
                        }
                        else if (bodyNumber == 4)
                        {
                            importTable4.Rows.Add(body.Id, body.Id64, body.BodyId, body.Name, body.Type, body.SubType, body.SystemId, body.SystemId64, body.SystemName);
                        }

                        if (importTable1.Rows.Count > 500)
                        {
                            importTable1.AcceptChanges();
                            batch1.WriteToServer(importTable1);
                            importTable1.Rows.Clear();
                        }

                        if (importTable2.Rows.Count > 500)
                        {
                            importTable2.AcceptChanges();
                            batch2.WriteToServer(importTable2);
                            importTable2.Rows.Clear();
                        }

                        if (importTable3.Rows.Count > 500)
                        {
                            importTable3.AcceptChanges();
                            batch3.WriteToServer(importTable3);
                            importTable3.Rows.Clear();
                        }

                        if (importTable4.Rows.Count > 500)
                        {
                            importTable4.AcceptChanges();
                            batch4.WriteToServer(importTable4);
                            importTable4.Rows.Clear();
                        }


                        if ((rowsDeserialized % 100000) == 0)
                        {
                            Console.WriteLine($"{rowsDeserialized} rows read");
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

                if (importTable1.Rows.Count > 0)
                {
                    importTable1.AcceptChanges();
                    batch1.WriteToServer(importTable1);
                }

                if (importTable2.Rows.Count > 0)
                {
                    importTable2.AcceptChanges();
                    batch2.WriteToServer(importTable2);
                }

                if (importTable3.Rows.Count > 0)
                {
                    importTable3.AcceptChanges();
                    batch3.WriteToServer(importTable3);
                }

                if (importTable4.Rows.Count > 0)
                {
                    importTable4.AcceptChanges();
                    batch4.WriteToServer(importTable4);
                }


                Console.WriteLine($"{rowsDeserialized} rows deserialized");
                Console.WriteLine($"{rowsWithErrors} rows had errors");
            }
        }

        private object GetSystemName(string message)
        {
            return _re.Match(message).Groups[1].Value;
        }

        public void ImportStations()
        {
            const string Stations = @"d:\Data\Downloads\Stations.json";
            Console.WriteLine("Importing systems, please wait");
            var readJsonTotal = new Stopwatch();
            var readJsonOneRow = new Stopwatch();
            var writeTotal = new Stopwatch();
            var write10Rows = new Stopwatch();
            long rowsDeserialized = 0;
            long rowsWritten = 0;
            long rowsWithErrors = 0;

            writeTotal.Start();
            using (var sr = new StreamReader(File.OpenRead(Stations)))
            using (var jr = new JsonTextReader(sr) { CloseInput = false, SupportMultipleContent = true })
            {
                readJsonTotal.Start();
                writeTotal.Start();
                var serializer = new JsonSerializer();
                sr.ReadLine();
                var duplicates = new List<DataRow>();
                var jsonDuplicates = new List<EdsmStation>();
                var stationServices = new List<string>();
                try
                {
                    while (jr.Read())
                    {
                        var station = serializer.Deserialize<EdsmStation>(jr);
                        station.SetOtherServices();
                        rowsDeserialized++;
                        DatabaseManager.ImportDataTable.Rows.Add(
                            station.Id, station.MarketId, station.Type, station.Name, station.DistanceToArrival, station.Allegiance, station.Government, station.Economy, station.SecondEconomy,
                            station.HaveMarket, station.HaveShipyard, station.HaveOutfitting, station.OtherServices, station.SystemId, station.SystemId64, station.SystemName,
                            station.Restock, station.Refuel, station.Repair, station.Contacts, station.UniversalCartographics, station.Missions, station.CrewLounge, station.Tuning,
                            station.SearchandRescue, station.BlackMarket, station.InterstellarFactorsContact, station.MaterialTrader, station.TechnologyBroker);
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
    }
}
