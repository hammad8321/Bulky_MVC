using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.Id.ToString()
            });
            return View(objProductList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product Added..!!";
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
            Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
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
