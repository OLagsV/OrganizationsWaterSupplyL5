using OrganizationsWaterSupplyL4.Data;
using OrganizationsWaterSupplyL4.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace OrganizationsWaterSupplyL4.Services
{
    public class CachedCountersDataService : ICachedService<CountersDatum>
    {
        private readonly OrganizationsWaterSupplyContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        public CachedCountersDataService(OrganizationsWaterSupplyContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        public void AddData(string cacheKey)
        {
            IEnumerable<CountersDatum> CountersData = _dbContext.CountersData.Include(x => x.RegistrationNumber).ToList();
            if (CountersData != null)
            {
                _memoryCache.Set(cacheKey, CountersData, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(270)
                });

            }

        }
        public IEnumerable<CountersDatum> GetData(string cacheKey)
        {
            IEnumerable<CountersDatum> counterData;
            if (!_memoryCache.TryGetValue(cacheKey, out counterData))
            {
                counterData = _dbContext.CountersData.Include(x => x.RegistrationNumber).ToList();
                if (counterData != null)
                {
                    _memoryCache.Set(cacheKey, counterData,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(270)));
                }
            }
            return counterData;
        }
        public void UpdateData(string cacheKey)
        {
            IEnumerable<CountersDatum> counterData;
                counterData = _dbContext.CountersData.Include(x => x.RegistrationNumber).ToList();
                    _memoryCache.Set(cacheKey, counterData,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(270)));
        }
    }
}

