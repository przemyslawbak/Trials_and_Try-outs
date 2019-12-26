using BasicConfig.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicConfig.Models.ViewModels;

namespace BasicConfig.Controllers
{
    public class ProductController : Controller
    {
        ProductViewModel productVM = new ProductViewModel();
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }
        public ViewResult Index()
        {
            var products = _context.Products;
            return View(products);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productDb = await _context.Products
                .Include(i => i.CPs)
                .ThenInclude(i => i.Client)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ProductID == id);
            if (productDb == null)
            {
                return NotFound();
            }
            var productsClients = productDb.CPs.Select(c => c.ClientID);
            //ładowanie VM
            productVM.ProductID = productDb.ProductID;
            productVM.Name = productDb.Name;
            productVM.Description = productDb.Description;
            productVM.Price = productDb.Price;
            productVM.ClientIds = productsClients.ToArray(); //array Clinets z CPs
            productVM.Clients = PopulateCliensList();
            return View(productVM); //zwróć VM
        }

        //POST Edit
        [HttpPost, ActionName("Edit")]
        public ActionResult Edit(ProductViewModel productVM)
        {
            if (productVM == null)
            {
                return RedirectToAction(nameof(Index));
            }

            productVM.Clients = PopulateCliensList();//dropdown
            int? productID = productVM.ProductID;
            //passing dropdown selected items to the collection
            List<SelectListItem> selectedItems = productVM.Clients.Where(p => productVM.ClientIds.Contains(int.Parse(p.Value))).ToList();


            //dla nowego
            if (productID == 0)
            {
                Product newProduct = new Product();
                newProduct.Name = productVM.Name;
                newProduct.Description = productVM.Description;
                newProduct.Price = productVM.Price;
                _context.Products.Add(newProduct);
                _context.SaveChanges();

                if (selectedItems != null)
                {
                    newProduct.CPs = new List<ClientsProducts>();
                    foreach (var item in selectedItems)
                    {
                        var clientToAdd = new ClientsProducts { ProductID = newProduct.ProductID, ClientID = int.Parse(item.Value) };
                        newProduct.CPs.Add(clientToAdd);
                    }
                }
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            //dla edycji
            return null;
        }

        private List<SelectListItem> PopulateCliensList() //method populating dropdown list
        {
            var query = from t in _context.Clients //getting products context from database
                            orderby t.Name //sorting by names
                            select t; //selecting item (??)
            if (query != null) //if db context var is not null...
            {
                return query.Select(n => new SelectListItem { Text = n.Name, Value = n.ClientID.ToString() }).ToList(); //...creating viewbag that is used for populating dropdown list in view
            }
            return null;
        }

        public ViewResult Create()
        {
            var newProduct = new ProductViewModel
            {
                Name = "Nazwa produktu",
                ClientIds = new int[] { },
                Price = 0,
                Clients = PopulateCliensList(),
                Description = "Opis produktu",
            };
            return View("Edit", newProduct);
        }
    }
}
