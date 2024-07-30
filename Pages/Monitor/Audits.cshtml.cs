using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Van_Authentication.Models;
using Van_Authentication.Services;

namespace Van_Authentication.Pages.Monitor
{
    public class AuditsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public IList<PartModelDTO> Parts { get; set; }
        public IList<Audit> Audits { get; set; }
        public IList<AuditDTO> Audit { get; set; }
        public IList<RouteCount> RouteCount { get; set; }
        [BindProperty]
        public DateOnly? date {  get; set; }
        [BindProperty]
        public string Shift { get; set; }

        public AuditsModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public void OnGet(DateOnly? date, string? Shift)
        {
            List<PartModelDTO> partModels = new List<PartModelDTO>();
            var query = from partModel in _context.PartModels
                        join workstation in _context.WorkStations
                        on partModel.WorkStationId equals workstation.WorkStationId
                        group new { partModel, workstation } by new { partModel.PartModelType, workstation.WorkStationName } into grouped
                        select new PartModelDTO
                        {
                            WorkStationName = grouped.Key.WorkStationName,
                            PartModelType = grouped.Key.PartModelType,
                            NumberOfAudits = grouped.Sum(x => x.partModel.LotControl)
                        };
            partModels = query.ToList();
            Parts = partModels;

            List<Audit> audits = _context.Audits.Where(x => x.Shift.Equals(Shift) && (DateOnly.FromDateTime(x.CreatedAt)==date) ).ToList();
            List<AuditDTO> auditsDTO = new List<AuditDTO>();
            foreach (var item in audits)
            {
                AuditDTO audit = new AuditDTO();
                audit.Type = _context.PartModels
                    .Where(x => x.WorkStation.WorkStationName.Equals(item.Line)).Select(y => y.PartModelType)
                    .FirstOrDefault();
                audit.AuditId = item.AuditID;
                audit.CreatedAt = item.CreatedAt;
                audit.Shift = item.Shift;
                audit.Auditor = item.Auditor;
                audit.Line = item.Line;
                audit.Route = item.Route;
                audit.Barcode = item.Barcode;
                audit.Result = item.Result;
                auditsDTO.Add(audit);
            }
            Audit = auditsDTO;

            //Get list of workstations and their routes
            List<RouteCount> routeCount = new List<RouteCount>();
            var result = _context.WorkStations
                .Join(_context.PartModels,
                workstation => workstation.WorkStationId,
                partmodel => partmodel.WorkStationId,
                (workstation, partmodel) => new { workstation, partmodel })
                .Join(_context.AuditRoutes,
                combined => combined.partmodel.PartModelId,
                auditroute => auditroute.PartModelId,
                (combined, auditroute) => new { combined, auditroute })
                .Select(x => new
                {
                    Workstation = x.combined.workstation.WorkStationName,
                    AuditType = x.combined.partmodel.PartModelType,
                    RouteName = x.auditroute.AuditRouteName
                }).ToList();
            
            //sub each workstation/routename into routeCount obj
            foreach(var item in result)
            {
                RouteCount sub = new RouteCount();
                sub.Workstation = item.Workstation;
                sub.AuditType = item.AuditType;
                sub.RouteName = item.RouteName;
                routeCount.Add(sub);
            }

            RouteCount = routeCount;
        } 

        public JsonResult OnGetCompletedData(DateOnly? date, string? Shift)
        {
            var query = from partModel in _context.PartModels
                        join workstation in _context.WorkStations
                        on partModel.WorkStationId equals workstation.WorkStationId
                        group new { partModel, workstation } by new { partModel.PartModelType, workstation.WorkStationName } into grouped
                        select new PartModelDTO
                        {
                            WorkStationName = grouped.Key.WorkStationName,
                            PartModelType = grouped.Key.PartModelType,
                            NumberOfAudits = grouped.Sum(x => x.partModel.LotControl)
                        };
            List<Audit> audits = _context.Audits.Where(x => x.Shift.Equals(Shift) && (DateOnly.FromDateTime(x.CreatedAt) == date)).ToList();
            List<AuditDTO> auditsDTO = new List<AuditDTO>();
            foreach (var item in audits)
            {
                AuditDTO audit = new AuditDTO();
                audit.Type = _context.PartModels
                    .Where(x => x.WorkStation.WorkStationName.Equals(item.Line)).Select(y => y.PartModelType)
                    .FirstOrDefault();
                audit.AuditId = item.AuditID;
                audit.CreatedAt = item.CreatedAt;
                audit.Shift = item.Shift;
                audit.Auditor = item.Auditor;
                audit.Line = item.Line;
                audit.Route = item.Route;
                audit.Barcode = item.Barcode;
                audit.Result = item.Result;
                auditsDTO.Add(audit);
            }
            
            List<int> completed = new List<int>();
            List<int> required = new List<int>();
            List<int> completedSealer = new List<int>();
            List<int> requiredSealer = new List<int>();
            List<string> workstations = new List<string>();

            // Get number of completed audits for Welds
            foreach(var item in query.Where(x => x.PartModelType.Equals("Weld")))
            {
                workstations.Add(item.WorkStationName);
                completed.Add(auditsDTO.Where(x => x.Line.Equals(item.WorkStationName) && x.Type.Equals("Weld") && !x.Result.Equals("Open")).Select(y => y.Line).Count());
                required.Add(item.NumberOfAudits -
                    auditsDTO.Where(x => x.Line.Equals(item.WorkStationName) && x.Type.Equals("Weld") && !x.Result.Equals("Open")).Select(y => y.Line).Count());
            }

            // Get number of completed audits for Sealer
            foreach (var item in query.Where(x => x.PartModelType.Equals("Sealer")))
            {
                completedSealer.Add(auditsDTO.Where(x => x.Line.Equals(item.WorkStationName) && x.Type.Equals("Sealer") && !x.Result.Equals("Open")).Select(y => y.Line).Count());
                requiredSealer.Add(item.NumberOfAudits -
                    auditsDTO.Where(x => x.Line.Equals(item.WorkStationName) && x.Type.Equals("Sealer") && !x.Result.Equals("Open")).Select(y => y.Line).Count());
            }


            return new JsonResult( new {name = workstations, completed = completed,  required = required, completedSealer = completedSealer, requiredSealer = requiredSealer} );
        }

    }

    public class PartModelDTO 
    {
        public string WorkStationName { get; set; }
        public string PartModelType { get; set; }
        public int NumberOfAudits { get; set; }
    }

    public class AuditDTO
    {
        public DateTime CreatedAt { get; set; }
        public int AuditId { get; set; }
        public string Shift { get; set; } = "";
        public string Auditor { get; set; } = "";
        public string Line { get; set; } = "";
        public string Route { get; set; } = "";
        public string Barcode { get; set; } = "";
        public string Result { get; set; } = "";
        public string Model { get; set; } = "";
        public string Type { get; set; } = "";
    }

    public class RouteCount
    {
        public string Workstation { get; set; } = "";
        public string AuditType { get; set; } = "";
        public string RouteName { get; set; } = "";
    }

}
