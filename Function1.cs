using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Data.SqlClient;

namespace FunctionApp2
{
    public static class Function1
    {
        static Function1()
        {
            ApplicationHelper.Startup();
        }

        [FunctionName("Function1")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            var str = Environment.GetEnvironmentVariable("sqldb_connection");

            try {
                using (SqlConnection conn = new SqlConnection(str))
                {
                    conn.Open();
                    string t = DateTime.Now.ToString();
                    var text = "INSERT INTO timetest VALUES('"+t+"');";

                    using (SqlCommand cmd = new SqlCommand(text, conn))
                    {
                        // Execute the command and log the # rows affected.
                        var rows = await cmd.ExecuteNonQueryAsync();
                        log.Info("1 row" + t + " inserted");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Info(ex.ToString());
            }

            return null;
        }
    }
}
