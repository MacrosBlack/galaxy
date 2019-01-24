using galaxy;
using System.Threading.Tasks;

namespace Galaxy
{
    class Program
    {
        static void Main(string[] args)
        {
            // SqlPackage.exe /Action:Publish /SourceFile:D:\prj\galaxy1\EDDB\bin\Debug\EDDB.dacpac /TargetConnectionString:"Data Source=(LocalDB)\EDMaster; Initial Catalog = EDSystems"
            var dbMgr = new DatabaseManager(@"d:\Data\Galaxy");
            dbMgr.CreateDatabase("EDSystems", false);

            var importer = new ImportManager();
            var importTask = Task.Run(() => importer.ImportSystemsWithCoordinatesAsync());
            importTask.Wait();
        }
    }
}