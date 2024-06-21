using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Van_Authentication.Pages
{
    [Authorize(Roles = "manager, coordinator")]
    public class AdminModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
