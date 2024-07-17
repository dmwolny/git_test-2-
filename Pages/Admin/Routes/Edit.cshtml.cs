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

namespace Van_Authentication.Pages.Admin.Routes
{
    public class EditModel : PageModel
    {
        private readonly Van_Authentication.Services.ApplicationDbContext _context;

        public EditModel(Van_Authentication.Services.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AuditRoute AuditRoute { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auditroute =  await _context.AuditRoutes.FirstOrDefaultAsync(m => m.AuditRouteId == id);
            if (auditroute == null)
            {
                return NotFound();
            }
            AuditRoute = auditroute;
           ViewData["PartModelId"] = new SelectList(_context.PartModels, "PartModelId", "PartModelName");
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

            _context.Attach(AuditRoute).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuditRouteExists(AuditRoute.AuditRouteId))
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

        private bool AuditRouteExists(int id)
        {
            return _context.AuditRoutes.Any(e => e.AuditRouteId == id);
        }
    }
}
