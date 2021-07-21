using System;
using System.Text;
using Sap.Data.Hana;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            SapHanaSetEnvironment();

            var server = "localhost:39017";
            var user = "system";
            var password = "Password854*";
            var others = "encrypt=true;sslValidateCertificate=false";

            try
            {
                using (var conn = new HanaConnection(string.Format("Server={0};UID={1};PWD={2}; {3}", server, user, password, others)))
                {
                    conn.Open();
                    Console.WriteLine("Connected to " + server);
         
                    var query = "SELECT * FROM sys.AFL_AREAS_";
                    using (var cmd = new HanaCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine("Query result:");

                        // Print column names
                        var sbCol = new StringBuilder();
                        for (var i = 0; i < reader.FieldCount; i++)
                        {                            
                            sbCol.Append(reader.GetName(i).PadRight(20));
                        }
                        Console.WriteLine(sbCol.ToString());

                        // Print rows
                        while (reader.Read())
                        {
                            var sbRow = new StringBuilder();

                            // Print items
                            for (var i = 0; i < reader.FieldCount; i++)
                            {                                
                                sbRow.Append(reader[i].ToString().PadRight(20));
                            }
                            Console.WriteLine(sbRow.ToString());
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

        private static void SapHanaSetEnvironment()
        {
            var SapHanaEnv = Environment.GetEnvironmentVariable("HDBDOTNETCORE");

            if (SapHanaEnv == null)
            {
                Environment.SetEnvironmentVariable("HDBDOTNETCORE", "/SapHanaBionexo/dotnetcore");
            }
        }
    }
}