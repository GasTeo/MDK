using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesForecast
{
    public class SalesPredictor
    {
        public Dictionary<string, double> ForecastSales(List<SalesData> salesData, int forecastDays)
        {
            var groupedData = salesData.GroupBy(d => d.Product);
            Dictionary<string, double> forecasts = new Dictionary<string, double>();

            foreach (var group in groupedData)
            {
                string product = group.Key;
                var salesQuantities = group.OrderBy(d => d.Date).Select(d => d.Quantity).ToList();

                double averageSales = salesQuantities.Average();
                double forecast = averageSales * forecastDays;

                forecasts[product] = forecast;
            }

            return forecasts;
        }
    }
}