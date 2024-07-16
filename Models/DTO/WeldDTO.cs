using System.ComponentModel.DataAnnotations;

namespace Van_Authentication.Models.DTO
{
    public class WeldDTO
    {
        public int WeldID { get; set; }
        public string WeldType { get; set; } = "";
        public float NuggetSize { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "Please select a defect")]
        public string Graphic { get; set; } = "";

    }
}
