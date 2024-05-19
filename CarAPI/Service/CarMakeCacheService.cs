using CarAPI.Helper;
using Microsoft.Extensions.Caching.Memory;

namespace CarAPI.Service
{
    public class CarMakeCacheService
    {
        //We can use cache servers like Redis, and we must use cache invalidation mechanisms to update cached data if the CSV or data source changes.

        private readonly IMemoryCache _memoryCache;
        private readonly CarMakeCsvLoader _csvLoader;

        public CarMakeCacheService(IMemoryCache memoryCache, CarMakeCsvLoader csvLoader)
        {
            _memoryCache = memoryCache;
            _csvLoader = csvLoader;
        }

        public Dictionary<string, int> GetCarMakes()
        {
            if (!_memoryCache.TryGetValue(Constants.CAR_MAKE_CACHE_KEY, out Dictionary<string, int> carMakes))
            {
                carMakes = _csvLoader.LoadCarMakes();

                // Sets how long the cache entry can be inactive before it will be removed (1 hour).
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromHours(1));

                _memoryCache.Set(Constants.CAR_MAKE_CACHE_KEY, carMakes, cacheEntryOptions);
            }

            return carMakes;
        }
    }
}
