using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Van_Authentication.Models;
using Van_Authentication.Services;

namespace Van_Authentication.Pages.Admin.Routes.Welds
{
    public class CreateModel : PageModel
    {
        private readonly Van_Authentication.Services.ApplicationDbContext _context;

        public CreateModel(Van_Authentication.Services.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["AuditRouteId"] = new SelectList(_context.AuditRoutes, "AuditRouteId", "AuditRouteId");
        ViewData["WeldId"] = new SelectList(_context.Welds, "WeldID", "WeldType");
            return Page();
        }

        [BindProperty]
        public AuditRouteWeld AuditRouteWeld { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.AuditRouteWelds.Add(AuditRouteWeld);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
