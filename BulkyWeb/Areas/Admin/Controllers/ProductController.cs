using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork db)
        {
            _unitOfWork = db;
        }

        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();
            return View(objProductList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]

        public IActionResult Create(Product obj)
        {

            if (obj.Title == obj.Description.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");

            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj);
                _unitOfWork.Save();


            }
            return View();
        }
        [HttpPost]
        public IActionResult Edit(Product o)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(o);
                _unitOfWork.Save();
                TempData["success"] = "Product Updated..!!";
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
