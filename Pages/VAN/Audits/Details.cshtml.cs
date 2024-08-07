using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Van_Authentication.Models;
using Van_Authentication.Services;

namespace Van_Authentication.Pages.Audits
{
    [Authorize(Roles = "manager, coordinator")]
    public class DetailsModel : PageModel
    {
        private readonly Van_Authentication.Services.ApplicationDbContext _context;

        public DetailsModel(Van_Authentication.Services.ApplicationDbContext context)
        {
            _context = context;
        }

        public Audit Audit { get; set; } = default!;
        public IList<WeldConcern> WeldConcerns { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<WeldConcern> welds = new List<WeldConcern>();
            var audit = await _context.Audits.FirstOrDefaultAsync(m => m.AuditID == id);
            welds = await _context.WeldConcerns.Where(m => m.AuditID == id).ToListAsync();
            var routeWelds = await _context.AuditRoutes.Where(m => m.AuditRouteName.Equals(audit.Route))
                .Include(x => x.AuditRouteWelds).ThenInclude(y => y.Weld).ToListAsync();

            var okWelds = _context.Welds
                .Join(_context.AuditRouteWelds,
                welds => welds.WeldID,
                routeWelds => routeWelds.WeldId,
                (welds, routeWelds) => new { welds, routeWelds })
                .Join(_context.AuditRoutes,
                combined => combined.routeWelds.AuditRouteId,
                routeAudit => routeAudit.AuditRouteId,
                (combined, routeAudit) => new { combined, routeAudit })
                .Where(m => m.routeAudit.AuditRouteName.Equals(audit.Route)).Select(x => x.combined.welds).ToList();

            foreach(Weld weld in okWelds)
            {
                //if route welds is not found in the weldconcerns list
                if (!welds.Any(x => x.WeldID == weld.WeldID))
                {
                    WeldConcern wc = new WeldConcern();
                    var rbtID = await _context.RobotWelds
                        .Where(x => x.WeldID == weld.WeldID)
                        .Select(y => y.RobotID)
                        .FirstOrDefaultAsync();
                    wc.AuditID = (int)id;
                    wc.WeldID = weld.WeldID;
                    wc.WeldType = weld.WeldType;
                    wc.NuggetSize = weld.NuggetSize;
                    wc.Line = audit.Line;
                    wc.Station = _context.Robots.Where(x => x.RobotID == rbtID).Select(y => y.Station).FirstOrDefault();
                    wc.RobotNumber = _context.Robots.Where(x => x.RobotID == rbtID).Select(y => y.RobotNumber).FirstOrDefault();
                    wc.Style = _context.Robots.Where(x => x.RobotID == rbtID).Select(y => y.Style).FirstOrDefault();
                    wc.Defect = "None";
                    welds.Add(wc);
                } 
            }

            if (audit == null)
            {
                return NotFound();
            }
            else
            {
                Audit = audit;
                WeldConcerns = welds;
            }
            return Page();
        }
    }
}
