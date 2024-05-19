using CarAPI.Data;
using CarAPI.Helper;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Formats.Asn1;
using System.Globalization;

namespace CarAPI.Service
{
    public class CarMakeService
    {
        private readonly HttpClient _httpClient;
        private readonly string _csvFilePath;

        // I read CsvFilePath from app.setting because I think tha file name changed always 
        // we can Use IOptionSnapshot if automaitc apply changes if change file name 
        // now if you want change file name you should rerun application in server 
        // becasue IOptionSnapshot register ad scoped but IOption register as singelton 
        // we can use auto mapper if has simplify complex mappings between different object models

        public CarMakeService(IOptions<CarMakeSettings> options, HttpClient httpClient)
        {
            _csvFilePath = options.Value.CsvFilePath;
            _httpClient = httpClient;
        }

        public int? GetCarMakeId(string make)
        {
            using (var reader = new StreamReader(_csvFilePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                var records = csv.GetRecords<CarMake>().FirstOrDefault(r => r.make_name.Equals(make, StringComparison.OrdinalIgnoreCase));
                return records?.make_id;
            }
        }

        public async Task<List<string>> GetModelsAsync(string make, int modelYear)
        {
            var makeId = GetCarMakeId(make);
            if (makeId == null)
            {
                // we can add resource file to return Msg by language
                throw new KeyNotFoundException("Invalid car make");
            }

            // I use constant because URL is constant and if we need to change it change just in constants class 
            var url = Constants.GET_MODELS_URL + $"{makeId}/modelyear/{modelYear}?format=json";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                // we can add resource file to return Msg by language
                throw new HttpRequestException("Failed to fetch data from external API");
            }

            var content = await response.Content.ReadAsStringAsync();
            
            var models = JObject.Parse(content)["Results"].Select(r => r["Model_Name"].ToString()).ToList();

            return models;
        }
    }
}
