using OrganizationsWaterSupplyL4.Data;
using OrganizationsWaterSupplyL4.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace OrganizationsWaterSupplyL4.Services
{
    public class CachedRatesService : ICachedService<Rate>
    {
        private readonly OrganizationsWaterSupplyContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        public CachedRatesService(OrganizationsWaterSupplyContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        public void AddData(string cacheKey)
        {
            IEnumerable<Rate> Rates = _dbContext.Rates.ToList();
            if (Rates != null)
            {
                _memoryCache.Set(cacheKey, Rates, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(270)
                });

            }

        }
        public IEnumerable<Rate> GetData(string cacheKey)
        {
            IEnumerable<Rate> rates;
            if (!_memoryCache.TryGetValue(cacheKey, out rates))
            {
                rates = _dbContext.Rates.ToList();
                if (rates != null)
                {
                    _memoryCache.Set(cacheKey, rates,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(270)));
                }
            }
            return rates;
        }
        public void UpdateData(string cacheKey)
        {
            IEnumerable<Rate> counters;
                counters = _dbContext.Rates.ToList();
                    _memoryCache.Set(cacheKey, counters,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(270)));
        }
    }
}

