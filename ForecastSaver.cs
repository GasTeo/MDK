namespace SalesForecast
{
    public class ForecastSaver
    {
        public void SaveForecasts(string filePath, Dictionary<string, double> forecasts)
        {
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Product;ForecastedQuantity");
                foreach (var forecast in forecasts)
                {
                    writer.WriteLine($"{forecast.Key};{forecast.Value:F2}");
                }
            }

            Console.WriteLine($"Прогнозы успешно сохранены в файл: {filePath}");
        }
    }
}
