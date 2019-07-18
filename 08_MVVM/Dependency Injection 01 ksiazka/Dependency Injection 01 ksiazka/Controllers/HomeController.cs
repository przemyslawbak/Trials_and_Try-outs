using Dependency_Injection_01_ksiazka.Infrastructure;
using Dependency_Injection_01_ksiazka.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dependency_Injection_01_ksiazka.Controllers
{
    public class HomeController : Controller
    {
        private IRepository repository;
        public HomeController(IRepository repo) => repository = repo;
        public ViewResult Index() => View(repository.Products);
    }
}
