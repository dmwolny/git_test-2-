using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Van_Authentication.Models;
using Van_Authentication.Services;

namespace Van_Authentication.Pages.Certs
{
    public class EditModel : PageModel
    {
        private readonly Van_Authentication.Services.ApplicationDbContext _context;

        public EditModel(Van_Authentication.Services.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Cert Cert { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            ViewData["RouteLine"] = new SelectList(_context.Robots.Select(x => x.Line).Distinct());
            if (id == null)
            {
                return NotFound();
            }

            var cert =  await _context.Certs.FirstOrDefaultAsync(m => m.CertId == id);
            if (cert == null)
            {
                return NotFound();
            }
            Cert = cert;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if(Cert.Result.Count() > 0)
            {
                Cert.Status = "Closed";
            }
            _context.Attach(Cert).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CertExists(Cert.CertId))
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

        private bool CertExists(int id)
        {
            return _context.Certs.Any(e => e.CertId == id);
        }
    }
}
