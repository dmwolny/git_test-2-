using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Van_Authentication.Models;
using Van_Authentication.Services;

namespace Van_Authentication.Pages.Admin.Routes
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
        ViewData["PartModelId"] = new SelectList(_context.PartModels, "PartModelId", "PartModelName");
            return Page();
        }

        [BindProperty]
        public AuditRoute AuditRoute { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.AuditRoutes.Add(AuditRoute);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
