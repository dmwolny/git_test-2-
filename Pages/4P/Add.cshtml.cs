using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Van_Authentication.Migrations;
using Van_Authentication.Models.FourP;
using Van_Authentication.Services;

namespace Van_Authentication.Pages._4P
{
    public class AddModel : PageModel
    {
        private ApplicationDbContext _context;

        public AddModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Certifiy Certifiy { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Certifications.Add(Certifiy);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
