using BasicConfig.Models;
using BasicConfig.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace BasicConfig.Controllers
{
    public class ProductController : Controller
    {
        private readonly IServicesProvider _provider;
        private readonly IMemoryCache _cache;
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context, IMemoryCache memoryCache, IServicesProvider provider)
        {
            _cache = memoryCache;
            _context = context;
            _provider = provider;
        }

        public int GetUserClicks()
        {
            int result;
            if (!_cache.TryGetValue(CacheKeys.UserClicks, out result))
            {
                result = 0;

                SetUserClicks(0);
            }

            return result;
        }

        public void SetUserClicks(int value)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(60));
            _cache.Set(CacheKeys.UserClicks, value, cacheEntryOptions);
        }

        public ViewResult Index()
        {
            var dupa = _provider.Get("4programmers");
            return View(_context.Products);
        }
        public IActionResult Other()
        {
            int x = GetUserClicks();
            x++;
            SetUserClicks(x);

            return View(x);
        }
    }
}
