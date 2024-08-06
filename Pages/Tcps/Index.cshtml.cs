using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Nancy.Extensions;
using Van_Authentication.Migrations;
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

        //public IList<Tcp> Tcp { get;set; } = default!;
        public IList<TcpFlatten> Tcp {  set; get; } = default!;

        //Pagination variables
        public int pageIndex = 1;
        public int totalPages = 0;
        private readonly int pageSize = 14;

        // Search variable
        public string search = "";
        [BindProperty(SupportsGet = true)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public string? startDate { get; set; }
        [BindProperty(SupportsGet = true)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd")]
        public string? endDate { get; set; }

        public async Task OnGetAsync(int? pageIndex, string? search, DateTime startDate, DateTime endDate)
        {
            if(startDate != DateTime.MinValue && endDate != DateTime.MinValue)
            {
                this.startDate = startDate.ToString("yyyy-MM-dd");
                this.endDate = endDate.ToString("yyyy-MM-dd");
            }


            IQueryable<TcpFlatten> query = _context.Tcps
                .GroupJoin(_context.WeldConcerns
                    .Join(_context.Audits,
                        weldConcern => weldConcern.AuditID,
                        audit => audit.AuditID,
                        (weldConcern, audit) => new { weldConcern, audit }
                        ),
                    tcp => tcp.TcpId,
                    wc => wc.weldConcern.TcpID,
                    (tcp, wcGroup) => new { tcp, wcGroup }
                    ).SelectMany(
                        x => x.wcGroup.DefaultIfEmpty(),
                        (x, wc) => new TcpFlatten
                        {
                            TcpId = x.tcp.TcpId,
                            CreatedAt = x.tcp.CreatedAt,
                            Shift = wc.audit != null ? wc.audit.Shift : null,
                            Status = x.tcp.Status,
                            Line = wc.weldConcern != null ? wc.weldConcern.Line : null,
                            Station = wc.weldConcern != null ? wc.weldConcern.Station : (int?)null,
                            RobotNumber = wc.weldConcern != null ? wc.weldConcern.RobotNumber : (int?)null,
                            Production = x.tcp.Production,
                            Maintenance = x.tcp.Maintenance,
                            Engineering = x.tcp.Engineering,
                            PurgeSheet = x.tcp.PurgeSheet
                        }).Distinct();
            //IQueryable<Tcp> query =  _context.Tcps.Include(p => p.WeldConcerns).ThenInclude(a => a.Audit);
            //IQueryable<TcpFlatten> query = _context.Tcps
            //    .Join(_context.WeldConcerns,
            //    tcp => tcp.TcpId,
            //    weldAudit => weldAudit.TcpID,
            //    (tcp, weldAudit) => new { tcp, weldAudit })
            //    .Join(_context.Audits,
            //    joined => joined.weldAudit.AuditID,
            //    audit => audit.AuditID,
            //    (joined, audit) => new TcpFlatten
            //    {
            //        TcpId = joined.tcp.TcpId,
            //        CreatedAt = joined.tcp.CreatedAt,
            //        Shift = audit.Shift,
            //        Status = joined.tcp.Status,
            //        Line = joined.weldAudit.Line,
            //        Station = joined.weldAudit.Station,
            //        RobotNumber = joined.weldAudit.RobotNumber,
            //        Production = joined.tcp.Production,
            //        Maintenance = joined.tcp.Maintenance,
            //        Engineering = joined.tcp.Engineering,
            //        PurgeSheet = joined.tcp.PurgeSheet
            //    })
            //    .Distinct();
            var start = startDate;
            var end = endDate;
            
            // search functionality
            //by daterange if that is the only thing selected.
            if(startDate != DateTime.MinValue && endDate != DateTime.MinValue) 
            {
                endDate = endDate.AddHours(24);
                query = query.Where(x => x.CreatedAt > startDate && x.CreatedAt < endDate);
            }

            // for searching by tcp#, status or by line.
            if(search != null)
            {
                this.search = search;
                int x = 0;

                //check to see if user did not select a date range
                //then set the date range to 1/1/2024 0:00:00 to DateTime.UTCNow
                if (startDate == DateTime.MinValue && endDate == DateTime.MinValue)
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
                    query = query.Where(x => x.Line.Contains(search) && x.CreatedAt > startDate && x.CreatedAt < endDate || x.Status.Contains(search) && x.CreatedAt > startDate && x.CreatedAt < endDate);
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

        }
    }

    public class TcpFlatten
    {
        public DateTime? CreatedAt;
        public string? Shift;
        public int TcpId;
        public string? Status;
        public string? Line;
        public int? Station;
        public int? RobotNumber;
        public string? Production;
        public string? Maintenance;
        public string? Engineering;
        public string? PurgeSheet;
    }
}
