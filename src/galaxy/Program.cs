using Galaxy;
using System;
using System.Threading.Tasks;

namespace Galaxy
{
    class Program
    {
        static void Main(string[] args)
        {
            // SqlPackage.exe /Action:Publish /SourceFile:D:\prj\galaxy\src\EDDB\bin\Debug\eddb.dacpac /TargetConnectionString:"Data Source=(LocalDB)\EDMaster; Initial Catalog = EDSystems" /Diagnostics
            var dbMgr = new DatabaseManager(@"d:\Data\Galaxy");
            dbMgr.CreateDatabase("EDSystems", false);

            var importer = new ImportManager();
            Task.Run(() => importer.ImportSystemsWithCoordinatesAsync()).Wait();

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }
    }
}