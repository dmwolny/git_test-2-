using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.ComponentModel;
using Van_Authentication.Models;
using Van_Authentication.Services;

namespace Van_Authentication.Pages.VAN.Admin.Users
{
    [Authorize(Roles = "manager, coordinator")]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [DisplayName("First Name:")]
        public string? FirstName { get; set; }
        [DisplayName("Last Name:")]
        public string? LastName { get; set; }
        public string? Shift { get; set; }
        public string? Line { get; set; }
        public string? Id { get; set; }
        public UserInfo UserInfo { get; set; }
        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                FirstName = user.FirstName;
                LastName = user.LastName;
                Shift = user.Shift;
                Line = user.Line;
                Id = user.Id;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                ApplicationUser userToDelete = user;
                _context.Users.Remove(userToDelete);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }

}
