using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        //  [BindProperty]
        public Category Category1 { get; set; }
        public CreateModel(ApplicationDbContext db)
        {
            _db = db;

        }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            _db.Categories.Add(Category1);
            _db.SaveChanges();
            return RedirectToPage("Index");
        }
    }
}
