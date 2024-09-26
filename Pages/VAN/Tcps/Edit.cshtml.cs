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

namespace Van_Authentication.Pages.VAN.Tcps
{
    [Authorize(Roles = "manager, coordinator, supervisor")]
    public class EditModel : PageModel
    {
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditModel(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment env)
        {
            _env = env;
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Tcp Tcp { get; set; } = default!;
        [BindProperty]
        public bool TcpStatus { get; set; } = false;
        [BindProperty]
        public IFormFile? PAN { get; set; }
        public string? Graphic { get; set; }
        public string? Auditor { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewData["RepairProcedures"] = new SelectList(_context.RepairProcedure, "RepairProcedureName", "RepairProcedureName");

            var tcp = await _context.Tcps.Include(x => x.WeldConcerns).FirstOrDefaultAsync(m => m.TcpId == id);
            var line = tcp.WeldConcerns.Select(x => x.Line).FirstOrDefault();
            var station = tcp.WeldConcerns.Select(x => x.Station).FirstOrDefault();
            var robot = tcp.WeldConcerns.Select(x => x.RobotNumber).FirstOrDefault();
            var style = tcp.WeldConcerns.Select(x => x.Style).FirstOrDefault();
            var audit = tcp.WeldConcerns.Select(x => x.AuditID).FirstOrDefault();
            Auditor = await _context.Audits.Where(x => x.AuditID == audit).Select(m => m.Auditor).FirstOrDefaultAsync();
            if (tcp == null)
            {
                return NotFound();
            }
            if (tcp.Status.Equals("Closed"))
            {
                TcpStatus = true;
            }
            Tcp = tcp;
            Graphic = await _context.Robots.Where(m => m.Line.Equals(line) && m.Station == station && m.RobotNumber == robot && m.Style.Equals(style)).Select(m => m.Graphic).FirstOrDefaultAsync();
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


            // Update the image file if we have a new image file
            string newFileName = "";
            if (PAN != null)
            {
                newFileName = "TCP-" + Tcp.TcpId.ToString();
                newFileName += Path.GetExtension(PAN.FileName);

                string imageFullPath = _env.WebRootPath + "/Images/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    PAN.CopyTo(stream);
                }
            }

            Tcp.PurgeSheet = newFileName;
            _context.Attach(Tcp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TcpExists(Tcp.TcpId))
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

        public async Task<IActionResult> OnPostProductionAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Tcp.Production = user.FirstName + " " + user.LastName;
            _context.Attach(Tcp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                TempData["success"] = "Changes to Production saved.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TcpExists(Tcp.TcpId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Edit", new { id = Tcp.TcpId });
        }

        public async Task<IActionResult> OnPostMaintenanceAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Tcp.Maintenance = user.FirstName + " " + user.LastName;
            _context.Attach(Tcp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                TempData["success"] = "Changes to Maintenance saved.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TcpExists(Tcp.TcpId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Edit", new { id = Tcp.TcpId });
        }

        public async Task<IActionResult> OnPostEngineeringAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Tcp.Engineering = user.FirstName + " " + user.LastName;
            if (TcpStatus == true)
            {
                Tcp.Status = "Closed";
            }
            else
            {
                Tcp.Status = "Open";
            }
            _context.Attach(Tcp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                TempData["success"] = "Changes to Engineering saved.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TcpExists(Tcp.TcpId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Edit", new { id = Tcp.TcpId });
        }

        private bool TcpExists(int id)
        {
            return _context.Tcps.Any(e => e.TcpId == id);
        }
    }
}
