using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories
{
    [BindProperties]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        //  [BindProperty]
        public Category Category1 { get; set; }
        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;

        }

        public void OnGet(int? id)
        {
            if (id != null && id != 0)
            {
                Category1 = _db.Categories.Find(id);
            }
        }



        public IActionResult OnPost()
        {
            Category? o = _db.Categories.Find(Category1.Id);
                if(o==null)
            {
                return NotFound();
            }
                _db.Categories.Remove(o);
            _db.SaveChanges();
            return RedirectToPage("Index");

        }



    }
}
