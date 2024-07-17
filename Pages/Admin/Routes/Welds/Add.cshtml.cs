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
    public class AddModel : PageModel
    {
        private readonly Van_Authentication.Services.ApplicationDbContext _context;

        public AddModel(Van_Authentication.Services.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int? routeID)
        {
        ViewData["AuditRouteId"] = new SelectList(_context.AuditRoutes, "AuditRouteId", "AuditRouteName");
        ViewData["WeldId"] = new SelectList(_context.Welds, "WeldID", "WeldID");
        if(routeID != null)
            {
                id = (int)routeID;

            }
        return Page();
        }

        [BindProperty]
        public AuditRouteWeld AuditRouteWeld { get; set; } = default!;
        [BindProperty]
        public int id { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            AuditRouteWeld.AuditRouteId = id;
            _context.AuditRouteWelds.Add(AuditRouteWeld);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Details", new { id = AuditRouteWeld.AuditRouteId});
        }
    }
}
