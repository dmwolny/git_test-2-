using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Nancy.Extensions;
using Van_Authentication.Models;
using Van_Authentication.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Van_Authentication.Pages.Tcps
{
    [Authorize(Roles = "manager, coordinator, supervisor")]
    public class IndexModel : PageModel
    {
        private readonly Van_Authentication.Services.ApplicationDbContext _context;

        public IndexModel(Van_Authentication.Services.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Tcp> Tcp { get;set; } = default!;

        //Pagination variables
        public int pageIndex = 1;
        public int totalPages = 0;
        private readonly int pageSize = 14;

        // Search variable
        public string search = "";
        [BindProperty]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? startDate { get; set; } = default!;
        [BindProperty]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy")]
        public DateTime? endDate { get; set; } = default!;

        public async Task OnGetAsync(int? pageIndex, string? search, DateTime? startDate, DateTime? endDate)
        {
            IQueryable<Tcp> query =  _context.Tcps.Include(p => p.WeldConcerns).ThenInclude(a => a.Audit);
            var start = startDate;
            var end = endDate;

            // search functionality
            //by daterange if that is the only thing selected.
            if(startDate != null && endDate != null) 
            {
                query = query.Where(x => x.CreatedAt > startDate && x.CreatedAt < endDate);
            }

            // for searching by tcp#, status or by line.
            if(search != null)
            {
                this.search = search;
                int x = 0;

                //check to see if user did not select a date range
                //then set the date range to 1/1/2024 0:00:00 to DateTime.UTCNow
                if (startDate == null && endDate == null)
                {
                    startDate = DateTime.MinValue;
                    endDate = DateTime.UtcNow;
                }

                //if search term is a number then search by TCP #
                var id = Int32.TryParse(search, out x);
                if (id){
                    query = query.Where(id => id.TcpId == x && id.CreatedAt > startDate && id.CreatedAt < endDate);
                }
                else
                {
                    query = (from tcp in _context.Tcps
                             join wc in _context.WeldConcerns on tcp.TcpId equals wc.TcpID
                             where (wc.Line.Contains(search) && tcp.CreatedAt > startDate && tcp.CreatedAt < endDate || tcp.Status.Contains(search) && tcp.CreatedAt > startDate && tcp.CreatedAt < endDate)
                             select new Tcp
                             {
                                 TcpId = tcp.TcpId,
                                 Status = tcp.Status,
                                 Production = tcp.Production,
                                 Repaired = tcp.Repaired,
                                 FirstRepaired = tcp.FirstRepaired,
                                 LastRepaired = tcp.LastRepaired,
                                 RepairProcedure = tcp.RepairProcedure,
                                 ProductionNotes = tcp.ProductionNotes,
                                 Maintenance = tcp.Maintenance,
                                 RootCause = tcp.RootCause,
                                 CorrectiveAction = tcp.CorrectiveAction,
                                 MaintenanceNotes = tcp.MaintenanceNotes,
                                 Engineering = tcp.Engineering,
                                 EngineeringNotes = tcp.EngineeringNotes,
                                 WeldConcerns = tcp.WeldConcerns,
                                 CreatedAt = tcp.CreatedAt
                             }) ;
                }
            }

            query = query.OrderByDescending(p => p.TcpId);

            // Pagination function
            if (pageIndex == null || pageIndex < 1)
            {
                pageIndex = 1;
            }

            this.pageIndex = (int) pageIndex;
            decimal count = query.Count();
            totalPages = (int)Math.Ceiling(count / pageSize);
            query = query.Skip((this.pageIndex - 1) * pageSize)
                .Take(pageSize);
            Tcp = await query.ToListAsync();

            startDate = null;
            endDate = null;
        }
    }
}
