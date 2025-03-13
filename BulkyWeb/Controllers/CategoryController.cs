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

        //Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category categoryobj)
        {
            if (ModelState.IsValid) 
            {
                _db.Categories.Add(categoryobj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        //Edit
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0) 
            {
                return NotFound();
            }
            
            //Find the id
            Category? categoryFromDB = _db.Categories.Find(id);

            //There's Two more approaches to finding the id
                //Category? categoryFromDB = _db.Categories.FirstOrDefault(u => u.Id == id);
                //Category? categoryFromDB = _db.Categories.Where(u => u.Id == id).FirstOrDefault();

            if (categoryFromDB == null)
            {
                return NotFound();
            }
            return View(categoryFromDB);
        }

        [HttpPost]
        public IActionResult Edit(Category categoryobj)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(categoryobj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        //Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //Find the id
            Category? categoryFromDB = _db.Categories.Find(id);

            if (categoryFromDB == null)
            {
                return NotFound();
            }
            return View(categoryFromDB);
        }

        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? categoryobj = _db.Categories.Find(id);
            if (categoryobj == null)
            {
                return NotFound();

            }
            _db.Categories.Remove(categoryobj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
