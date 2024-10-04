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
            //   var objCategoryList = _db.Categories.ToList();
            List<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }



        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category o)
        {
            //if (o.Name == o.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("name", "nams is same ");
            //}
            //if (o.Name!=null &&  o.Name.ToLower()=="test")
            //{
            //    ModelState.AddModelError("", "no Test value plz ");
            //}
            if (ModelState.IsValid)
            {
                _db.Categories.Add(o);
                _db.SaveChanges();
                TempData["success"] = "Category Added..!!";
                return RedirectToAction("Index");

            }
            return View();
            
        }

        public IActionResult Edit( int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _db.Categories.Find(id);
         // Category categoryFromDb = _db.Categories.FirstOrDefault(c => c.Id == id);
         // Category categoryFromDb = _db.Categories.Where(u=>u.Id == id).FirstOrDefault();
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
                _db.Categories.Update(o);
                _db.SaveChanges();
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
            Category? categoryFromDb = _db.Categories.Find(id);
           
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost , ActionName("Delete")]
        public IActionResult DeletePOST(int ? id)
        {   
            Category? o = _db.Categories.Find(id);
            if (o == null)
                { return NotFound(); } 
            _db.Categories.Remove(o); 
            _db.SaveChanges();
            TempData["success"] = "Category bye bye..!!";
            return RedirectToAction("Index");
            

          

        }

    }
}
