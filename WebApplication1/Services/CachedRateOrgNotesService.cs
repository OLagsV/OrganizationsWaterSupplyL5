using OrganizationsWaterSupplyL4.Data;
using OrganizationsWaterSupplyL4.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace OrganizationsWaterSupplyL4.Services
{
    public class CachedRateOrgNotesService : ICachedService<RateOrgNote>
    {
        private readonly OrganizationsWaterSupplyContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        public CachedRateOrgNotesService(OrganizationsWaterSupplyContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        public void AddData(string cacheKey)
        {
            IEnumerable<RateOrgNote> RateOrgNotes = _dbContext.RateOrgNotes.Include(x => x.Rate).Include(x => x.Organization).ToList();
            if (RateOrgNotes != null)
            {
                _memoryCache.Set(cacheKey, RateOrgNotes, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(270)
                });

            }

        }
        public IEnumerable<RateOrgNote> GetData(string cacheKey)
        {
            IEnumerable<RateOrgNote> rateOrgNotes;
            if (!_memoryCache.TryGetValue(cacheKey, out rateOrgNotes))
            {
                rateOrgNotes = _dbContext.RateOrgNotes.Include(x => x.Organization).Include(x => x.Rate).ToList();
                if (rateOrgNotes != null)
                {
                    _memoryCache.Set(cacheKey, rateOrgNotes,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(270)));
                }
            }
            return rateOrgNotes;
        }
        public void UpdateData(string cacheKey)
        {
            IEnumerable<RateOrgNote> rateOrgNotes;
                rateOrgNotes = _dbContext.RateOrgNotes.Include(x => x.Rate).Include(x => x.Organization).ToList();
                    _memoryCache.Set(cacheKey, rateOrgNotes,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(270)));
        }
    }
}

