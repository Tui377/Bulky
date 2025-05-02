using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SD7501Bulky.DataAccess.Repository.IRepository;
using SD7501Bulky.Models;
using SD7501Bulky.Models.ViewModels;
using System.Collections.Generic;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        
        }
        public IActionResult Index()
        {
            var objProductList = _unitOfWork.Product.GetAll().ToList();
            return View(objProductList);
        }

        //Create *****REMOVED for wk7 because we using ViewModel method
        //public IActionResult Create()
        //{
        //    IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
        //    {
        //        Text = u.Name,
        //        Value = u.Id.ToString()

        //    });

        //    // ViewBag.CategoryList = CategoryList; // Remove this
        //    //ViewData["CategoryList"] = CategoryList; // Also to be removed if using ViewModel

        //    return View();
        //}


        //Create ***Wk7***
        public IActionResult Create()
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };

            return View(productVM);
        }

        [HttpPost]
        //public IActionResult Create(Product productobj)
        public IActionResult Create(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                //_unitOfWork.Product.Add(productobj);
                _unitOfWork.Product.Add(productVM.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });

                return View(productVM);
            }
            //return View();
        }

        //Edit
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0) 
            {
                return NotFound();
            }

            //Find the id
            Product? productFromDB = _unitOfWork.Product.Get(u => u.Id == id);
          
            //There's Two more approaches to finding the id
                //Product? productFromDB = _db.Categories.Find(id);
                //Product? productFromDB = _db.Categories.Where(u => u.Id == id).FirstOrDefault();

            if (productFromDB == null)
            {
                return NotFound();
            }
            return View(productFromDB);
        }

        [HttpPost]
        public IActionResult Edit(Product productobj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(productobj);
                _unitOfWork.Save();
                TempData["success"] = "Product update successfully";
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
            Product? productFromDB = _unitOfWork.Product.Get(u => u.Id == id);

            if (productFromDB == null)
            {
                return NotFound();
            }
            return View(productFromDB);
        }

        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? productobj = _unitOfWork.Product.Get(u => u.Id == id);
            if (productobj == null)
            {
                return NotFound();

            }
            _unitOfWork.Product.Remove(productobj);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
