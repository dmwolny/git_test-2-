using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Van_Authentication.Models;
using Van_Authentication.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Van_Authentication.Pages.Monitor
{
    public class WorkstationModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public WorkstationModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public IList<Audit> Audits { get; set; } = default!;
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public Audit Audit { get; set; } = default!;
        public string workstation { get; set; } = "";

        //Pagination variables
        public int pageIndex = 1;
        public int totalPages = 0;
        private readonly int pageSize = 14;

        public async Task OnGetAsync(int? pageIndex, string? workstation, DateTime startDate, DateTime endDate)
        {
            ViewData["WorkStationName"] = new SelectList(_context.WorkStations, "WorkStationName", "WorkStationName");
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
    }
}
