using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BulkyWeb.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class ProductController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork db , IWebHostEnvironment webHostEnvironment )
        {
            _unitOfWork = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();        
         
            return View(objProductList);
        }

        public IActionResult Upsert(int? id)  // do both update and insert/create
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Id.ToString()
                }),
                Product = new Product()
            };
            if(id== null || id == 0)  // create function
            {
                return View(productVM);
            }
            else   // update
            {
                productVM.Product = _unitOfWork.Product.Get(x => x.Id == id);
                return View(productVM);

            }
           
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file) //update insert
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\images\product" +fileName;
                }
                _unitOfWork.Product.Add(productVM.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product Added..!!";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Id.ToString()
                });
                 return View(productVM);
            }
           
        }
        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    // Category? categoryFromDb = _categoryRepo.Get(u => u.Id == id);
        //    // Category? categoryFromDb = _categoryRepo.Categories.FirstOrDefault(c => c.Id == id);
        //    // Category categoryFromDb = _db.Categories.FirstOrDefault(c => c.Id == id);
        //    Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
        //    if (productFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(productFromDb);
        //}

        //[HttpPost]
        //public IActionResult Edit(Product o)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Update(o);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Product Updated..!!";
        //        return RedirectToAction("Index");

        //    }
        //    return View();

        //}

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? productFromDb = _unitOfWork.Product.Get(c => c.Id == id);

            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? o = _unitOfWork.Product.Get(u => u.Id == id);
            if (o == null)
            { return NotFound(); }
            _unitOfWork.Product.Remove(o);
            _unitOfWork.Save();
            TempData["success"] = "Product bye bye..!!";
            return RedirectToAction("Index");

        }
    }
}
