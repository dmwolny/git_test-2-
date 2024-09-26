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

namespace Van_Authentication.Pages.VAN.Admin.RepairProcedures
{
    [Authorize(Roles = "manager, coordinator")]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RepairProcedure RepairProcedure { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repairprocedure = await _context.RepairProcedure.FirstOrDefaultAsync(m => m.RepairProcedureId == id);

            if (repairprocedure == null)
            {
                return NotFound();
            }
            else
            {
                RepairProcedure = repairprocedure;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repairprocedure = await _context.RepairProcedure.FindAsync(id);
            if (repairprocedure != null)
            {
                RepairProcedure = repairprocedure;
                _context.RepairProcedure.Remove(RepairProcedure);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
