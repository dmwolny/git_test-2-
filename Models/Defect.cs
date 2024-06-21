using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Van_Authentication.Models
{
    public class Defect
    {
        [Key]
        public int DefectID { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Defect descripton can't be longer than 50 characters.")]
        [DisplayName("Defect Description")]
        public string DefectDesc { get; set; } = "";
        [Required]
        [StringLength(3, ErrorMessage = "Defect type can't be longer than 3 characters")]
        [DisplayName("Defect Type")]
        public string DefectType { get; set; } = "";
    }
}
