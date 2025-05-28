using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace EFund.Seeding.Utils;

public class SeedingUtils
{
    public static List<T> GetFromCsv<T>(string filePath, string delimiter = ",")
    {
        var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = delimiter,
            HeaderValidated = null,
            MissingFieldFound = null
        };

        using var streamReader = new StreamReader(filePath);
        using var csvReader = new CsvReader(streamReader, configuration);
        return csvReader.GetRecords<T>().ToList();
    }
}