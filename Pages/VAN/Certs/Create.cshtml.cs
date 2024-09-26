using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Van_Authentication.Models;
using Van_Authentication.Services;

namespace Van_Authentication.Pages.VAN.Certs
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["RouteLine"] = new SelectList(_context.Robots.Select(x => x.Line).Distinct());
            return Page();
        }

        [BindProperty]
        public Cert Cert { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Certs.Add(Cert);
            await _context.SaveChangesAsync();

            TempData["success"] = Cert.Type + " Cert #: " + Cert.CertId + " has been submitted.";
            TempData["subject"] = Cert.Type + " Cert #: " + Cert.CertId + " has been submitted";
            TempData["message"] = User.Identity.Name + " has submitted a "
                + "<a href= 'http://10.92.16.89:8055/Certs/Edit/" + Cert.CertId + "'>" + Cert.Type + "  Cert #: " + Cert.CertId + "</a>"
                + " from " + Cert.Line + "<br>Station: " + Cert.Station + "<br>Robot #: " + Cert.RobotNumber;

            return RedirectToPage("./Index");
        }
        public JsonResult OnGetStation(string Line, int id)
        {
            var lineRobots = new SelectList(_context.Robots.Where(m => m.Line == Line).Select(x => x.Station).Distinct(), "Station");
            return new JsonResult(lineRobots);
        }

        public JsonResult OnGetRobotNum(string Line, int Station, int id)
        {
            var lineRobots = new SelectList(_context.Robots.Where(m => m.Line == Line && m.Station == Station).Select(x => x.RobotNumber).Distinct(), "RobotNumber");
            return new JsonResult(lineRobots);
        }

        public JsonResult OnGetStyle(string Line, int Station, int RobotNumber, int id)
        {
            var lineRobots = new SelectList(_context.Robots.Where(m => m.Line == Line && m.Station == Station && m.RobotNumber == RobotNumber).Select(x => x.Style).Distinct(), "Style");
            return new JsonResult(lineRobots);
        }
    }
}
