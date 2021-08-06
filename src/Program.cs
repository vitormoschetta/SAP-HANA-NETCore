using System;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json.Linq;
using Npgsql;
using Sap.Data.Hana;

namespace App
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await PostgresTest();
            await SapHanaTest();
        }

        private static async Task SapHanaTest()
        {
            SetEnvironmentSapHana();

            try
            {
                var server = "localhost:39017";
                var user = "system";
                var password = "Password854*";
                var others = "encrypt=true;sslValidateCertificate=false";

                using (var conn = new HanaConnection($"Server={server};UID={user};PWD={password}; {others}"))
                {
                    conn.Open();
                    Console.WriteLine("Open SAP Hana database connection");

                    var query = "SELECT p.ID, p.NAME, p.PRICE FROM PRODUCT p";

                    var queryResult = await conn.QueryAsync<dynamic>(query);

                    foreach (var row in queryResult)
                    {
                        Console.WriteLine(row);
                    }

                    var jsonResult = JToken.FromObject(queryResult);

                    Console.WriteLine("Successful!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
                Console.WriteLine(ex.ToString());
            }
        }


        private static void SetEnvironmentSapHana()
        {
            var SapHanaEnv = Environment.GetEnvironmentVariable("HDBDOTNETCORE");

            if (SapHanaEnv == null)
            {
                Environment.SetEnvironmentVariable("HDBDOTNETCORE", "/SapHana/dotnetcore");
            }
        }

        private static async Task PostgresTest()
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

                    Console.WriteLine("Successful!\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
                Console.WriteLine(ex.ToString());
            }
        }

        private static void SapHanaTest2()
        {
            SetEnvironmentSapHana();

            var server = "localhost:39017";
            var user = "system";
            var password = "Password854*";
            var others = "encrypt=true;sslValidateCertificate=false";

            try
            {
                using (var conn = new HanaConnection(string.Format("Server={0};UID={1};PWD={2}; {3}", server, user, password, others)))
                {
                    conn.Open();

                    var query = "SELECT p.ID, p.NAME, p.PRICE FROM PRODUCT p";
                    using (var cmd = new HanaCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var sbRow = new StringBuilder();

                            for (var i = 0; i < reader.FieldCount; i++)
                            {
                                sbRow.Append(reader[i].ToString().PadRight(20));
                            }

                            Console.WriteLine(sbRow.ToString());

                            var jsonResult = JToken.FromObject(sbRow);
                        }
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
                Console.WriteLine(ex.ToString());
            }
        }
    }
}