using CarAPI.Data;
using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;

namespace CarAPI.Service
{
    public class CarMakeCsvLoader
    {
        private readonly string _csvFilePath;

        public CarMakeCsvLoader(string csvFilePath)
        {
            _csvFilePath = csvFilePath;
        }

        public Dictionary<string, int> LoadCarMakes()
        {
            using (var reader = new StreamReader(_csvFilePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                return csv.GetRecords<CarMake>().ToDictionary(r => r.Make, r => r.Id);
            }
        }
    }
}
