using System;

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
            //dbMgr.CreateDatabase("EDSystems", false);
            // SqlPackage.exe /Action:Publish /SourceFile:D:\prj\galaxy\src\EDDB\bin\Debug\eddb.dacpac /TargetConnectionString:"Data Source=(LocalDB)\EDMaster; Initial Catalog = EDSystems" /Diagnostics
            var dbMgr = new DatabaseManager(@"d:\Data\Galaxy", "tblEDSystemsWithCoordinates");
            var importer = new ImportManager(dbMgr);
            importer.ImportSystemsWithCoordinates();

            //Task.Run(() => importer.ImportSystemsWithCoordinatesAsync());
            //var dbMgr = new DatabaseManager(@"d:\Data\Galaxy", "tblEDStations");
            //var importer = new ImportManager(dbMgr);
            //importer.ImportStations();
        }
    }
}