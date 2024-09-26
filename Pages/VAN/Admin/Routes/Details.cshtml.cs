using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Van_Authentication.Models;
using Van_Authentication.Services;

namespace Van_Authentication.Pages.VAN.Admin.Routes
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public AuditRoute AuditRoute { get; set; } = default!;
        public IList<RobotWeld> RobotWelds { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auditroute = await _context.AuditRoutes.Include(x => x.PartModel).FirstOrDefaultAsync(m => m.AuditRouteId == id);
            if (auditroute == null)
            {
                return NotFound();
            }
            else
            {
                AuditRoute = auditroute;
                List<RobotWeld> tempWelds = new List<RobotWeld>();
                var welds = await _context.AuditRouteWelds.Where(x => x.AuditRouteId == id).ToListAsync();
                if (welds != null)
                {
                    foreach (var weld in welds)
                    {
                        RobotWeld robotWeld = new RobotWeld();
                        robotWeld = await _context.RobotWelds.Include(m => m.Robot).Include(p => p.Weld).FirstOrDefaultAsync(x => x.WeldID == weld.WeldId);
                        tempWelds.Add(robotWeld);
                    }
                    RobotWelds = tempWelds;
                }
            }
            return Page();
        }
    }
}
