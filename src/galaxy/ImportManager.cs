using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Galaxy
{
    public class ImportManager
    {
        public async void ImportSystemsWithCoordinatesAsync()
        {
            Int64 i = 0;
            const string SystemsWithCoordinates = @"d:\prj\galaxy\edsm.net\systemsWithCoordinates1.json";
            Console.WriteLine("Importing systems, please wait");
            var readJsonTotal = new Stopwatch();
            var readJsonOneRow = new Stopwatch();
            var writeTotal = new Stopwatch();
            var write10Rows = new Stopwatch();

            var dbMgr = new DatabaseManager(@"d:\Data\Galaxy");
            DatabaseManager.RowsAdded = await dbMgr.GetProgress(SystemsWithCoordinates);
            var systemQueue = new BlockingCollection<EdsmSystem>(500);
            for (int j = 1; j < 6; j++)
            {
                AddWorker(j, systemQueue, new DatabaseManager(@"d:\Data\Galaxy"));
            }

            using (var sr = new StreamReader(File.OpenRead(SystemsWithCoordinates)))
            using (var jr = new JsonTextReader(sr) { CloseInput = false, SupportMultipleContent = true })
            {
                readJsonTotal.Start();
                writeTotal.Start();
                var serializer = new JsonSerializer();
                sr.ReadLine();
                try
                {
                    var systems = new List<EdsmSystem>();
                    while (jr.Read())
                    {
                        var sys = serializer.Deserialize<EdsmSystem>(jr);
                        systems.Add(sys);
                        if (systems.Count == 10)
                        {
                            if (i > DatabaseManager.RowsAdded)
                            {
                                systemQueue.Add(sys);
                            }

                            systems.Clear();
                        }

                        i++;
                        if (i % 1000 == 0)
                        {
                            Console.WriteLine($"Read {i} systems");
                        }

                        if (DatabaseManager.RowsAdded % 10000 == 0 && DatabaseManager.RowsAdded > 0)
                        {
                            await dbMgr.UpdateProgress(SystemsWithCoordinates, DatabaseManager.RowsAdded);
                        }
                    }

                    foreach (var system in systems)
                    {
                        await dbMgr.AddSystemAsync(0, system);
                    }
                }
                catch (JsonReaderException ex)
                {
                    if (jr.TokenType == JsonToken.EndArray)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        private void AddWorker(int workerId, BlockingCollection<EdsmSystem> queue, DatabaseManager dbMgr)
        {
           Task.Factory.StartNew(async () =>
           {
               var buffer = new List<EdsmSystem>();
               foreach (var system in queue.GetConsumingEnumerable())
               {
                   buffer.Add(system);
                   if (buffer.Count == 10)
                   {
                       await dbMgr.Add10SystemsAsync(workerId, buffer);
                       buffer.Clear();
                   }
               }
           }, TaskCreationOptions.LongRunning);
        }
    }
}
