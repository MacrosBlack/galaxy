using Galaxy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace galaxy
{
    public class ImportManager
    {
        public async void ImportSystemsWithCoordinatesAsync()
        {
            var dbMgr = new DatabaseManager(@"d:\Data\Galaxy");
            Console.WriteLine("Importing systems, please wait");
            Int64 i = 0;
            using (var sr = new StreamReader(File.OpenRead(@"d:\prj\galaxy\edsm.net\systemsWithCoordinates.json")))
            using (var jr = new JsonTextReader(sr) { CloseInput = false, SupportMultipleContent = true })
            {
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
                            await dbMgr.Add10SystemsAsync(systems);
                            systems.Clear();
                        }

                        i++;
                        if (i % 100000 == 0)
                        {
                            Console.WriteLine($"Read {i} systems");
                        }
                    }

                    foreach (var system in systems)
                    {
                        await dbMgr.AddSystemAsync(system);
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
    }
}
