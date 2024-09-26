using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Van_Authentication.Models;
using Van_Authentication.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Van_Authentication.Pages.VAN.Monitor
{
    public class WorkstationModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public WorkstationModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public IList<Audit> Audits { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string? startDate { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? endDate { get; set; }
        public Audit Audit { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string Workstation { get; set; } = "";

        //Pagination variables
        public int pageIndex = 1;
        public int totalPages = 0;
        private readonly int pageSize = 14;

        public async Task OnGetAsync(int? pageIndex, string? workstation, DateTime startDate, DateTime endDate)
        {
            if (startDate != DateTime.MinValue && endDate != DateTime.MinValue)
            {
                this.startDate = startDate.ToString("yyyy-MM-dd");
                this.endDate = endDate.ToString("yyyy-MM-dd");
            }

            ViewData["WorkStationName"] = new SelectList(_context.WorkStations, "WorkStationName", "WorkStationName");
            IQueryable<Audit> query = _context.Audits;

            if (workstation != null)
            {
                Workstation = workstation;
                query = query.Where(x => x.Line.Equals(workstation));
            }

            if (startDate != DateTime.MinValue && endDate != DateTime.MinValue)
            {
                endDate = endDate.AddHours(24);
                query = query.Where(x => x.CreatedAt > startDate && x.CreatedAt < endDate);
            }

            // Pagination function
            if (pageIndex == null || pageIndex < 1)
            {
                pageIndex = 1;
            }

            this.pageIndex = (int)pageIndex;
            decimal count = query.Count();
            totalPages = (int)Math.Ceiling(count / pageSize);
            query = query.Skip((this.pageIndex - 1) * pageSize)
                .Take(pageSize);
            Audits = await query.ToListAsync();
        }

        public FileResult OnPostExport(string? workstation, DateTime startDate, DateTime endDate)
        {
            List<Audit> audits = _context.Audits.ToList();
            IQueryable<Audit> query = _context.Audits;

            if (workstation != null)
            {
                query = query.Where(x => x.Line.Equals(workstation));
            }

            if (startDate != DateTime.MinValue && endDate != DateTime.MinValue)
            {
                endDate = endDate.AddHours(24);
                query = query.Where(x => x.CreatedAt > startDate && x.CreatedAt < endDate);
            }

            Audits = query.ToList();

            StringBuilder sb = new StringBuilder();
            sb.Append("Audit ID, Created At, Shift,Auditor,Line,Model,Route,Identifier,Result,Notes\r\n");
            for (int i = 0; i < Audits.Count; i++)
            {
                sb.Append(Audits[i].AuditID.ToString() + ',');
                sb.Append(Audits[i].CreatedAt.ToString() + ',');
                sb.Append(Audits[i].Shift + ',');
                sb.Append(Audits[i].Auditor + ',');
                sb.Append(Audits[i].Line + ',');
                sb.Append(Audits[i].Model + ',');
                sb.Append(Audits[i].Route + ',');
                sb.Append(Audits[i].Barcode + ',');
                sb.Append(Audits[i].Result + ",");
                if (Audits[i].Notes != null)
                {
                    sb.Append(Audits[i].Notes.Replace("\r\n", " ") + ",");
                }
                else sb.Append(Audits[i].Notes + ",");

                //Append new line character
                sb.Append("\r\n");
            }

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "Audits.csv");
        }
    }
}
