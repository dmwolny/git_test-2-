using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Van_Authentication.Models.FourP;
using Van_Authentication.Models.Notes;
using Van_Authentication.Services;

namespace Van_Authentication.Pages._4P
{
    public class DeleteModel : PageModel
    {
        private ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public Certifiy Certifiy { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fourp = await _context.Certifications.FirstOrDefaultAsync(m => m.Id == id);

            if (fourp == null)
            {
                return NotFound();
            }
            else
            {
                Certifiy = fourp;
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fourp = await _context.Certifications.FindAsync(id);
            if (fourp != null)
            {
                 Certifiy = fourp;
                _context.Certifications.Remove(Certifiy);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
