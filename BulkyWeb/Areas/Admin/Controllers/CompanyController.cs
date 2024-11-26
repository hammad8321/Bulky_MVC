using Bulky.DataAccess.Repository.IRepository;
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
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork db)
        {
            _unitOfWork = db;

        }

        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();

            return View(objCompanyList);
        }

        public IActionResult Upsert(int? id)  // do both update and insert/create
        {

            if (id == null || id == 0)  // create function
            {
                return View(new Company());
            }
            else   // update
            {
                Company companyObj = _unitOfWork.Company.Get(x => x.Id == id);
                return View(companyObj);

            }

        }

        [HttpPost]
        public IActionResult Upsert(Company CompanyObj) //update insert
        {
            if (ModelState.IsValid)
            {

                if (CompanyObj.Id == 0)
                {
                    _unitOfWork.Company.Add(CompanyObj);
                }
                else
                {
                    _unitOfWork.Company.Update(CompanyObj);
                }

                _unitOfWork.Save();
                TempData["success"] = "Company Added..!!";
                return RedirectToAction("Index");
            }
            else
            {

                return View(CompanyObj);
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
        //    Company? CompanyFromDb = _unitOfWork.Company.Get(u => u.Id == id);
        //    if (CompanyFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(CompanyFromDb);
        //}

        //[HttpPost]
        //public IActionResult Edit(Company o)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Company.Update(o);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Company Updated..!!";
        //        return RedirectToAction("Index");

        //    }
        //    return View();

        //}

        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Company? CompanyFromDb = _unitOfWork.Company.Get(c => c.Id == id);

        //    if (CompanyFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(CompanyFromDb);
        //}
        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeletePOST(int? id)
        //{
        //    Company? o = _unitOfWork.Company.Get(u => u.Id == id);
        //    if (o == null)
        //    { return NotFound(); }
        //    _unitOfWork.Company.Remove(o);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Company bye bye..!!";
        //    return RedirectToAction("Index");

        //}
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {

            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });

        }


        //[HttpGet]
        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted = _unitOfWork.Company.Get(x => x.Id == id);
            if (CompanyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Company.Remove(CompanyToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Deleted successfully" });

            //List<Company> objCompanyList = _unitOfWork.Company.GetAll(includeProperties: "Category").ToList();
            //return Json(new { data = objCompanyList });

        }





        #endregion

    }
}
