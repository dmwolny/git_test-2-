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
    public class DetailsModel : PageModel
    {
        private readonly Van_Authentication.Services.ApplicationDbContext _context;

        public DetailsModel(Van_Authentication.Services.ApplicationDbContext context)
        {
            _context = context;
        }

        public AuditRouteWeld AuditRouteWeld { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auditrouteweld = await _context.AuditRouteWelds.FirstOrDefaultAsync(m => m.AuditRouteId == id);
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
    }
}
