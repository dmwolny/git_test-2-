using Newtonsoft.Json.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Van_Authentication.Models
{
    public class Part
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(15, ErrorMessage = "Part Number cannot be longer than 15 characters.")]
        [DisplayName("Part Number")]
        public string PartID { get; set; } = "";
        [StringLength(15, ErrorMessage = "Material can't be longer than 15 characters")]
        public string? Material { get; set; }
        [StringLength(50, ErrorMessage = "Part Description can't be longer than 50 characters")]
        [Display(Name = "Part Description")]
        public string? PartDesc { get; set; }
        public int? Thickness { get; set; }

        ICollection<PartWeld> PartWelds { get; set; }
    }
}
