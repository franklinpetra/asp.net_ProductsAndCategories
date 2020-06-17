using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductsAndCategories.Models;
using Microsoft.AspNetCore.Http; 
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity; // This is the password hasher//

namespace ProductsAndCategories.Controllers
{
    public class HomeController : Controller
    {

        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext = context;
        } 
        //  CRUD operations for Products and Categories //
        [HttpGet("")]
        public ViewResult Index()
        {
            Wrap WrapMethod = new Wrap();
            WrapMethod.AllProducts = dbContext.Products
                // Include the intermediary table
                .Include(p => p.CategoryProductIsPartOf)
                // And from that middle table, include the object on the OTHER side of the
                // many to many relationship
                .ThenInclude(i => i.Category)
                .ToList();
            WrapMethod.AllCategories = dbContext.Categories
                // Including the navigation property from Category
                .Include(c => c.ProductsInCategory)
                // Including the navigation property of "Product" in the ProductsInCategory class that was already included
                .ThenInclude(i => i.Product)
                .ToList();
            return View("Index", WrapMethod);
        }


    //      {
    //          Decimal value = 106.25m;
    //          Console.WriteLine("Current Culture: {0}",
    //                         CultureInfo.CurrentCulture.Name);
    //          Console.WriteLine("Currency Symbol: {0}",
    //                         NumberFormatInfo.CurrentInfo.CurrencySymbol);
    //          Console.WriteLine("Currency Value:  {0:C2}", value);
//          }
// //      The above script renders the following output:
        // //       Current Culture: en-US
        // //       Currency Symbol: $
        // //       Currency Value:  $106.25

        [HttpPost("categories/join")]
        public IActionResult JoinCategory(Wrap FromForm)
        {
            if(ModelState.IsValid)
            {
                // Stop duplicates when combining in a Many To Many join //
                if(dbContext.Associations.Any(p => p.ProductId == FromForm.Form.ProductId && p.ProductId == FromForm.Form.ProductId))
                {
                    ModelState.AddModelError("Form.ProductId", "We have that product already in that category. Can only have one listing per product");
                    return Index();
                }

                dbContext.Add(FromForm.Form);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return Index();
            }
        }
        [HttpGet("products/new")]
        public ViewResult NewProduct()
        {
            return View("NewProduct");
        }

        [HttpPost("products/create")]
        public IActionResult CreateProduct(Product FromForm)
        {
            if(ModelState.IsValid)
            {
                dbContext.Products.Add(FromForm);
                dbContext.SaveChanges();
                // ViewBag.Price=Decimal.size.ToString("d1");
                return RedirectToAction("Index");
            }
            else
            {
                return NewProduct();
            }
        }
        //The first line is the url being delivered. 
        //The second is the action. 
        //The third is the page being rendered.
        [HttpGet("category/new")]
        public ViewResult NewCategory()
        {
            return View("NewCategory");
        }


        [HttpPost("categories/create")]
        public IActionResult CreateCategory(Category FromForm)
        {
            if(ModelState.IsValid)
            {
                dbContext.Categories.Add(FromForm);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return NewCategory();
            }
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


            // TimeDisplay controller //
        [HttpGet("TimeDisplay")]
        public IActionResult TimeDisplay()
        {
            ViewBag.Date=DateTime.Now.ToString("MMM dd,yyyy");
            ViewBag.Time=DateTime.Now.ToString("h:mm tt");
            return View("TimeDisplay");
        }

        [HttpGet("TimeTravel")]
        public IActionResult TimeTravel()
        {
            return View("TimeTravel");
        }

        [HttpGet("Cat")]
        public IActionResult Cat()
        {
            return View("Cat");
        }
        [HttpGet("Pup")]
        public IActionResult Pup()
        {
            return View("Pup");
        }


    }
}

