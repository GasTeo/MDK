using System.Globalization;

namespace SalesForecast
{
    public class FileValidator
    {
        public bool ValidateFileStructure(string filePath, out List<string> errors)
        {
            errors = new List<string>();
            using (var reader = new StreamReader(filePath))
            {
                string headerLine = reader.ReadLine();
                if (string.IsNullOrEmpty(headerLine))
                {
                    errors.Add("Файл пустой или не содержит заголовков.");
                    return false;
                }

                var headers = headerLine.Split(';');
                if (headers.Length != 3 || headers[0] != "Product" || headers[1] != "Date" || headers[2] != "Quantity")
                {
                    errors.Add("Неверная структура заголовков. Ожидается: Product;Date;Quantity.");
                    return false;
                }

                string line;
                int lineNumber = 1;
                while ((line = reader.ReadLine()) != null)
                {
                    lineNumber++;
                    var values = line.Split(';');
                    if (values.Length != 3)
                    {
                        errors.Add($"Неверное количество столбцов в строке {lineNumber}.");
                        continue;
                    }

                    if (!DateTime.TryParseExact(values[1], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                    {
                        errors.Add($"Некорректный формат даты в строке {lineNumber}.");
                    }

                    if (!int.TryParse(values[2], out _))
                    {
                        errors.Add($"Некорректное числовое значение в строке {lineNumber}.");
                    }
                }
            }

            return !errors.Any();
        }
    }
}
