using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Galaxy
{
    public class ImportManager
    {
        public BlockingCollection<EdsmSystem> Queue;
        private Regex _re = new Regex(@"\((.*\))", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public ImportManager(BlockingCollection<EdsmSystem> queue)
        {
            Queue = queue;
        }

        public async void ImportSystemsWithCoordinatesAsync()
        {
            Int64 i = 0;
            const string SystemsWithCoordinates = @"d:\Data\Downloads\systemsWithCoordinates.json";
            Console.WriteLine("Importing systems, please wait");
            var readJsonTotal = new Stopwatch();
            var readJsonOneRow = new Stopwatch();
            var writeTotal = new Stopwatch();
            var write10Rows = new Stopwatch();

            var dbMgr = new DatabaseManager(@"d:\Data\Galaxy", "tblEDSystemsWithCoordinates");
            DatabaseManager.RowsAdded = await dbMgr.GetProgress(SystemsWithCoordinates);
            for (int j = 1; j < 2; j++)
            {
                // AddWorker(j, Queue, new DatabaseManager(@"d:\Data\Galaxy", "tblEDSystemsWithCoordinates"));
            }

            writeTotal.Start();
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
                        i++;
                        if (i < DatabaseManager.RowsAdded)
                        {
                            continue;
                        }

                        var sys = serializer.Deserialize<EdsmSystem>(jr);
                        dbMgr.ImportDataTable.Rows.Add(sys.Id, sys.Id64, sys.Name, sys.Coords.Value.X, sys.Coords.Value.Y, sys.Coords.Value.Z, sys.Date);
                        if (dbMgr.ImportDataTable.Rows.Count > 500)
                        {
                            dbMgr.ImportDataTable.AcceptChanges();
                            try
                            {
                                await dbMgr.Batch.WriteToServerAsync(dbMgr.ImportDataTable);
                            }
                            catch (SqlException ex)
                            {
                                Console.WriteLine(ex.Message);
                                var name = GetSystemName(ex.Message);
                                duplicates.Add(dbMgr.ImportDataTable.Rows.Find(name));

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                            dbMgr.ImportDataTable.Rows.Clear();
                        }

                        if (DatabaseManager.RowsAdded % 10000 == 0 && DatabaseManager.RowsAdded > 0)
                        {
                            await dbMgr.UpdateProgress(SystemsWithCoordinates, DatabaseManager.RowsAdded);
                            Console.WriteLine($"{i} rows added");
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
            }
        }

        private object GetSystemName(string message)
        {
            return _re.Match(message).Groups[1].Value;
        }

        private void AddWorker(int workerId, BlockingCollection<EdsmSystem> queue, DatabaseManager dbMgr)
        {
            Task.Factory.StartNew(async () =>
            {
                foreach (var sys in queue.GetConsumingEnumerable())
                {
                    dbMgr.ImportDataTable.Rows.Add(sys.Id, sys.Id64, sys.Name, sys.Coords.Value.X, sys.Coords.Value.Y, sys.Coords.Value.Z, sys.Date);
                    if (dbMgr.ImportDataTable.Rows.Count > 50)
                    {
                        dbMgr.ImportDataTable.AcceptChanges();
                        await dbMgr.Batch.WriteToServerAsync(dbMgr.ImportDataTable);
                        dbMgr.ImportDataTable.Rows.Clear();
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }
    }
}
