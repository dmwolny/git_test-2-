using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Van_Authentication.Models;
using Van_Authentication.Services;

namespace Van_Authentication.Pages.VAN.Audits.Welds
{
    [Authorize(Roles = "manager, coordinator")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public WeldConcern WeldConcern { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weldconcern = await _context.WeldConcerns.FirstOrDefaultAsync(m => m.WeldConcernID == id);
            if (weldconcern == null)
            {
                return NotFound();
            }
            WeldConcern = weldconcern;
            ViewData["AuditID"] = new SelectList(_context.Audits, "AuditID", "Auditor");
            ViewData["TcpID"] = new SelectList(_context.Tcps, "TcpId", "TcpId");
            ViewData["Defect"] = new SelectList(_context.Defects, "DefectDesc", "DefectDesc");
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

            _context.Attach(WeldConcern).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeldConcernExists(WeldConcern.WeldConcernID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("../Edit", new { id = WeldConcern.AuditID });
        }

        private bool WeldConcernExists(int id)
        {
            return _context.WeldConcerns.Any(e => e.WeldConcernID == id);
        }
    }
}
