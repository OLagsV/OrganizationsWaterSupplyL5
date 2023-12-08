using OrganizationsWaterSupplyL4.Data;
using OrganizationsWaterSupplyL4.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace OrganizationsWaterSupplyL4.Services
{
    public class CachedCountersChecksService : ICachedService<CountersCheck>
    {
        private readonly OrganizationsWaterSupplyContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        public CachedCountersChecksService(OrganizationsWaterSupplyContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        public void AddData(string cacheKey)
        {
            IEnumerable<CountersCheck> CountersCheck = _dbContext.CountersChecks.Include(x => x.RegistrationNumber).ToList();
            if (CountersCheck != null)
            {
                _memoryCache.Set(cacheKey, CountersCheck, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(270)
                });

            }

        }
        public IEnumerable<CountersCheck> GetData(string cacheKey)
        {
            IEnumerable<CountersCheck> countersChecks;
            if (!_memoryCache.TryGetValue(cacheKey, out countersChecks))
            {
                countersChecks = _dbContext.CountersChecks.Include(x => x.RegistrationNumber).ToList();
                if (countersChecks != null)
                {
                    _memoryCache.Set(cacheKey, countersChecks,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(270)));
                }
            }
            return countersChecks;
        }
        public void UpdateData(string cacheKey)
        {
            IEnumerable<CountersCheck> countersChecks;
                countersChecks = _dbContext.CountersChecks.Include(x => x.RegistrationNumber).ToList();
                    _memoryCache.Set(cacheKey, countersChecks,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(270)));
        }
    }
}

