using OrganizationsWaterSupplyL4.Data;
using OrganizationsWaterSupplyL4.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace OrganizationsWaterSupplyL4.Services
{
    public class CachedCountersService : ICachedService<Counter>
    {
        private readonly OrganizationsWaterSupplyContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        public CachedCountersService(OrganizationsWaterSupplyContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        public void AddData(string cacheKey)
        {
            IEnumerable<Counter> Counters = _dbContext.Counters.Include(x => x.Model).Include(x => x.Organization).ToList();
            if (Counters != null)
            {
                _memoryCache.Set(cacheKey, Counters, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(270)
                });

            }

        }
        public IEnumerable<Counter> GetData(string cacheKey)
        {
            IEnumerable<Counter> counters;
            if (!_memoryCache.TryGetValue(cacheKey, out counters))
            {
                counters = _dbContext.Counters.Include(x => x.Model).Include(x => x.Organization).ToList();
                if (counters != null)
                {
                    _memoryCache.Set(cacheKey, counters,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(270)));
                }
            }
            return counters;
        }
        public void UpdateData(string cacheKey)
        {
            IEnumerable<Counter> counters;
                counters = _dbContext.Counters.Include(x => x.Model).Include(x => x.Organization).ToList();
                    _memoryCache.Set(cacheKey, counters,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(270)));
        }
    }
}

