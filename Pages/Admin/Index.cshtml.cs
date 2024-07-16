using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Van_Authentication.Pages.Admin
{
    [Authorize(Roles = "manager, coordinator")]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
