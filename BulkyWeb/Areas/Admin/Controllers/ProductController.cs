﻿using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol;
using System.Collections.Generic;

namespace BulkyWeb.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
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
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();        
         
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

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        // delete the old image
                        var oldImagepath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagepath))
                        {
                            System.IO.File.Delete(oldImagepath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\images\product\" +fileName;
                }
                   
                if (productVM.Product.Id==0)
                {
                     _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                     _unitOfWork.Product.Update(productVM.Product);
                }
               
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


      

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll() 
        {

            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });

        }


        //[HttpGet]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted =_unitOfWork.Product.Get(x=>x.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Deleted successfully" });

            //List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            //return Json(new { data = objProductList });

        }





        #endregion

    }
}
