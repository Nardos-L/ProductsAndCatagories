using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductsAndCatagories.Models;

namespace ProductsAndCatagories.Controllers
{
    public class HomeController : Controller
    {
        private ProductsAndCatagoriesContext db;
        public HomeController(ProductsAndCatagoriesContext context)
        {
            db = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            List<Product> allProducts = db.Products.ToList();
            ViewBag.allProducts = allProducts;
            return View();
        }

        [HttpPost("/create")]
        public IActionResult createProduct(Product newProduct)
        {
            if(ModelState.IsValid == false)
            {
                return View("Index");
            }
            db.Add(newProduct);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet("/categories")]
        public IActionResult Categories()
        {
            List<Catagory> allCategories = db.Catagories.ToList();
            ViewBag.allCategories = allCategories;
            return View("Category");
            
        }

        [HttpPost("/create/category")]
        public IActionResult CreateCategory(Catagory newCatagory)
        {
            if(ModelState.IsValid == false)
            {
                return RedirectToAction("Categories");
            }
            db.Add(newCatagory);
            db.SaveChanges();

            return RedirectToAction("Categories");
        }

        
        [HttpGet("/products/{productId}")]
        public IActionResult Details(int productId)
        {
            Product Product = db.Products
            .Include(p => p.ProductAssociations)
                    .ThenInclude(proass => proass.Catagory)
                .FirstOrDefault(p => p.ProductId == productId);

            List<Catagory> allCategories = db.Catagories.ToList();
            ViewBag.allCategories = allCategories;
            List<Catagory> catagoriesToAdd = new List<Catagory>();

            ViewBag.Catagories = db.Catagories.ToList()
                .Except(db.Catagories
                .Where(Catagory => Catagory.CatagoryAssociations
                .Any(Association => Association.ProductId == productId))
                .ToList());
            
            return View("ProductDetails",Product);
        }

        [HttpPost("/add/category/{productId}")]
        public IActionResult AddCategory(int productId, int catagoryId)
        {
            //Product selectedProduct = db.Products.Include(p => p.ProductAssociations).FirstOrDefault(p => p.ProductId == productId);

            bool selectedProduct = db.Associations.Any(a => a.ProductId == productId && a.CatagoryId == catagoryId );
            
            if(!selectedProduct)
            {
                Association connection= new Association()
                {
                    CatagoryId = catagoryId,
                    ProductId = productId
                };
                db.Associations.Add(connection);
                db.SaveChanges();
                return RedirectToAction("Details",new {productId = productId});
            }
            
            return View("Details", new {productId = productId});
        }


        [HttpGet("/catagories/{catagoryId}")]
        public IActionResult CatagoryDetails(int catagoryId)
        {
            Catagory catagory = db.Catagories
            .Include(c => c.CatagoryAssociations)
                    .ThenInclude(proass => proass.Product)
                .FirstOrDefault(c => c.CatagoryId == catagoryId);

            List<Product> allProducts = db.Products.ToList();
            ViewBag.allProducts = allProducts;
            List<Product> productsToAdd = new List<Product>();

            ViewBag.Products = db.Products.ToList()
                .Except(db.Products
                .Where(Product => Product.ProductAssociations
                .Any(Association => Association.CatagoryId == catagoryId))
                .ToList());
            
            return View("CatagoryDetails",catagory);
        }

        [HttpPost("/add/product/{catagoryID}")]
        public IActionResult AddProduct(int productId, int catagoryId)
        {
            //Product selectedProduct = db.Products.Include(p => p.ProductAssociations).FirstOrDefault(p => p.ProductId == productId);

            bool selectedCatagory = db.Associations.Any(a => a.ProductId == productId && a.CatagoryId == catagoryId );
            
            if(!selectedCatagory)
            {
                Association connection= new Association()
                {
                    CatagoryId = catagoryId,
                    ProductId = productId
                };
                db.Associations.Add(connection);
                db.SaveChanges();
                return RedirectToAction("CatagoryDetails",new {catagoryId = catagoryId});
            }
            
            return View("CatagoryDetails", new {catagoryId = catagoryId});
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
