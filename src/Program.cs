using System.Threading.Tasks;

namespace src
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await PostgresTest.Execute();
            await SapaHanaTest.Execute();
        }
    }
}