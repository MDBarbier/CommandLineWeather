using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CommandLineWeather
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string clientId = string.Empty;
            string secret = string.Empty;
            string latitude = string.Empty;
            string longitude = string.Empty;
            string baseUrl = "https://api-metoffice.apiconnect.ibmcloud.com/metoffice/production/v0/forecasts/point/hourly";

#if DEBUG
            Console.WriteLine("Enter client id:");
            clientId = Console.ReadLine();
            Console.WriteLine("Enter client secret:");
            secret = Console.ReadLine();
            Console.WriteLine("Enter client latitude:");
            latitude = Console.ReadLine();
            Console.WriteLine("Enter client longitude:");
            longitude = Console.ReadLine();

#endif

#if RELEASE
            if (args.Length != 4)
            {
                throw new Exception("Incorrect number of arguments supplied");
            }

            latitude = args[0].ToString();
            longitude = args[1].ToString();
            clientId = args[2].ToString();
            secret = args[3].ToString();
#endif


            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("x-ibm-client-id", clientId);
                httpClient.DefaultRequestHeaders.Add("x-ibm-client-secret", secret);
                httpClient.DefaultRequestHeaders.Add("accept", "application/json");

                var reply = await httpClient.GetAsync($"{baseUrl}?excludeParameterMetadata=false&includeLocationName=true&latitude={latitude}&longitude={longitude}");

                var contents = await reply.Content.ReadAsStringAsync();

                JObject jsonWeatherData = JObject.Parse(contents);

                Console.WriteLine(jsonWeatherData);
            }

        }
    }
}
