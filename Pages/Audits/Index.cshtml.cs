using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Van_Authentication.Models;
using Van_Authentication.Services;

namespace Van_Authentication.Pages.Audits
{
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

        public async Task OnGetAsync()
        {
            Audits = await _context.Audits.ToListAsync();
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
            
            _context.Audits.Add(data);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Edit",new { id = data.AuditID });
        }
    }
}
