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

namespace Van_Authentication.Pages.VAN.Tcps
{
    [Authorize(Roles = "manager, coordinator, supervisor")]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Tcp Tcp { get; set; } = default!;
        public string? Graphic { get; set; }
        public string? Auditor { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tcp = await _context.Tcps.Include(x => x.WeldConcerns).FirstOrDefaultAsync(m => m.TcpId == id);
            var audit = tcp.WeldConcerns.Select(x => x.AuditID).FirstOrDefault();
            Auditor = await _context.Audits.Where(x => x.AuditID == audit).Select(m => m.Auditor).FirstOrDefaultAsync();
            if (tcp == null)
            {
                return NotFound();
            }
            else
            {
                Tcp = tcp;
            }
            //get graphic

            var line = tcp.WeldConcerns.Select(x => x.Line).FirstOrDefault();
            var station = tcp.WeldConcerns.Select(x => x.Station).FirstOrDefault();
            var robot = tcp.WeldConcerns.Select(x => x.RobotNumber).FirstOrDefault();
            var style = tcp.WeldConcerns.Select(x => x.Style).FirstOrDefault();
            Graphic = await _context.Robots.Where(m => m.Line.Equals(line) && m.Station == station && m.RobotNumber == robot && m.Style.Equals(style)).Select(m => m.Graphic).FirstOrDefaultAsync();

            return Page();
        }
    }
}
