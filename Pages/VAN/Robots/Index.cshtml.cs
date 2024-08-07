using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Van_Authentication.Models;
using Van_Authentication.Pages.VAN.Robots;
using Van_Authentication.Services;

namespace Van_Authentication.Pages.Robots
{
    [Authorize]
    public class IndexModel : RobotName
    {
        private readonly Van_Authentication.Services.ApplicationDbContext _context;

        public IndexModel(Van_Authentication.Services.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Robot> Robot { get;set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string? Line { get; set; }

        public async Task OnGetAsync()
        {
            PopulateRobotsDropDownList(_context);

            var robots = from r in _context.Robots
                         select r;

            if (!string.IsNullOrEmpty(Line))
            {
                robots = robots.Where(r => r.Line.Contains(Line));
                Robot = await robots.ToListAsync();
            }
        }
    }
}
