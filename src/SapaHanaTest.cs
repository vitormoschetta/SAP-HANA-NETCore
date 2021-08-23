using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sap.Data.Hana;
using src.Custom;

namespace src
{
    public static class SapaHanaTest
    {
        public static async Task Execute()
        {
            SetEnvironmentSapHana();

            var server = "localhost:39017";
            var user = "system";
            var password = "Password854*";
            var others = "encrypt=true;sslValidateCertificate=false";

            try
            {
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

                    JsonConvert.DefaultSettings = () => new JsonSerializerSettings
                    {
                        Converters = new List<JsonConverter> { new StringDecimalConverter() }
                    };

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


        private static void SetEnvironmentSapHana()
        {
            var SapHanaEnv = Environment.GetEnvironmentVariable("HDBDOTNETCORE");

            if (SapHanaEnv == null)
            {
                Environment.SetEnvironmentVariable("HDBDOTNETCORE", "/SapHana/dotnetcore");
            }
        }


        public static string querySchema = @"SELECT
                                                column_name AS name,
                                                data_type_name AS TYPE,
                                                LENGTH AS PRECISION,
                                                SCALE AS SCALE
                                            FROM
                                                TABLE_COLUMNS
                                            WHERE
                                                schema_name = 'SYSTEM'
                                                AND table_name = 'PRODUCT'";
    }
}