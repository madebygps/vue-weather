using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;
using System.Text;

namespace Company.Function
{
    public static class GetWeather
    {
        [FunctionName("GetWeather")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string url_base = System.Environment.GetEnvironmentVariable("url_base");
            string api_key = System.Environment.GetEnvironmentVariable("api_key");

            string city = req.Query["query"];;

            string query = string.Format("{0}{1}",city,"&units=imperial&APPID=");


            HttpClient client = new HttpClient();
            var response = await client.GetAsync(string.Format("{0}{1}{2}", url_base, query, api_key));
            string content = await response.Content.ReadAsStringAsync();

            Root weather = JsonConvert.DeserializeObject<Root>(content);
            ParsedWeather parsedWeather = new ParsedWeather();

            parsedWeather.condition = weather.weather[0].main;
            parsedWeather.temp = weather.main.temp;
            parsedWeather.country = weather.sys.country;
            parsedWeather.cityName = weather.name;

            string output = JsonConvert.SerializeObject(parsedWeather);

            // http://api.openweathermap.org/data/2.5/weather?q=danbury&units=imperial&APPID=1ca7c164507d176307c573fa19b06ff9

    

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(output, Encoding.UTF8, "application/json")
            };

            
        }
    }


public class ParsedWeather {
    public string condition{ get; set; }
    public double temp { get; set; }
    public string country { get; set; }

    public string cityName { get; set; }

}

    public class Weather
    {
        public string main { get; set; }
    }

    public class Main
    {
        public double temp { get; set; }

    }

    public class Sys
    {
        public string country { get; set; }
    }

    public class Root
    {

        public List<Weather> weather { get; set; }
        public Main main { get; set; }
        public Sys sys { get; set; }
        public string name { get; set; }

    }

}
