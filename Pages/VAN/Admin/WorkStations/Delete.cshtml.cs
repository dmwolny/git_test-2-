using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Van_Authentication.Models;
using Van_Authentication.Services;

namespace Van_Authentication.Pages.Admin.WorkStations
{
    public class DeleteModel : PageModel
    {
        private readonly Van_Authentication.Services.ApplicationDbContext _context;

        public DeleteModel(Van_Authentication.Services.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public WorkStation WorkStation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workstation = await _context.WorkStations.FirstOrDefaultAsync(m => m.WorkStationId == id);

            if (workstation == null)
            {
                return NotFound();
            }
            else
            {
                WorkStation = workstation;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workstation = await _context.WorkStations.FindAsync(id);
            if (workstation != null)
            {
                WorkStation = workstation;
                _context.WorkStations.Remove(WorkStation);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
