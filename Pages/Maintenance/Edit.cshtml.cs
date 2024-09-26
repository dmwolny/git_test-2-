using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Van_Authentication.Hubs;
using Van_Authentication.Models;
using Van_Authentication.Models.DTO;
using Van_Authentication.Models.Notes;
using Van_Authentication.Services;

namespace Van_Authentication.Pages.Maintenance
{
    public class EditModel : PageModel
    {
        private readonly IWebHostEnvironment _env;
        private readonly Van_Authentication.Services.ApplicationDbContext _context;
        private readonly ChatHub _hub;

        public EditModel(IWebHostEnvironment env, Van_Authentication.Services.ApplicationDbContext context, ChatHub hub)
        {
            _env = env;
            _context = context;
            _hub = hub;
        }

        public MaintenanceNote Notes { get; set; } = default!;
        [BindProperty]
        public MaintenanceDTO Maintenance { get; set; } = new MaintenanceDTO();

        public List<string> Images = new List<string>();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenance = await _context.maintenanceNotes.FirstOrDefaultAsync(m => m.Id == id);

            if (maintenance == null)
            {
                return NotFound();
            }

            Maintenance.Id = maintenance.Id;
            Maintenance.Shift = maintenance.Shift;
            Maintenance.Date = maintenance.Date;
            Maintenance.Safety = maintenance.Safety;
            Maintenance.Quality = maintenance.Quality;
            Maintenance.Delivery = maintenance.Delivery;
            Maintenance.Cost = maintenance.Cost;
            Maintenance.Morale = maintenance.Morale;

            if(maintenance.Graphic != null)
            {
                Images = maintenance.Graphic.Split(',').ToList();
            }


            Notes = maintenance;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Notes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaintenanceExists(Notes.Id))
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

        public async Task<JsonResult> OnPostSendCallAsync([FromBody] MaintenanceNote data)
        {
            var result = data;
            _context.Attach(result).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaintenanceExists(Maintenance.Id))
                {
                    return new JsonResult("not found");
                }
                else
                {
                    throw;
                }
            }

            return new JsonResult(result);

        }

        public async Task OnPostUploadAsync()
        {
            if (!ModelState.IsValid)
            {
                Page();
            }

            // Update the image file if we have a new image file
            string newFileName = string.Empty;
            if (Maintenance.ImageFile != null)
            {
                newFileName = Path.GetFileName(Maintenance.ImageFile.FileName);

                string imageFullPath = _env.WebRootPath + "/Images/Shift Notes/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    Maintenance.ImageFile.CopyTo(stream);
                }
            }
            var maintenance = await _context.maintenanceNotes.FirstOrDefaultAsync(m => m.Id == Maintenance.Id);
            //save the new robot in the database
            if (maintenance.Graphic == null)
            {
                maintenance.Graphic = newFileName;
            } else
            {
                maintenance.Graphic += "," + newFileName;
            }

            _context.maintenanceNotes.Update(maintenance);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaintenanceExists(Maintenance.Id))
                {
                    NotFound();
                }
                else
                {
                    throw;
                }
            }
            var group = "maint" + maintenance.Shift + maintenance.Date.ToString();
            _hub.ToAddImage(maintenance.Id, newFileName,group);
        }

        public async Task<JsonResult> OnPostDeleteAsync(int id, string fileName)
        {
            var maintenance = await _context.maintenanceNotes.FirstOrDefaultAsync(m => m.Id == id);
            if(maintenance.Graphic != null)
            {
                Images = maintenance.Graphic.Split(',').ToList();
                Images.Remove(fileName);
                string imagePath = _env.WebRootPath + "/Images/Shift Notes/" + fileName;
                System.IO.File.Delete(imagePath);
                maintenance.Graphic = string.Join(",", Images);
            }

            _context.maintenanceNotes.Update(maintenance);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaintenanceExists(Maintenance.Id))
                {
                    return new JsonResult("not found");
                }
                else
                {
                    throw;
                }
            }
            return new JsonResult("Success!");
        }

        private bool MaintenanceExists(int id)
        {
            return _context.maintenanceNotes.Any(e => e.Id == id);
        }
    }
}
