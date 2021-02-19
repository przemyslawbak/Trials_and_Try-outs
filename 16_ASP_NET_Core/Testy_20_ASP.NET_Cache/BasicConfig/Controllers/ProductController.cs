using BasicConfig.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace BasicConfig.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context, IMemoryCache memoryCache)
        {
            _cache = memoryCache;
            _context = context;
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

        public ViewResult Index() => View(_context.Products);
        public IActionResult Other()
        {
            int x = GetUserClicks();
            x++;
            SetUserClicks(x);

            return View(x);
        }
    }
}
