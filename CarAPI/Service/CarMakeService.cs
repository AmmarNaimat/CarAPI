using CarAPI.Data;
using CarAPI.Helper;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Formats.Asn1;
using System.Globalization;

namespace CarAPI.Service
{
    public class CarMakeService
    {

        
        // becasue IOptionSnapshot register ad scoped but IOption register as singelton 
        // we can use auto mapper if has simplify complex mappings between different object models
        // we can add resource file to return Msg by language
        // I use constant because URL is constant and if we need to change it change just in constants class 


        private readonly CarMakeCacheService _cacheService;
        private readonly HttpClient _httpClient;

        public CarMakeService(CarMakeCacheService cacheService, HttpClient httpClient)
        {
            _cacheService = cacheService;
            _httpClient = httpClient;
        }

        public int? GetCarMakeId(string make)
        {
            var carMakes = _cacheService.GetCarMakes();
            if (carMakes.TryGetValue(make, out var id))
            {
                return id;
            }
            return null;
        }

        public async Task<List<string>> GetModelsAsync(string make, int modelYear)
        {
            var makeId = GetCarMakeId(make);
            if (makeId == null)
            {
                throw new KeyNotFoundException("Invalid car make");
            }

            var url =  $"{Constants.GET_MODELS_URL}{makeId}/modelyear/{modelYear}?format=json";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Failed to fetch data from external API");
            }

            var content = await response.Content.ReadAsStringAsync();
            var models = JObject.Parse(content)["Results"].Select(r => r["Model_Name"].ToString()).ToList();

            return models;
        }
    }
}
