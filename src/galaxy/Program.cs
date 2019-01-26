using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

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
        static int Main(string[] args)
        {
            // SqlPackage.exe /Action:Publish /SourceFile:D:\prj\galaxy\src\EDDB\bin\Debug\eddb.dacpac /TargetConnectionString:"Data Source=(LocalDB)\EDMaster; Initial Catalog = EDSystems" /Diagnostics
            var dbMgr = new DatabaseManager(@"d:\Data\Galaxy", "tblEDSystemsWithCoordinates");
            dbMgr.CreateDatabase("EDSystems", false);
            var importer = new ImportManager(new BlockingCollection<EdsmSystem>(1000));
            Task.Run(() => importer.ImportSystemsWithCoordinatesAsync());
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();

            return 0;
        }
    }
}