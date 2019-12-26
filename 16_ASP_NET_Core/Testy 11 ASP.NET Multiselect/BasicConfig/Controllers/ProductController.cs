using BasicConfig.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicConfig.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Protocols;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace BasicConfig.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        ProductViewModel productVM = new ProductViewModel(); //viewmodel instance
        int[] beforeIds = new int[] { };//!!!!!
        int[] afterIds = new int[] { };//!!!!!!
        int[] newIds = new int[] { };//!!!!!!
        private readonly ApplicationDbContext _context; //database context
        public ProductController(IProductRepository repo, ApplicationDbContext context)
        {
            repository = repo;
            _context = context; //context for controller
        }
        private List<SelectListItem> PopulateProducts() //method populating dropdown list
        {
            var techQuery = from t in _context.Brands //getting products context from database
                            orderby t.Name //sorting by names
                            select t; //selecting item (??)
            if (techQuery != null) //if db context var is not null...
            {
                return techQuery.Select(n => new SelectListItem { Text = n.Name, Value = n.BrandID.ToString() }).ToList(); //...creating viewbag that is used for populating dropdown list in view
            }
            return null;
        }
        //GET
        public ViewResult Index()
        {
            return View(_context.Products);//ok
        }
        public ActionResult Edit(int? productId)
        {
            var productDb = _context.Products.FirstOrDefault(o => o.ProductID == productId);
            if (productDb == null)
            {
                return NotFound();
            }
            //do repo
            string[] zmiana = productDb.BradIds.Split(',');
            int[] zmieniony = new int[zmiana.Length-1];
            for (int i = 0; i < zmiana.Length; i++)
            {
                if (zmiana[i] != "")
                {
                    zmieniony[i] = Int32.Parse(zmiana[i]);
                }
            }
            //
            //loading VM
            productVM.ProductID = productDb.ProductID;
            productVM.Name = productDb.Name;
            productVM.Category = productDb.Category;
            productVM.Description = productDb.Description;
            productVM.Price = productDb.Price;
            productVM.BrandIds = zmieniony;
            productVM.Brands = PopulateProducts(); //populating dropdown
            
            return View(productVM); //returning viewmodel
        }
        //POST (I belive that here I have screwed something a lot, I do not understand everything what is going on in this action)
        [HttpPost, ActionName("Edit")]
        public ActionResult Edit(ProductViewModel productVM)
        {
            
            productVM.Brands = PopulateProducts();//productVM.Products przybiera to co zwraca dropdown
            int? productId = productVM.ProductID;
            //passing dropdown selected items to the collection
            List<SelectListItem> selectedItems = productVM.Brands.Where(p => productVM.BrandIds.Contains(int.Parse(p.Value))).ToList();
            string tekst = "";
            foreach (var item in selectedItems)
            {
                tekst = tekst + item.Value.ToString() + ",";
            }
            if (productId == 0)
            {
                Product newProduct = new Product();
                repository.SaveProduct(newProduct, productVM, tekst);
                _context.Products.Add(newProduct);
                _context.SaveChanges();
                //new product`s ID is assigned after saving data context!!!!!
                newIds = productVM.BrandIds;
                //Entering Brand context and adding this product from ProductIds
                foreach (var id in newIds)
                {
                    Brand brandDb = _context.Brands.FirstOrDefault(o => o.BrandID == id);
                    //add product ID to ProductIds
                    brandDb.ProductIds = brandDb.ProductIds + newProduct.ProductID.ToString() + ",";
                }
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                Product productDb = _context.Products.FirstOrDefault(o => o.ProductID == productId);
                //do repo
                //splitting string into integers
                string[] zmiana = productDb.BradIds.Split(',');//!!!!!
                int[] zmieniony = new int[zmiana.Length-1];
                for (int i = 0; i < zmiana.Length; i++)
                {
                    if (zmiana[i] != "")
                    {
                        zmieniony[i] = Int32.Parse(zmiana[i]);
                    }
                }
                //
                //arrays for BrandIds before posting Edit and after posting Edit
                beforeIds = zmieniony;//!!!!!!
                afterIds = productVM.BrandIds;//!!!!!!!!!
                //searching for pairs before and after posting BrandIds
                foreach (var before in beforeIds)
                {
                    foreach (var after in afterIds)
                    {
                        if (before == after)
                        {
                            //removing from arrays paired Ids
                            beforeIds = beforeIds.Where(val => val != before).ToArray();
                            afterIds = afterIds.Where(val => val != after).ToArray();
                        }
                    }
                }
                //Entering Brand context and removing/adding this product from ProductIds
                foreach (var id in beforeIds)
                {
                    Brand brandDb = _context.Brands.FirstOrDefault(o => o.BrandID == id);
                    //remove product ID from ProductIds
                    int idIndex = brandDb.ProductIds.IndexOf(productId.ToString());
                    brandDb.ProductIds = brandDb.ProductIds.Remove(idIndex, productId.ToString().Length + 1);
                }
                foreach (var id in afterIds)
                {
                    Brand brandDb = _context.Brands.FirstOrDefault(o => o.BrandID == id);
                    //add product ID to ProductIds
                    brandDb.ProductIds = brandDb.ProductIds + productId.ToString() + ",";
                }
                repository.SaveProduct(productDb, productVM, tekst);
                _context.SaveChanges();
                return RedirectToAction("Index", productVM);
            }
            
        }
        

        public ViewResult Create()
        {
            var newProduct = new ProductViewModel
            {
                BrandIds = new int[] { },
                Category = "nowy",
                Description = "nowy",
                Name = "nowy",
                Price = 0,
                Brands = PopulateProducts()
            };
            return View("Edit", newProduct);
        }

        [HttpPost]
        public IActionResult Delete(int productID)
        {
            Product deleteProduct = repository.DeleteProduct(productID);
            if (deleteProduct != null)
            {
                TempData["message"] = $"Deleted {deleteProduct.Name}.";
            }
            return RedirectToAction("Index");
        }
    }
}

