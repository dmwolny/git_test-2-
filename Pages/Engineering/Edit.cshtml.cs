using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Van_Authentication.Hubs;
using Van_Authentication.Models.DTO;
using Van_Authentication.Models.Notes;
using Van_Authentication.Services;

namespace Van_Authentication.Pages.Engineering
{
    public class EditModel : PageModel
    {
        private readonly Van_Authentication.Services.ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ChatHub _hub;

        public EditModel(Van_Authentication.Services.ApplicationDbContext context, IWebHostEnvironment env, ChatHub hub)
        {
            _context = context;
            _env = env;
            _hub = hub;
        }

        public EngineeringNote Notes { get; set; } = default!;
        [BindProperty]
        public EngineeringDTO Engineering { get; set; } = new EngineeringDTO();
        public List<string> Images = new List<string>();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var engineeringnote =  await _context.EngineeringNotes.FirstOrDefaultAsync(m => m.Id == id);
            if (engineeringnote == null)
            {
                return NotFound();
            }

            Engineering.Id = engineeringnote.Id;
            Engineering.Shift = engineeringnote.Shift;
            Engineering.Date = engineeringnote.Date;
            Engineering.Safety = engineeringnote.Safety;
            Engineering.Quality = engineeringnote.Quality;
            Engineering.Delivery = engineeringnote.Delivery;
            Engineering.Cost = engineeringnote.Cost;
            Engineering.Morale = engineeringnote.Morale;
            if (engineeringnote.Graphic != null)
            {
                Images = engineeringnote.Graphic.Split(',').ToList();
            }
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

            _context.Attach(Engineering).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EngineeringNoteExists(Engineering.Id))
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

        public async Task<JsonResult> OnPostSendCallAsync([FromBody] EngineeringNote data)
        {
            var result = data;
            //get the images stored in the graphic field
            result.Graphic = _context.EngineeringNotes.Where(m => m.Id == data.Id).Select(x => x.Graphic).FirstOrDefault();
            _context.Attach(result).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EngineeringNoteExists(Engineering.Id))
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

        public async Task<IActionResult> OnPostUploadAsync()
        {
            if (Engineering.ImageFile == null)
            {
                return RedirectToPage("./Edit", Engineering.Id);
            }

            // Update the image file if we have a new image file
            string newFileName = string.Empty;
            if (Engineering.ImageFile != null)
            {
                newFileName = Path.GetFileName(Engineering.ImageFile.FileName);

                string imageFullPath = _env.WebRootPath + "/Images/Shift Notes/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    Engineering.ImageFile.CopyTo(stream);
                }
            }
            var engineering = await _context.EngineeringNotes.FirstOrDefaultAsync(m => m.Id == Engineering.Id);
            //save the new robot in the database
            if (engineering.Graphic == null)
            {
                engineering.Graphic = newFileName;
            }
            else
            {
                engineering.Graphic += "," + newFileName;
            }

            _context.EngineeringNotes.Update(engineering);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EngineeringNoteExists(Engineering.Id))
                {
                    NotFound();
                }
                else
                {
                    throw;
                }
            }
            var group = "eng" + engineering.Shift + engineering.Date.ToString("yyyy-MM-dd");
            await _hub.ToAddImage(engineering.Id, newFileName, group);
            return RedirectToPage("./Edit", Engineering.Id);
        }

        public async Task<JsonResult> OnPostDeleteAsync(int id, string fileName)
        {
            var engineering = await _context.EngineeringNotes.FirstOrDefaultAsync(m => m.Id == id);
            if (engineering.Graphic != null)
            {
                Images = engineering.Graphic.Split(',').ToList();
                Images.Remove(fileName);
                string imagePath = _env.WebRootPath + "/Images/Shift Notes/" + fileName;
                System.IO.File.Delete(imagePath);
                if (Images.Count > 0)
                {
                    engineering.Graphic = string.Join(",", Images);
                }
                else
                {
                    engineering.Graphic = null;
                }
            }

            _context.EngineeringNotes.Update(engineering);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EngineeringNoteExists(Engineering.Id))
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

        private bool EngineeringNoteExists(int id)
        {
            return _context.EngineeringNotes.Any(e => e.Id == id);
        }
    }
}
