using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Van_Authentication.Models;
using Van_Authentication.Models.DTO;
using Van_Authentication.Services;

namespace Van_Authentication.Pages.VAN.Admin.Routes.Welds
{
    public class AddModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AddModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public List<WeldDTO> WeldDTO { get; set; }

        [BindProperty]
        public string? Line { get; set; }
        [BindProperty]
        public int Station { get; set; }
        [BindProperty]
        public int RobotNumber { get; set; }
        [BindProperty]
        public string? Style { get; set; }
        //[BindProperty]
        //public AuditRouteWeld AuditRouteWeld { get; set; } = default!;
        public List<AuditRouteWeld> Welds { get; set; }
        [BindProperty]
        public int id { get; set; }

        public IActionResult OnGet(int? routeID)
        {
            ViewData["AuditRouteId"] = new SelectList(_context.AuditRoutes, "AuditRouteId", "AuditRouteName");
            ViewData["WeldId"] = new SelectList(_context.Welds, "WeldID", "WeldID");
            ViewData["RouteLine"] = new SelectList(_context.Robots.Select(x => x.Line).Distinct());

            if (routeID != null)
            {
                id = (int)routeID;

            }
            return Page();
        }
        public async Task<IActionResult> OnGetWeldsAsync(string Line, int Station, int RobotNumber, string Style)
        {
            List<WeldDTO> weldInfo = new List<WeldDTO>();

            var rbtID = await _context.Robots.Where(r => r.Line.Equals(Line) && r.Station == Station && r.RobotNumber == RobotNumber && r.Style.Equals(Style))
                .Select(x => x.RobotID).FirstOrDefaultAsync();
            var rbtGraphic = await _context.Robots.Where(r => r.Line.Equals(Line) && r.Station == Station && r.RobotNumber == RobotNumber && r.Style.Equals(Style))
                .Select(x => x.Graphic).FirstOrDefaultAsync();

            var anotherway = _context.RobotWelds
                .Join(_context.Welds,
                robotWeld => robotWeld.WeldID,
                weld => weld.WeldID,
                (robotWeld, weld) => new { robotWeld, weld })
                .Where(x => x.robotWeld.RobotID == rbtID).Select(y => y.weld).ToList();
            foreach (Weld rw in anotherway)
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

            //  Send list of welds back to JS function as a JSON
            var settings = new JsonSerializerSettings();
            settings.StringEscapeHandling = StringEscapeHandling.EscapeHtml;
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            string output = JsonConvert.SerializeObject(weldInfo, settings);
            return new JsonResult(output);
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            foreach (var weld in WeldDTO)
            {
                if (weld.Graphic != null)
                {
                    AuditRouteWeld temp = new AuditRouteWeld();
                    temp.AuditRouteId = id;
                    temp.WeldId = weld.WeldID;
                    _context.AuditRouteWelds.Add(temp);
                }

            }

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return Page();
            }


            return RedirectToPage("../Details", new { id });

            //AuditRouteWeld.AuditRouteId = id;
            //_context.AuditRouteWelds.Add(AuditRouteWeld);
            //await _context.SaveChangesAsync();

            //return RedirectToPage("../Details", new { id = AuditRouteWeld.AuditRouteId});
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
