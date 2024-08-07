using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Van_Authentication.Models;
using Van_Authentication.Services;

namespace Van_Authentication.Pages.Tcps
{
    [Authorize(Roles = "manager, coordinator, supervisor")]
    public class DeleteModel : PageModel
    {
        private readonly Van_Authentication.Services.ApplicationDbContext _context;

        public DeleteModel(Van_Authentication.Services.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Tcp Tcp { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tcp = await _context.Tcps.FirstOrDefaultAsync(m => m.TcpId == id);

            if (tcp == null)
            {
                return NotFound();
            }
            else
            {
                Tcp = tcp;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tcp = await _context.Tcps.FindAsync(id);
            if (tcp != null)
            {
                Tcp = tcp;
                _context.Tcps.Remove(Tcp);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
