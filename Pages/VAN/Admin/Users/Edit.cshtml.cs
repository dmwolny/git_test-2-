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

namespace Van_Authentication.Pages.VAN.Admin.Users
{
    [Authorize(Roles = "manager, coordinator")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

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
        [Required]
        [BindProperty]
        public string? assignRole { get; set; }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            ViewData["WorkStationName"] = new SelectList(_context.WorkStations, "WorkStationName", "WorkStationName");
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.Where(u => u.Id.Equals(id)).FirstOrDefaultAsync();

            //Get user Role
            var roleID = _context.UserRoles.Where(x => x.UserId.Equals(id)).Select(y => y.RoleId).FirstOrDefault();
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

        public async Task<IActionResult> OnPostAsync(string? id)
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _context.Users.Where(u => u.Id.Equals(id)).FirstOrDefaultAsync();
            user.Line = Line;
            user.Shift = Shift;
            string userRole = "";

            //Check to see if there is a role assigned to the user
            //if true then get the role.
            if (_userManager.GetRolesAsync(user).Result.Count() > 0)
            {
                userRole = _userManager.GetRolesAsync(user).Result[0].ToString();
            }

            _context.Users.Update(user);

            try
            {
                await _context.SaveChangesAsync();

                //if a userRole was found then remove the role
                if (userRole.Count() > 0)
                {
                    await _userManager.RemoveFromRoleAsync(user, userRole);
                }

                //Assign the new role to the user
                await _userManager.AddToRoleAsync(user, assignRole);
                TempData["success"] = user.FirstName + " " + user.LastName + " was successfully updated.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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
