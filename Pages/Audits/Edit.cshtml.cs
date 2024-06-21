using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;
using Van_Authentication.Models;
using Van_Authentication.Services;

namespace Van_Authentication.Pages.Audits
{
    public class EditModel : PageModel
    {
        private readonly Van_Authentication.Services.ApplicationDbContext _context;

        public EditModel(Van_Authentication.Services.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Audit Audit { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var audit =  await _context.Audits.FirstOrDefaultAsync(m => m.AuditID == id);
            if (audit == null)
            {
                return NotFound();
            }
            Audit = audit;
            Robot = await _context.Robots.ToListAsync();
            return Page();
        }

        public IList<Robot> Robot { get; set; } = default!;
        public List<Weld> welds { get; set; }

        public async Task<IActionResult> OnGetWeldsAsync()
        {
            List<Weld> weldData = new List<Weld>();
            var weldQuery = await _context.Robots.Where(r => r.Line.Contains("Center Floor") && r.Station == 1).Include(rw => rw.RobotWelds).ThenInclude(w => w.Weld).ToListAsync();
            foreach(Robot r in weldQuery)
            {
                foreach(RobotWeld rw in r.RobotWelds)
                {
                    weldData.Add(rw.Weld);
                }
            }

            List<Weld> newWeld = new List<Weld>
            {
                new Weld{WeldID = 1, WeldType = "O", NuggetSize = 4 },
                new Weld{WeldID = 2, WeldType = "S", NuggetSize = 5 },
                new Weld{WeldID = 3, WeldType = "A", NuggetSize = 6 },
            };
            

            var settings = new JsonSerializerSettings();
            settings.StringEscapeHandling = StringEscapeHandling.EscapeHtml;
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            string output = JsonConvert.SerializeObject(weldData, settings);
            return new JsonResult(output);
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Audit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuditExists(Audit.AuditID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool AuditExists(int id)
        {
            return _context.Audits.Any(e => e.AuditID == id);
        }
    }
}
