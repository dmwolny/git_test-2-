using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Van_Authentication.Models;
using Van_Authentication.Services;

namespace Van_Authentication.Pages.VAN.Robots
{
    [Authorize(Roles = "manager, coordinator")]
    public class CreateModel : PageModel
    {
        private readonly IWebHostEnvironment env;
        private readonly ApplicationDbContext context;

        [BindProperty]
        public RobotDTO RobotDTO { get; set; } = new RobotDTO();

        public CreateModel(IWebHostEnvironment env, ApplicationDbContext context)
        {
            this.env = env;
            this.context = context;
        }

        public void OnGet()
        {
        }

        public string errorMessage = "";
        public string successMessage = "";
        public void OnPost()
        {
            if (!ModelState.IsValid)
            {
                errorMessage = "Please provide all the required fields";
                return;
            }

            string newFileName = "default.PNG";

            //save the image file
            if (RobotDTO.ImageFile != null)
            {
                newFileName = RobotDTO.Line + " Rbt " + RobotDTO.RobotNumber.ToString();
                newFileName += Path.GetExtension(RobotDTO.ImageFile.FileName);

                string imageFullPath = env.WebRootPath + "/Images/" + newFileName;

                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    RobotDTO.ImageFile.CopyTo(stream);
                }
            }

            //save the new robot in the database
            Robot robot = new Robot()
            {
                Line = RobotDTO.Line,
                Station = RobotDTO.Station,
                RobotNumber = RobotDTO.RobotNumber,
                Style = RobotDTO.Style,
                Graphic = newFileName
            };

            context.Robots.Add(robot);
            context.SaveChanges();

            //clear the form
            RobotDTO.Line = "";
            RobotDTO.RobotNumber = 0;
            RobotDTO.ImageFile = null;

            ModelState.Clear();

            successMessage = "Robot created successfully";
            Response.Redirect("/Robots/Index");
        }
    }
}