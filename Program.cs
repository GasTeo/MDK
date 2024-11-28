using System;
using System.Collections.Generic;
using System.Formats.Asn1;

namespace SalesForecast
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFilePath = "D:\\Project\\sales.csv"; // Укажите путь к входному файлу
            string outputFilePath = "D:\\Project\\forecast.csv"; // Укажите путь к выходному файлу

            int forecastDays;
            Console.WriteLine("Введите количество дней для прогноза:");
            while (!int.TryParse(Console.ReadLine(), out forecastDays) || forecastDays <= 0)
            {
                Console.WriteLine("Пожалуйста, введите корректное число дней.");
            }

            FileValidator validator = new FileValidator();
            if (!validator.ValidateFileStructure(inputFilePath, out var validationErrors))
            {
                Console.WriteLine("Ошибки валидации файла:");
                validationErrors.ForEach(Console.WriteLine);
                return;
            }

            CSVReader csvReader = new CSVReader();
            List<SalesData> salesData = csvReader.ReadSalesData(inputFilePath);
            Console.WriteLine("Чтение данных завершено.");

            Console.WriteLine("Анализ данных на выбросы...");
            List<SalesData> filteredData = csvReader.RemoveOutliers(salesData);
            Console.WriteLine($"Количество записей после удаления выбросов: {filteredData.Count}");

            SalesPredictor salesPredictor = new SalesPredictor();
            Dictionary<string, double> forecasts = salesPredictor.ForecastSales(filteredData, forecastDays);

            Console.WriteLine("\nПрогноз продаж на следующие " + forecastDays + " дней:");
            foreach (var forecast in forecasts)
            {
                Console.WriteLine($"Товар: {forecast.Key}, Прогнозируемые продажи: {forecast.Value} единиц");
            }

            ForecastSaver saver = new ForecastSaver();
            saver.SaveForecasts(outputFilePath, forecasts);
        }
    }
}
