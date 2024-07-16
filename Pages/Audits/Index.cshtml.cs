using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }

        public async Task OnGetAsync(DateTime? startDate, DateTime? endDate)
        {
            Audits = await _context.Audits.ToListAsync();
            if(startDate != null && endDate != null)
            {
                Audits = await _context.Audits.Where(x => x.CreatedAt > startDate && x.CreatedAt < endDate).ToListAsync();
                DateTime.Now.AddHours(-12);
            }
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
            data.Result = "OK";
            
            _context.Audits.Add(data);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Edit",new { id = data.AuditID });
        }
    }
}
