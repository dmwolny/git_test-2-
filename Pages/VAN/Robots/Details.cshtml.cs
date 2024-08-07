using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Van_Authentication.Models;
using Van_Authentication.Models.ViewModels;
using Van_Authentication.Services;

namespace Van_Authentication.Pages.Robots
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly Van_Authentication.Services.ApplicationDbContext _context;

        public DetailsModel(Van_Authentication.Services.ApplicationDbContext context)
        {
            _context = context;
        }

        public Robot Robot { get; set; } = default!;
        public RobotWeldVM weldDetail { get; set; }

        public List<Weld> welds { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var robot = await _context.Robots.FirstOrDefaultAsync(m => m.RobotID == id);
            if (robot == null)
            {
                return NotFound();
            }
            else
            {
                Robot = robot;
                List<Weld> weldData = new List<Weld>();
                var weldID = 0;
                var robotWelds = _context.RobotWelds.Where(m => m.RobotID == id).Include(w => w.Weld).ToList();
                foreach(RobotWeld rw in robotWelds)
                {
                    weldData.Add(rw.Weld);
                }
                              
                welds = weldData;
               // weldDetail.Welds = null;
            }
            return Page();
        }
    }
}
