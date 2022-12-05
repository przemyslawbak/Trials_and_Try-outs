using BasicConfig.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;

namespace BasicConfig.Controllers
{
    //https://dotnetcoretutorials.com/2017/03/05/using-inmemory-cache-net-core/
    public class ProductController : Controller
    {
        private IMemoryCache _cache;
        private ApplicationDbContext _context;

        public ProductController(IMemoryCache memoryCache, ApplicationDbContext ctx)
        {
            _cache = memoryCache;
            _context = ctx;
        }

        public IActionResult Index()
        {
            string companyName = GetCompanyName();
            return View();
        }

        public IActionResult StageFirst()
        {
            string companyName = GetCompanyName();
            return View();
        }

        public IActionResult StageSecond()
        {
            string companyName = GetCompanyName();
            return View();
        }

        public IActionResult StageThird()
        {
            string companyName = GetCompanyName();
            return View();
        }
        public string GetCompanyName()
        {
            string result;
            //https://docs.microsoft.com/en-us/aspnet/core/performance/caching/memory?view=aspnetcore-3.1
            // Look for cache key.
            if (!_cache.TryGetValue(CacheKeys.CompanyName, out result))
            {
                Audit audit = _context.AuditRecords.FirstOrDefault();
                // Key not in cache, so get data.
                result = audit.AreaAccessed;

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromMinutes(60));

                // Save data in cache.
                _cache.Set(CacheKeys.CompanyName, result, cacheEntryOptions);
            }

            return result;
        }
    }
    public static class CacheKeys
    {
        public static string CompanyName { get { return "_CompanyName"; } }
    }

}
