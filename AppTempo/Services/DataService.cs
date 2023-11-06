using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using AppTempo.Model;

namespace AppTempo.Services
{
    class DataService
    {
        public static async Task<dynamic> getDataFromService(string query)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(query);
            dynamic data = null;
            if(response != null)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                data = JsonConvert.DeserializeObject(json);
            } 
            return data;
        }

        public static async Task<Tempo> GetPrevisaoDoTempo(string cidade)
        {
            string appID = "3a9759666cf49bbf369b94808d332902";

            string query = $"http://api.openweathermap.org/data/2.5/weather?q={cidade}&units=metric&appid={appID}";

            dynamic resultado = await getDataFromService(query).ConfigureAwait(false);
            if (resultado["weather"] != null)
            {
                Tempo previsao = new Tempo();
                previsao.Title = (string)resultado["name"];
                previsao.Temperature = (string)resultado["main"]["temp"] + " C";
                previsao.Wind = (string)resultado["wind"]["speed"] + " mph";
                previsao.Humidity = (string)resultado["main"]["humidity"] + " %";
                previsao.Visibility = (string)resultado["weather"][0]["main"];
                DateTime time = new DateTime(1970, 1, 1, 0,0,0,0);
                DateTime sunrise = time.AddSeconds((double)resultado["sys"]["sunrise"]);
                DateTime sunset = time.AddSeconds((double)resultado["sys"]["sunset"]);
                previsao.Sunrise = String.Format("{0:d/MM/yyyy HH:mm:ss}",sunrise);
                previsao.Sunset = String.Format("{0:d/MM/yyyy HH:mm:ss}", sunset);
                return previsao;
            }
                return null;
        }
    }
}
