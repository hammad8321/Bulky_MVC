using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories
{
    [BindProperties]
    public class EditModel : PageModel
    {
        
        private readonly ApplicationDbContext _db;
        //  [BindProperty]
        public Category Category1 { get; set; }
        public EditModel(ApplicationDbContext db)
        {
            _db = db;

        }

        public void OnGet( int? id)
        {
            if (id!=null && id!=0) 
            {
                Category1 = _db.Categories.Find(id);
            }
        }



        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(Category1);
                _db.SaveChanges();
                //TempData["success"] = "Categery Updated Horray...!!!";
                return RedirectToPage("Index");
                    
            }
            return Page();
    
           
        }
    
   
    }
}
