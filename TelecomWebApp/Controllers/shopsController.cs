using Microsoft.AspNetCore.Mvc;
using TelecomWebApp.Models;
using System.Collections.Generic;

namespace TelecomWebApp.Controllers
{
    public class ShopsController : Controller
    {
        public IActionResult Index()
        {
            // Example data - In a real application, this would come from a database
            var shops = new List<shop>
            {
                new shop { shopID = 1, name = "Shop A", Category = "Electronics" },
                new shop { shopID = 2, name = "Shop B", Category = "Clothing" },
                new shop { shopID = 3, name = "Shop C", Category = "Groceries" }
            };

            // Pass the list of shops to the view
            return View(shops);
        }
    }
}
