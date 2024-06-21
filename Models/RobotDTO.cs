using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Van_Authentication.Models
{
    public class RobotDTO
    {
        [Required]
        public int RobotId { get; set; }
        [Required]
        public string Line { get; set; } = "";
        [Required]
        public int Station { get; set; }
        [Required]
        public int RobotNumber { get; set; }
        [Required]
        public string Style { get; set; } = "";
        public IFormFile? ImageFile { get; set; }
    }
}
