using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Van_Authentication.Models;
using Van_Authentication.Pages.VAN.Admin.Users;
using Van_Authentication.Services;

namespace Van_Authentication.Pages.VAN.Monitor
{
    public class AuditorModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AuditorModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public IList<Audit> Audits { get; set; } = default!;
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public Audit Audit { get; set; } = default!;
        public string auditor { get; set; } = "";
        public List<UserInfo> listUsers = new List<UserInfo>();

        //Pagination variables
        public int pageIndex = 1;
        public int totalPages = 0;
        private readonly int pageSize = 14;

        public async Task OnGetAsync(int? pageIndex, string? auditor, DateTime startDate, DateTime endDate)
        {
            //Get all users
            var users = _context.Users;
            foreach (var user in users)
            {
                UserInfo userInfo = new UserInfo();
                userInfo.id = user.Id;
                userInfo.firstname = user.FirstName + " " + user.LastName;
                userInfo.lastname = user.LastName;
                userInfo.shift = user.Shift;
                userInfo.line = user.Line;
                listUsers.Add(userInfo);
            }

            //Grab users role
            foreach (var user in listUsers)
            {
                var roleID = _context.UserRoles.Where(x => x.UserId.Equals(user.id)).Select(y => y.RoleId).FirstOrDefault();
                if (roleID != null)
                {
                    user.role = _context.Roles.Where(x => x.Id.Equals(roleID)).Select(y => y.Name).FirstOrDefault();
                }
            }

            //keep users who are auditors
            listUsers.RemoveAll(s => s.role != "auditor");

            ViewData["AuditorName"] = new SelectList(listUsers.Select(x => x.firstname));

            //Get all audits
            IQueryable<Audit> query = _context.Audits;

            //Search by auditor name
            if (auditor != null)
            {
                query = query.Where(x => x.Auditor.Equals(auditor));
            }

            //search by date range
            if (startDate != DateTime.MinValue && endDate != DateTime.MinValue)
            {
                endDate = endDate.AddHours(24);
                query = query.Where(x => x.CreatedAt > startDate && x.CreatedAt < endDate);
            }

            // Pagination function
            if (pageIndex == null || pageIndex < 1)
            {
                pageIndex = 1;
            }

            this.pageIndex = (int)pageIndex;
            decimal count = query.Count();
            totalPages = (int)Math.Ceiling(count / pageSize);
            query = query.Skip((this.pageIndex - 1) * pageSize)
                .Take(pageSize);
            Audits = await query.ToListAsync();
        }
    }
}
