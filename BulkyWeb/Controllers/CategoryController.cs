using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;
using SD7501Bulky.DataAccess.Repository.IRepository;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        
        }
        public IActionResult Index()
        {
            var objCatergoryList = _unitOfWork.Category.GetAll().ToList();
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
            if (categoryobj.Name == categoryobj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The Display Order cannot exactly match with the name");
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(categoryobj);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully";
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
            Category? categoryFromDB = _unitOfWork.Category.Get(u => u.Id == id);
          
            //There's Two more approaches to finding the id
                //Category? categoryFromDB = _db.Categories.Find(id);
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
                _unitOfWork.Category.Update(categoryobj);
                _unitOfWork.Save();
                TempData["success"] = "Category update successfully";
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
            Category? categoryFromDB = _unitOfWork.Category.Get(u => u.Id == id);

            if (categoryFromDB == null)
            {
                return NotFound();
            }
            return View(categoryFromDB);
        }

        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? categoryobj = _unitOfWork.Category.Get(u => u.Id == id);
            if (categoryobj == null)
            {
                return NotFound();

            }
            _unitOfWork.Category.Remove(categoryobj);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
