using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonDeserializingTest
{
    public class JsonDeserializeObject
    {
        JObject _jobject;

        public JsonDeserializeObject(JObject jObject) => _jobject = jObject;

        public string JsonValue(string key)
        {
            return _jobject[key].Value<string>();
        }
    }

    public class Rates
    {
        private JsonDeserializeObject _jsonDeserialize;
        
        public Rates(JsonDeserializeObject jsonDeserialize) => _jsonDeserialize = jsonDeserialize;

        public string ask => _jsonDeserialize.JsonValue(nameof(ask));
        public string bid => _jsonDeserialize.JsonValue(nameof(bid));
        public string volume => _jsonDeserialize.JsonValue(nameof(volume));
        public string vWap => _jsonDeserialize.JsonValue(nameof(vWap));
        public string low => _jsonDeserialize.JsonValue(nameof(low));
        public string high => _jsonDeserialize.JsonValue(nameof(high));
        public string last => _jsonDeserialize.JsonValue(nameof(last));
        public string timestamp => _jsonDeserialize.JsonValue(nameof(timestamp));
        public string open => _jsonDeserialize.JsonValue(nameof(open));
    }
    class Program
    {
        public static async Task GetRates()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            using HttpResponseMessage response = await httpClient.GetAsync(new Uri("https://www.bitstamp.net/api/v2/ticker/btcusd/"));
            if (response.IsSuccessStatusCode)
            {
                string responseMessage = await response.Content.ReadAsStringAsync();
                /*
                 * instead of this:
                 * Rates rate = JsonConvert.DeserializeObject<Rates>(responseMessage);
                 */
                Rates rate = new Rates(new JsonDeserializeObject(JObject.Parse(responseMessage)));
                Console.WriteLine(rate.high);
            }
        }

        static void Main(string[] args)
        {
            Task.Run(GetRates).Wait();

        }
    }
}
