using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Van_Authentication.Models.Notes;
using Van_Authentication.Services;

namespace Van_Authentication.Pages.Engineering
{
    public class DeleteModel : PageModel
    {
        private readonly Van_Authentication.Services.ApplicationDbContext _context;

        public DeleteModel(Van_Authentication.Services.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public EngineeringNote EngineeringNote { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var engineeringnote = await _context.EngineeringNotes.FirstOrDefaultAsync(m => m.Id == id);

            if (engineeringnote == null)
            {
                return NotFound();
            }
            else
            {
                EngineeringNote = engineeringnote;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var engineeringnote = await _context.EngineeringNotes.FindAsync(id);
            if (engineeringnote != null)
            {
                EngineeringNote = engineeringnote;
                _context.EngineeringNotes.Remove(EngineeringNote);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
