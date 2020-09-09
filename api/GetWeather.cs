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

            
            

            string url_base = System.Environment.GetEnvironmentVariable("url_base");
            string api_key = System.Environment.GetEnvironmentVariable("api_key");

            
            string city = req.Query["query"];
            log.LogInformation(city);
            string query = string.Format("{0}{1}",city,"&units=imperial&APPID=");


            HttpClient client = new HttpClient();
            var response = await client.GetAsync(string.Format("{0}{1}{2}", url_base, query, api_key));
            string content = await response.Content.ReadAsStringAsync();

            OpenWeather openWeather = JsonConvert.DeserializeObject<OpenWeather>(content);
            ParsedWeather parsedWeather = new ParsedWeather();

            parsedWeather.condition = openWeather.weather[0].main;
            parsedWeather.temp = openWeather.main.temp;
            parsedWeather.country = openWeather.sys.country;
            parsedWeather.cityName = openWeather.name;

            string output = JsonConvert.SerializeObject(parsedWeather);

    

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

    public class OpenWeather
    {

        public List<Weather> weather { get; set; }
        public Main main { get; set; }
        public Sys sys { get; set; }
        public string name { get; set; }

    }

}
