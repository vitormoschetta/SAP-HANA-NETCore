using System;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json.Linq;
using Npgsql;

namespace src
{
    public static class PostgresTest
    {
        public static async Task Execute()
        {
            try
            {
                var server = "localhost";
                var user = "postgres";

                using (var conn = new NpgsqlConnection($"Server={server};Port=5499;User Id={user};"))
                {
                    conn.Open();
                    Console.WriteLine("Open Postgres database connection");

                    var query = "SELECT p.ID, p.NAME, p.PRICE FROM PRODUCT p";                            

                    var queryResult = await conn.QueryAsync<dynamic>(query);

                    foreach (var row in queryResult)
                    {
                        Console.WriteLine(row);
                    }

                    var jsonResult = JToken.FromObject(queryResult);

                    Console.WriteLine($"json: {jsonResult}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
                Console.WriteLine(ex.ToString());
            }
        }

         public static string querySchema = @"select
                                                COLUMN_NAME as name,
                                                DATA_TYPE as type,
                                                NUMERIC_PRECISION as precision,
                                                NUMERIC_SCALE as scale
                                            from
                                                INFORMATION_SCHEMA.COLUMNS
                                            where
                                                TABLE_NAME = 'product'
                                                and TABLE_SCHEMA = 'public'";   
    }
}