using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Van_Authentication.Models;
using Van_Authentication.Services;

namespace Van_Authentication.Pages.Audits
{
    [Authorize(Roles = "manager, coordinator, auditor")]
    public class IndexModel : PageModel
    {
        private readonly Van_Authentication.Services.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(Van_Authentication.Services.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Audit> Audits { get;set; } = default!;
        public string? Line {  get; set; }
        public string? Shift { get; set; }

        public async Task OnGetAsync(string? line, string? shift)
        {
            var user = await _userManager.GetUserAsync(User);
            Line = user.Line;
            Shift = user.Shift;
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today.AddHours(24);
            ViewData["WorkStationName"] = new SelectList(_context.WorkStations, "WorkStationName", "WorkStationName");

            IQueryable<Audit> query = _context.Audits
                .Where(x => x.CreatedAt > startDate && x.CreatedAt < endDate);

            if (User.IsInRole("auditor"))
            {
                query = query.Where(x => x.Line.Equals(Line) && x.Shift.Equals(Shift));
            }

            Audits = await query.ToListAsync();

        }
        public async Task OnGetAuditsAsync(string? line, string? shift)
        {
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today.AddHours(24);
            IQueryable<Audit> query = _context.Audits.Where(x => x.CreatedAt > startDate && x.CreatedAt < endDate);

            //Search by selected line
            if (line != null)
            {
                query = query.Where(x => x.Line.Equals(line));
            }

            //Search by selected shift
            if (shift != null)
            {
                query = query.Where(x => x.Shift.Equals(shift));
            }

            Audits = await query.ToListAsync();

        }
        public Audit Audit { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var data = new Audit();

            data.Shift = user.Shift;
            data.Auditor = user.FirstName+" "+user.LastName;
            data.Line = user.Line;
            data.CreatedAt = DateTime.Now;
            data.Result = "Open";
            
            _context.Audits.Add(data);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Edit",new { id = data.AuditID });
        }
    }
}
