using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Van_Authentication.Services;

namespace Van_Authentication.Pages.VAN.Robots
{
    public class RobotName : PageModel
    {

        public SelectList? RobotNameSL { get; set; }

        public void PopulateRobotsDropDownList(ApplicationDbContext _context, object selectedRobot = null)
        {
            var robotsQuery = from r in _context.Robots
                              orderby r.Line
                              select r;

            var Lines = from r in _context.Robots
                        group r.Line by r.Line;

            var distinctLines = _context.Robots.Select(x => x.Line).Distinct().ToList();

            RobotNameSL = new SelectList(distinctLines, selectedRobot);
        }

    }
}
