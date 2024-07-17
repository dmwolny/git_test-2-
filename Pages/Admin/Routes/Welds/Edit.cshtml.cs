using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Van_Authentication.Models;
using Van_Authentication.Services;

namespace Van_Authentication.Pages.Admin.Routes.Welds
{
    public class EditModel : PageModel
    {
        private readonly Van_Authentication.Services.ApplicationDbContext _context;

        public EditModel(Van_Authentication.Services.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AuditRouteWeld AuditRouteWeld { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auditrouteweld =  await _context.AuditRouteWelds.FirstOrDefaultAsync(m => m.AuditRouteId == id);
            if (auditrouteweld == null)
            {
                return NotFound();
            }
            AuditRouteWeld = auditrouteweld;
           ViewData["AuditRouteId"] = new SelectList(_context.AuditRoutes, "AuditRouteId", "AuditRouteId");
           ViewData["WeldId"] = new SelectList(_context.Welds, "WeldID", "WeldType");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(AuditRouteWeld).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuditRouteWeldExists(AuditRouteWeld.AuditRouteId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool AuditRouteWeldExists(int id)
        {
            return _context.AuditRouteWelds.Any(e => e.AuditRouteId == id);
        }
    }
}
