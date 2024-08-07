using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.ComponentModel.DataAnnotations;
using Van_Authentication.Models;
using Van_Authentication.Services;

namespace Van_Authentication.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Roles = "manager, coordinator")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditModel(Van_Authentication.Services.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [TempData]
        public string StatusMessage { get; set; }

        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        [BindProperty]
        public string Shift { get; set; } = "";
        [Required]
        [BindProperty]
        public string Line { get; set; } = "";
        public string? assignRole { get; set; }
      
        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["WorkStationName"] = new SelectList(_context.WorkStations, "WorkStationName", "WorkStationName"); 
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            
            //Get user Role
            var roleID = _context.UserRoles.Where(x => x.UserId.Equals(user.Id)).Select(y => y.RoleId).FirstOrDefault();
            if (roleID != null)
            {
                assignRole = _context.Roles.Where(x => x.Id.Equals(roleID)).Select(y => y.Name).FirstOrDefault();
            }

            if (user == null) 
            {
                return NotFound();
            }

            FirstName = user.FirstName;
            LastName = user.LastName;
            Shift = user.Shift;
            Line = user.Line;            

            return Page();

        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            user.Line = Line;
            user.Shift = Shift;

            _context.Users.Update(user);

            try
            {
                await _context.SaveChangesAsync();
                
                TempData["success"] = user.FirstName + " " + user.LastName + " was successfully updated.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Id))
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
        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id.Equals(id));
        }
    }
}
