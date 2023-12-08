using OrganizationsWaterSupplyL4.Data;
using OrganizationsWaterSupplyL4.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace OrganizationsWaterSupplyL4.Services
{
    public class CachedModelsService : ICachedService<CounterModel>
    {
        private readonly OrganizationsWaterSupplyContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        public CachedModelsService(OrganizationsWaterSupplyContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        public void AddData(string cacheKey)
        {
            IEnumerable<CounterModel> models = _dbContext.CounterModels.ToList();
            if (models != null)
            {
                _memoryCache.Set(cacheKey, models, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(270)
                });

            }

        }
        public IEnumerable<CounterModel> GetData(string cacheKey)
        {
            IEnumerable<CounterModel> models;
            if (!_memoryCache.TryGetValue(cacheKey, out models))
            {
                models = _dbContext.CounterModels.ToList();
                if (models != null)
                {
                    _memoryCache.Set(cacheKey, models,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(270)));
                }
            }
            return models;
        }
        public void UpdateData(string cacheKey)
        {
            IEnumerable<CounterModel> models;
            models = _dbContext.CounterModels.ToList();
            _memoryCache.Set(cacheKey, models,
            new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(270)));
        }
    }
}

