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
    public class DetailsModel : PageModel
    {
        private readonly Van_Authentication.Services.ApplicationDbContext _context;

        public DetailsModel(Van_Authentication.Services.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
