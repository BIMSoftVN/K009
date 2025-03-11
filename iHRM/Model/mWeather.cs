using iHRM.Classes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace iHRM.Model
{
    internal class mWeather
    {
        public async static Task<List<clWeatherInfo>> LoadInfoByLocation(double ViDo, double KinhDo)
        {
            var plist = new List<clWeatherInfo>();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            using (HttpClient client = new HttpClient())
            {
                string URL = string.Format(@"https://api.openweathermap.org/data/2.5/forecast?lang=vi&units=metric&lat={0}&lon={1}&appid=11aa4b4963ff5453cf74ef722bb48abf", ViDo, KinhDo);

                var res = await client.GetAsync(URL);

                if (res.IsSuccessStatusCode)
                {
                    string JsonRes = await res.Content.ReadAsStringAsync();
                    var JData = JToken.Parse(JsonRes);

                    JToken[] JList = JData["list"].ToArray();
                    foreach (var JItem in JList)
                    {
                        var unixTime = (long)JItem["dt"];
                        DateTimeOffset dtOff = DateTimeOffset.FromUnixTimeSeconds(unixTime);
                        DateTime dt = dtOff.UtcDateTime;


                        var tt = new clWeatherInfo();
                        tt.wDate = dt;
                        tt.wMoTa = JItem["weather"][0]["description"].ToString();
                        tt.wNhietDo = double.Parse(JItem["main"]["temp"].ToString());
                        tt.wGio = double.Parse(JItem["wind"]["speed"].ToString());

                        string imageUrl = @"https://openweathermap.org/img/w/" + JItem["weather"][0]["icon"].ToString() + ".png";
                        tt.wImage = imageUrl;
                        //tt.wImage = await client.GetByteArrayAsync(new Uri(imageUrl));

                        plist.Add(tt);
                    }


                }


            }



            return plist;
        }
    }
}

