using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Van_Authentication.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(50, ErrorMessage = "First name can't be longer than 50 characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = "";
        [Required]
        [StringLength(50, ErrorMessage = "Last name can't be longer than 50 characters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = "";
        [Required]
        public string Shift { get; set; } = "";
        [Required]
        [StringLength(50, ErrorMessage = "Line can't be longer than 50 characters.")]
        public string Line { get; set; } = "";
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{g}")]
        public DateTime CreatedAt { get; set; }
    }
}
