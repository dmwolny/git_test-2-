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

namespace Van_Authentication.Pages.Audits.Welds
{
    [Authorize(Roles = "manager, coordinator")]
    public class DetailsModel : PageModel
    {
        private readonly Van_Authentication.Services.ApplicationDbContext _context;

        public DetailsModel(Van_Authentication.Services.ApplicationDbContext context)
        {
            _context = context;
        }

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
            else
            {
                WeldConcern = weldconcern;
            }
            return Page();
        }
    }
}
