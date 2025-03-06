using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db) 
        { 
            _db = db;
        
        }
        public IActionResult Index()
        {
            var objCatergoryList = _db.Categories.ToList();
            return View(objCatergoryList);
        }
        
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category categoryobj)
        {
            _db.Categories.Add(categoryobj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
