using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Van_Authentication.Models;
using Van_Authentication.Services;

namespace Van_Authentication.Pages.Admin.Routes.Welds
{
    public class DeleteModel : PageModel
    {
        private readonly Van_Authentication.Services.ApplicationDbContext _context;

        public DeleteModel(Van_Authentication.Services.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AuditRouteWeld AuditRouteWeld { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? routeID, int? weldID)
        {
            if (routeID == null)
            {
                return NotFound();
            }

            var auditrouteweld = await _context.AuditRouteWelds.Include(p => p.AuditRoute).FirstOrDefaultAsync(m => m.AuditRouteId == routeID && m.WeldId == weldID);

            if (auditrouteweld == null)
            {
                return NotFound();
            }
            else
            {
                AuditRouteWeld = auditrouteweld;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? routeID, int? weldID)
        {
            if (routeID == null)
            {
                return NotFound();
            }

            var auditrouteweld = await _context.AuditRouteWelds.FindAsync(routeID, weldID);
            if (auditrouteweld != null)
            {
                AuditRouteWeld = auditrouteweld;
                _context.AuditRouteWelds.Remove(AuditRouteWeld);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("../Details", new { id = routeID});
        }
    }
}
