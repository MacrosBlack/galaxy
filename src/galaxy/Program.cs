using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Galaxy
{
    class Program
    {
        /// <summary>
        /// 30033948 rows
        /// Total import time:346063ms: 1000 bulk size
        /// Total import time:419614ms: 500 bulk size
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static void Main(string[] args)
        {
            //using (var dbMgr = new DatabaseManager(@"d:\Data\Galaxy", "tblEDStations"))
            //{
            //    dbMgr.CreateDatabase("ED_Body4", false);
            //}


            // SqlPackage.exe /Action:Publish /SourceFile:D:\prj\galaxy\src\EDDB\bin\Debug\eddb.dacpac /TargetConnectionString:"Data Source=(LocalDB)\EDMaster; Initial Catalog = EDSystems" /Diagnostics

            //using (var dbMgr = new DatabaseManager(@"d:\Data\Galaxy", "tblEDStations"))
            //{
            //    var importer = new ImportManager(dbMgr);
            //    importer.ImportBodies();
            //}

            //HashSet<int> readSystems;
            //HashSet<int> writtenSystems;
            //HashSet<int> existingSystems;
            //using (var dbMgr = new DatabaseManager(@"d:\Data\Galaxy", "tblEDSystemsWithCoordinates"))
            //{
            //    var importer = new ImportManager(dbMgr);
            //    existingSystems = dbMgr.GetAllNamesAsHashCode();
            //    Console.WriteLine($"{existingSystems.Count} existing rows read from the database");
            //    readSystems = importer.ImportSystemsWithCoordinates(existingSystems);
            //    Console.WriteLine($"{readSystems.Count} read from the json file");
            //    writtenSystems= dbMgr.GetAllNamesAsHashCode();
            //}

            //Console.WriteLine($"{writtenSystems.Count} were written");
            //readSystems.ExceptWith(writtenSystems);
            //Console.WriteLine($"{readSystems.Count} was not imported");
            //if (readSystems.Count > 0)
            //{
            //    using (var stream = new StreamWriter("NotImportedSystems.txt"))
            //    {
            //        foreach(var system in readSystems)
            //        {
            //            stream.Write(system);
            //        }
            //    }
            //}

            //Task.Run(() => importer.ImportSystemsWithCoordinatesAsync());

            var dbMgr = new DatabaseManager(@"d:\Data\Galaxy", "tblEDStations");
            var importer = new ImportManager(dbMgr);
            importer.ImportStations();
        }
    }
}