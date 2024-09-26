using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Van_Authentication.Migrations;
using Van_Authentication.Models;
using Van_Authentication.Services;

namespace Van_Authentication.Pages.VAN.Tcps
{
    public class RemoveModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public RemoveModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<WeldConcern> TcpList { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Barcode can't be longer than 12 characters.")]
        [MinLength(1, ErrorMessage = "Barcode can't be longer than 12 characters.")]
        public string Notes { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tcps = _context.WeldConcerns.Where(x => x.AuditID == id && x.TcpID != null).Select(y => y.TcpID).Distinct().ToList();
            List<WeldConcern> tcps_ = new List<WeldConcern>();

            if (tcps == null)
            {
                return NotFound();
            }
            else
            {
                foreach (var tcp in tcps)
                {
                    var lines = _context.WeldConcerns.Where(x => x.AuditID == id && x.TcpID == tcp).FirstOrDefault();
                    if (lines != null)
                    {
                        tcps_.Add(lines);
                    }
                }
                TcpList = tcps_;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostRemove(int tcpId, string notes)
        {
            if (!ModelState.IsValid)
            {
                var id = _context.WeldConcerns.Include(z => z.Audit).Where(x => x.TcpID.Equals(tcpId)).Select(y => y.AuditID).FirstOrDefault();
                TempData["error"] = "Notes are required.";
                return RedirectToPage("./Remove", new { id });
            }
            //Get weldconcerns with TcpID
            var concerns = await _context.WeldConcerns.Include(z => z.Audit).Where(x => x.TcpID.Equals(tcpId)).ToListAsync();
            //Get TCP with TcpID
            var tcp = await _context.Tcps.Where(x => x.TcpId.Equals(tcpId)).FirstOrDefaultAsync();

            //Loop through weldconerns and set TcpID to null
            foreach (var item in concerns)
            {
                item.TcpID = null;
            }

            //Add notes to TCP and close TCP
            tcp.EngineeringNotes = "From Audit: " + concerns.Select(x => x.AuditID).FirstOrDefault()
                + "\r\nLine: " + concerns.Select(x => x.Line).FirstOrDefault()
                + "\r\nStation: " + concerns.Select(x => x.Station).FirstOrDefault()
                + "\r\nRobot#: " + concerns.Select(x => x.RobotNumber).FirstOrDefault()
                + "\r\nDefect: " + concerns.Select(x => x.Defect).FirstOrDefault()
                + "\r\nAuditor: " + concerns.Select(x => x.Audit.Auditor).FirstOrDefault()
                + "\r\nReason: " + notes;
            tcp.Status = "Closed";
            tcp.Engineering = User.Identity.Name.Replace('.', ' ');

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TcpExists(tcp.TcpId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/Audits/Edit", new { id = concerns.Select(x => x.AuditID).FirstOrDefault() });
        }


        private bool TcpExists(int id)
        {
            return _context.Tcps.Any(e => e.TcpId == id);
        }
    }
}
