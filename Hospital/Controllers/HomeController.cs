using Hospital.Data;
using Hospital.Models;
using Hospital.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
           
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Docters = _db.Docter.Include(u => u.Category),
                Categories = _db.Category
            };
            return View(homeVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateComment(Docter obj)
        {
            if (ModelState.IsValid)
            {
                _db.Docter.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View();
        }
        public IActionResult CreateComment()
        {
           

            return View();
        }
        public IActionResult Details(int id)
        {




            DetailsVM DetailsVM = new DetailsVM()
            {
                Docter = _db.Docter.Include(u => u.Category).Where(u => u.Id == id).FirstOrDefault(),
                
                
            };


            return View(DetailsVM);
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
