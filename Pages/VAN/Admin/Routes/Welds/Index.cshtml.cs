using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Van_Authentication.Models;
using Van_Authentication.Services;

namespace Van_Authentication.Pages.VAN.Admin.Routes.Welds
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<AuditRouteWeld> AuditRouteWeld { get; set; } = default!;

        public async Task OnGetAsync()
        {
            AuditRouteWeld = await _context.AuditRouteWelds
                .Include(a => a.AuditRoute)
                .Include(a => a.Weld).ToListAsync();
        }
    }
}
