using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Van_Authentication.Models;
using Van_Authentication.Services;

namespace Van_Authentication.Pages.Admin.Models
{
    public class DetailsModel : PageModel
    {
        private readonly Van_Authentication.Services.ApplicationDbContext _context;

        public DetailsModel(Van_Authentication.Services.ApplicationDbContext context)
        {
            _context = context;
        }

        public PartModel PartModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partmodel = await _context.PartModels.Include(x => x.WorkStation).FirstOrDefaultAsync(m => m.PartModelId == id);
            if (partmodel == null)
            {
                return NotFound();
            }
            else
            {
                PartModel = partmodel;
            }
            return Page();
        }
    }
}
