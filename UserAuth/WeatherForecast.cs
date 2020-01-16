using System;

namespace UserAuth
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556); 
        //convert double to int and add 32 to covert celcius to farenheit

        public string Summary { get; set; }
    }
}
