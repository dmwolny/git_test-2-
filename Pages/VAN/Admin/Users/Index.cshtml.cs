using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Van_Authentication.Pages.VAN.Admin.Users
{
    [Authorize(Roles = "manager, coordinator")]
    public class IndexModel : PageModel
    {
        private readonly Services.ApplicationDbContext _context;

        public IndexModel(Services.ApplicationDbContext context)
        {
            _context = context;
        }

        public List<UserInfo> listUsers = new List<UserInfo>();
        public void OnGet()
        {
            var users = _context.Users;
            foreach (var user in users)
            {
                UserInfo userInfo = new UserInfo();
                userInfo.id = user.Id;
                userInfo.firstname = user.FirstName;
                userInfo.lastname = user.LastName;
                userInfo.shift = user.Shift;
                userInfo.line = user.Line;
                listUsers.Add(userInfo);
            }

            foreach (var user in listUsers)
            {
                var roleID = _context.UserRoles.Where(x => x.UserId.Equals(user.id)).Select(y => y.RoleId).FirstOrDefault();
                if (roleID != null)
                {
                    user.role = _context.Roles.Where(x => x.Id.Equals(roleID)).Select(y => y.Name).FirstOrDefault();
                }
            }

            //If user requesting list is a weld coordinator, then remove all users with 
            //a role different from auditor.
            if (User.IsInRole("coordinator"))
            {
                listUsers.RemoveAll(s => s.role != "auditor");
            }
        }
    }

    public class UserInfo
    {
        public string id;
        public string firstname;
        public string lastname;
        public string role;
        public string shift;
        public string line;
    }
}
