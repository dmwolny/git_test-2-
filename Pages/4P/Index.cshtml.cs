using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Van_Authentication.Models.FourP;
using Van_Authentication.Services;

namespace Van_Authentication.Pages._4P
{
    public class IndexModel : PageModel
    {
        private ApplicationDbContext _context;

        public IList<Certifiy> FourPs { get; set; } = default;
        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
            FourPs = _context.Certifications.ToList();

        }
    }
}
