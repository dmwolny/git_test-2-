using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Van_Authentication.Models;
using Van_Authentication.Services;

namespace Van_Authentication.Pages.Audits.Welds
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
        ViewData["AuditID"] = new SelectList(_context.Audits, "AuditID", "Auditor");
        ViewData["TcpID"] = new SelectList(_context.Tcps, "TcpId", "Status");
            return Page();
        }

        [BindProperty]
        public WeldConcern WeldConcern { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.WeldConcerns.Add(WeldConcern);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
