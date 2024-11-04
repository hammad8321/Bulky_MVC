using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Bulky.Models.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Bulky.Utility;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork db)
        {
            _unitOfWork = db;
        }



        public IActionResult Index()
        {
            //  var objCategoryList = _db.Categories.ToList();
            // List<Category> objCategoryList = _categoryRepo.GetAll().ToList();
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();//.AsQueryable().ToList();
            return View(objCategoryList);
        }



        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            //if (o.Name == o.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("name", "nams is same ");
            //}
            //if (o.Name!=null &&  o.Name.ToLower()=="test")
            //{
            //    ModelState.AddModelError("", "no Test value plz ");
            //}

            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {

                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category Added..!!";
                return RedirectToAction("Index");

            }
            return View();

        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            // Category? categoryFromDb = _categoryRepo.Get(u => u.Id == id);
            // Category? categoryFromDb = _categoryRepo.Categories.FirstOrDefault(c => c.Id == id);
            // Category categoryFromDb = _db.Categories.FirstOrDefault(c => c.Id == id);
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category o)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(o);
                _unitOfWork.Save();
                TempData["success"] = "Category Updated..!!";
                return RedirectToAction("Index");

            }
            return View();

        }


        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _unitOfWork.Category.Get(c => c.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? o = _unitOfWork.Category.Get(u => u.Id == id);
            if (o == null)
            { return NotFound(); }
            _unitOfWork.Category.Remove(o);
            _unitOfWork.Save();
            TempData["success"] = "Category bye bye..!!";
            return RedirectToAction("Index");




        }

    }
}
