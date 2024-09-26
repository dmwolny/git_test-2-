using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Van_Authentication.Models;
using Van_Authentication.Services;

namespace Van_Authentication.Pages.VAN.Robots
{
    [Authorize(Roles = "manager, coordinator")]
    public class EditModel : PageModel
    {

        private readonly IWebHostEnvironment env;
        private readonly ApplicationDbContext context;

        public EditModel(IWebHostEnvironment env, ApplicationDbContext context)
        {
            this.env = env;
            this.context = context;
        }

        [BindProperty]
        public RobotDTO robotDTO { get; set; } = new RobotDTO();
        public Robot Robot { get; set; } = new Robot();

        public string errorMessage = "";
        public string successMessage = "";

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var robot = await context.Robots.FirstOrDefaultAsync(m => m.RobotID == id);
            if (robot == null)
            {
                return NotFound();
            }

            robotDTO.Line = robot.Line;
            robotDTO.Station = robot.Station;
            robotDTO.RobotNumber = robot.RobotNumber;
            robotDTO.Style = robot.Style;

            Robot = robot;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var robot = context.Robots.FirstOrDefault(r => r.RobotID == id);

            if (robot == null)
            {
                return NotFound();
            }

            // Update the image file if we have a new image file
            string newFileName = robot.Graphic;
            if (robotDTO.ImageFile != null)
            {
                newFileName = robot.Line + " Rbt " + robot.RobotNumber.ToString();
                newFileName += Path.GetExtension(robotDTO.ImageFile.FileName);

                string imageFullPath = env.WebRootPath + "/Images/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    robotDTO.ImageFile.CopyTo(stream);
                }
            }

            //save the new robot in the database
            robot.Line = robotDTO.Line;
            robot.Station = robotDTO.Station;
            robot.RobotNumber = robotDTO.RobotNumber;
            robot.Style = robotDTO.Style;
            robot.Graphic = newFileName;

            context.Robots.Update(robot);

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RobotExists(Robot.RobotID))
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

        private bool RobotExists(int id)
        {
            return context.Robots.Any(e => e.RobotID == id);
        }
    }
}
