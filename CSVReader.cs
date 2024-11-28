using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SalesForecast
{
    public class CSVReader
    {
        public List<SalesData> ReadSalesData(string filePath)
        {
            List<SalesData> salesData = new List<SalesData>();
            using (var reader = new StreamReader(filePath))
            {
                string headerLine = reader.ReadLine(); // Пропустить заголовок

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var values = line.Split(';');
                    if (values.Length == 3)
                    {
                        string productName = values[0];
                        DateTime saleDate;

                        // Проверяем формат даты
                        if (!DateTime.TryParseExact(values[1], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out saleDate))
                        {
                            throw new FormatException($"Некорректная дата: {values[1]}");
                        }

                        int quantity = int.Parse(values[2]);
                        salesData.Add(new SalesData { Product = productName, Date = saleDate, Quantity = quantity });
                    }
                }
                return salesData;
            }
        }

        // Функция для удаления выбросов на основе межквартильного размаха
        public List<SalesData> RemoveOutliers(List<SalesData> data)
        {
            var groupedData = data.GroupBy(d => d.Product);
            List<SalesData> filteredData = new List<SalesData>();

            foreach (var group in groupedData)
            {
                var quantities = group.Select(g => g.Quantity).ToList();
                quantities.Sort();

                int q1 = quantities[quantities.Count / 4];
                int q3 = quantities[3 * quantities.Count / 4];
                int iqr = q3 - q1;

                int lowerBound = q1 - 1 * iqr;
                int upperBound = q3 + 1 * iqr;

                filteredData.AddRange(group.Where(d => d.Quantity >= lowerBound && d.Quantity <= upperBound));
            }
            return filteredData;
        }
    }

    public class SalesData
    {
        public string Product { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
    }
}