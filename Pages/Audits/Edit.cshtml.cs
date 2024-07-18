using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Azure;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Nancy.Routing.Trie;
using Newtonsoft.Json;
using NuGet.Protocol;
using Van_Authentication.Models;
using Van_Authentication.Models.DTO;
using Van_Authentication.Pages.Robots;
using Van_Authentication.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Van_Authentication.Pages.Audits
{
    [Authorize(Roles = "manager, coordinator, auditor")]
    public class EditModel : RobotName
    {
        private readonly Van_Authentication.Services.ApplicationDbContext _context;

        public EditModel(Van_Authentication.Services.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Robot> Robot { get; set; } = default!;
        public IList<WeldConcern> WeldConcerns { get; set; } = default!;
        [BindProperty]
        public Audit Audit { get; set; } = default!;
        [BindProperty]
        public List<WeldDTO> WeldDTO { get; set; }
        [BindProperty]
        public bool TcpCheckbox { get; set; }
        [BindProperty]
        public string? Line { get; set; }
        [BindProperty]
        public int Station { get; set; }
        [BindProperty]
        public int RobotNumber { get; set; }
        [BindProperty]
        public string? Style { get; set; }
        public string? Type { get; set; }

        public IList<string> routeLines { get; set; } = default!;
        public ICollection<Robot> QueryRobots { get; set; } = default!;

        public string AlertMessage { get; set; } = "";

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            ViewData["WorkStationName"] = new SelectList(_context.WorkStations, "WorkStationName", "WorkStationName");
            ViewData["ModelName"] = new SelectList(_context.PartModels, "PartModelName", "PartModelName");
            ViewData["RouteName"] = new SelectList(_context.AuditRoutes, "AuditRouteName", "AuditRouteName");

            if (id == null)
            {
                return NotFound();
            }
            PopulateRobotsDropDownList(_context);

            var audit =  await _context.Audits.FirstOrDefaultAsync(m => m.AuditID == id);
            Type = await _context.PartModels.Where(m => m.PartModelName.Equals(audit.Model)).Select(n => n.PartModelType).FirstOrDefaultAsync();
            if(audit.Route != null)
            {
                var queryRoutes = _context.Robots
                    .Join(_context.RobotWelds,
                    robots => robots.RobotID,
                    robotWelds => robotWelds.RobotID,
                    (robots, robotWelds) => new { robots, robotWelds })
                    .Join(_context.AuditRouteWelds,
                    combinedRobots => combinedRobots.robotWelds.WeldID,
                    routeWelds => routeWelds.WeldId,
                    (combinedRobots, routeWelds) => new { combinedRobots, routeWelds }).
                    Join(_context.AuditRoutes,
                    combinedTables => combinedTables.routeWelds.AuditRouteId,
                    auditRoute => auditRoute.AuditRouteId,
                    (combinedTables, auditRoute) => new {combinedTables, auditRoute}).Where(m => m.auditRoute.AuditRouteName.Equals(audit.Route));
                routeLines = queryRoutes.Select(m => m.combinedTables.combinedRobots.robots.Line).Distinct().ToList();
                QueryRobots = queryRoutes.Select(m => m.combinedTables.combinedRobots.robots).ToList();
            }
            ViewData["RouteLine"] = new SelectList(routeLines);
            List<WeldConcern> discrepant = new List<WeldConcern>();
            discrepant = await _context.WeldConcerns.Where(d => d.AuditID == id).ToListAsync();

            if (audit == null)
            {
                return NotFound();
            }
            if(discrepant.Count == 0)
            {
                audit.Result = "OK";
            }
            else
            {
                audit.Result = "NOK";
            }
            Audit = audit;
            WeldConcerns = discrepant;
            return Page();
        }

        public async Task<IActionResult> OnGetWeldsAsync(string Line, int Station, int RobotNumber, string Style, int id)
        {
            List<WeldDTO> weldInfo = new List<WeldDTO>();

            var audit = _context.Audits.FirstOrDefault(m => m.AuditID == id);
            var rbtID = await _context.Robots.Where(r => r.Line.Equals(Line) && r.Station == Station && r.RobotNumber == RobotNumber && r.Style.Equals(Style))
                .Select(x => x.RobotID).FirstOrDefaultAsync();
            var rbtGraphic = await _context.Robots.Where(r => r.Line.Equals(Line) && r.Station == Station && r.RobotNumber == RobotNumber && r.Style.Equals(Style))
                .Select(x => x.Graphic).FirstOrDefaultAsync();
            var rtID = await _context.AuditRoutes.Where(x => x.AuditRouteName.Equals(audit.Route))
                .Select(y => y.AuditRouteId).FirstOrDefaultAsync();

            var anotherway = _context.RobotWelds
                .Join(_context.Welds,
                robotWeld => robotWeld.WeldID,
                weld => weld.WeldID,
                (robotWeld, weld) => new { robotWeld, weld })
                .Join(_context.AuditRouteWelds,
                welds => welds.weld.WeldID,
                auditwelds => auditwelds.WeldId,
                (welds, auditwelds) => new { welds, auditwelds })
                .Where(x => x.auditwelds.AuditRouteId == rtID && x.welds.robotWeld.RobotID == rbtID).Select(y => y.welds.weld).ToList();
            foreach(Weld rw in anotherway)
            {
                WeldDTO weld = new WeldDTO();
                weld.WeldID = rw.WeldID;
                weld.WeldType = rw.WeldType;
                weld.NuggetSize = rw.NuggetSize;
                if (rbtGraphic == null)
                {
                    weld.Graphic = "default.PNG";
                }
                else
                {
                    weld.Graphic = rbtGraphic;
                }
                weldInfo.Add(weld);
            }

            //var weldQuery = await _context.Robots.Where(r => r.Line.Equals(Line) && r.Station == Station && r.RobotNumber == RobotNumber && r.Style.Equals(Style)).Include(rw => rw.RobotWelds).ThenInclude(w => w.Weld).ToListAsync();
            
            //foreach (Robot r in weldQuery)
            //{
            //    if (r.RobotWelds.Count() == 0)
            //    {
            //        WeldDTO weld1 = new WeldDTO();
            //        weld1.WeldID = 0;
            //        weld1.WeldType = "not found";
            //        weld1.NuggetSize = 0;
            //        weld1.Graphic = "default.PNG";
            //        weldInfo.Add(weld1);
            //    }
            //    foreach (RobotWeld rw in r.RobotWelds)
            //    {
            //        WeldDTO weld = new WeldDTO();
            //        weld.WeldID = rw.Weld.WeldID;
            //        weld.WeldType = rw.Weld.WeldType;
            //        weld.NuggetSize = rw.Weld.NuggetSize;
            //        weld.Graphic = r.Graphic;
            //        weldInfo.Add(weld);
            //        if(r.Graphic == null)
            //        {
            //            weld.Graphic = "default.PNG";
            //        }
            //        else
            //        {
            //            weld.Graphic = r.Graphic;
            //        }
            //    }
            //}          

            //  Send list of welds back to JS function as a JSON
            var settings = new JsonSerializerSettings();
            settings.StringEscapeHandling = StringEscapeHandling.EscapeHtml;
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            string output = JsonConvert.SerializeObject(weldInfo, settings);
            return new JsonResult(output);
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int? id)
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

        public async Task<IActionResult> OnPostSetupAsync(int? id)
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

            return RedirectToPage("./Edit",id);
        }

        public async Task<IActionResult> OnPostAddConcernsAsync(int? id)
        {
            //Set tcpRecord to null
            Tcp tcpRecord = null;
            //Set tcp falg to false
            bool tcpFlag = false;

            //Get the Audit record
            if(id != null)
            {
                var audit = await _context.Audits.FirstOrDefaultAsync(m => m.AuditID == id);
                if(audit != null)
                {
                    Audit = audit;
                }
                
            }
            var htmlTable = "<table><thead><tr><th>Weld #</th><th>Weld Type</th><th>Nugget Size</th><th>Defect</th></thead><tbody>";
            //Iterate through the list of Weldconcerns and add those that have a defect to List<WeldConcern>
            List<WeldConcern> weldConcerns = new List<WeldConcern>();
            foreach (var item in WeldDTO)
            {
                htmlTable += "<tr>";

                //If the weldconcern has a defect then add it
                if(item.Graphic.Length > 0)
                {
                    WeldConcern weldConcern = new WeldConcern();
                    weldConcern.WeldID = item.WeldID;
                    weldConcern.WeldType = item.WeldType;
                    weldConcern.NuggetSize = item.NuggetSize;
                    weldConcern.Line = Line;
                    weldConcern.Station = Station;
                    weldConcern.RobotNumber = RobotNumber;
                    weldConcern.Style = Style;
                    weldConcern.Defect = item.Graphic;
                    weldConcern.Audit = Audit;
                    
                    //Set the tcp flag to true if tcp checkbox is checked
                    if(TcpCheckbox == true)
                    {
                        tcpFlag = true;
                    }
                    htmlTable += "<td>" + item.WeldID + "</td>"
                            + "<td>" + item.WeldType + "</td>"
                            + "<td>" + item.NuggetSize + "</td>"
                            + "<td>" + item.Graphic + "</td>";
                    weldConcerns.Add(weldConcern);
                }
                htmlTable += "</tr>";
            }

            htmlTable += "</tbody></table>";
            //Create a TCP if the TCP checkbox is checked
            //and if the TCP flag is set to true
            //(will be true if weld defect has been selected)
            if (TcpCheckbox == true && tcpFlag == true)
            {
                tcpRecord = new Tcp();
                tcpRecord.Status = "Open";
                tcpRecord.CreatedAt = DateTime.UtcNow;
                //tcpRecord.WeldConcerns = weldConcerns;
                _context.Tcps.Add(tcpRecord);
                await _context.SaveChangesAsync();
            }

            //iterate through the weld concerns that have defects identified
            foreach (var ent in weldConcerns)
            {
                TempData["success"] = "Weld concerns have successfully been added.";
                //if tcp exists, then add to each weldconcern
                if (tcpRecord != null)
                {
                    ent.Tcp = tcpRecord;
                    TempData["success"] = "TCP #: " + tcpRecord.TcpId + " has been submitted. Inform your supervisor.";
                    TempData["subject"] = "TCP #: " + tcpRecord.TcpId + " has been submitted";
                    TempData["message"] = User.Identity.Name + " has submitted TCP #: " + tcpRecord.TcpId
                        + "<a href= 'http://10.92.16.89:8055/Tcps/Edit/" + tcpRecord.TcpId + "'>Link to form</a>"
                        + " from " + ent.Line + "<br>Station: " + ent.Station + "<br>Robot #: " + ent.RobotNumber + htmlTable;

                }
                _context.WeldConcerns.Add(ent);
            }

            //Save changes to database
            _context.SaveChanges();

            //Redirect back to edit page including the audit ID
            return Redirect("/Audits/Edit/" + Audit.AuditID);
        }

        private bool AuditExists(int id)
        {
            return _context.Audits.Any(e => e.AuditID == id);
        }

        public JsonResult OnGetStation(string Line, int id)
        {
            var test = new SelectList(RouteData(id)
                .Where(m => m.Line == Line)
                .OrderBy(x => x.Station)
                .Select(y => y.Station)
                .Distinct(), "Station");
            var lineRobots = new SelectList(_context.Robots.Where(m => m.Line == Line).Select(x => x.Station).Distinct(),"Station");
            return new JsonResult(test);
        }

        public JsonResult OnGetRobotNum(string Line, int Station, int id)
        {
            var test = new SelectList(RouteData(id)
                .Where(m => m.Line == Line && m.Station == Station)
                .OrderBy(x => x.RobotNumber)
                .Select(y => y.RobotNumber)
                .Distinct(), "RobotNumber");
            var lineRobots = new SelectList(_context.Robots.Where(m => m.Line == Line && m.Station == Station).Select(x => x.RobotNumber).Distinct(), "RobotNumber");
            return new JsonResult(test);
        }

        public JsonResult OnGetStyle(string Line, int Station, int RobotNumber, int id)
        {
            var test = new SelectList(RouteData(id)
                .Where(m => m.Line == Line && m.Station == Station && m.RobotNumber == RobotNumber)
                .OrderBy(x => x.Style)
                .Select(y => y.Style)
                .Distinct(), "Style");
            var lineRobots = new SelectList(_context.Robots.Where(m => m.Line == Line && m.Station == Station && m.RobotNumber == RobotNumber).Select(x => x.Style).Distinct(), "Style");
            return new JsonResult(lineRobots);
        }

        public JsonResult OnGetModel(string line, string type)
        {
            var auditModels = new SelectList(_context.PartModels
                .Where(m => m.WorkStation.WorkStationName.Equals(line) && m.PartModelType.Equals(type))
                .Select(x => x.PartModelName)
                .Distinct(), "PartModelName");
            return new JsonResult(auditModels);
        }

        public JsonResult OnGetRoute(string line, string model)
        {
            var auditeRoutes = new SelectList(_context.AuditRoutes
                .Where(m => m.PartModel.WorkStation.WorkStationName.Equals(line) && m.PartModel.PartModelName.Equals(model))
                .Select(x => x.AuditRouteName)
                .Distinct(), "AuditRouteName");
            return new JsonResult(auditeRoutes);
        }

        public ICollection<Robot> RouteData(int? id)
        {
            var audit = _context.Audits.FirstOrDefault(m => m.AuditID == id);
            Type = _context.PartModels.Where(m => m.PartModelName.Equals(audit.Model)).Select(n => n.PartModelType).FirstOrDefault();
            if (audit.Route != null)
            {
                var queryRoutes = _context.Robots
                    .Join(_context.RobotWelds,
                    robots => robots.RobotID,
                    robotWelds => robotWelds.RobotID,
                    (robots, robotWelds) => new { robots, robotWelds })
                    .Join(_context.AuditRouteWelds,
                    combinedRobots => combinedRobots.robotWelds.WeldID,
                    routeWelds => routeWelds.WeldId,
                    (combinedRobots, routeWelds) => new { combinedRobots, routeWelds }).
                    Join(_context.AuditRoutes,
                    combinedTables => combinedTables.routeWelds.AuditRouteId,
                    auditRoute => auditRoute.AuditRouteId,
                    (combinedTables, auditRoute) => new { combinedTables, auditRoute })
                    .Where(m => m.auditRoute.AuditRouteName
                    .Equals(audit.Route));

                return queryRoutes.Select(m => m.combinedTables.combinedRobots.robots).ToList();
            }
            return null;
        }
    }
}
