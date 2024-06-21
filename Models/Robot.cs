using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Van_Authentication.Models
{
    public class Robot
    {
        [Key]
        public int RobotID { get; set; }
        [Required]
        [StringLength(50)]
        public string Line { get; set; } = "";
        public int Station { get; set; }
        [DisplayName("Robot #")]
        public int RobotNumber { get; set; }
        [Required]
        [StringLength(50)]
        public string Style { get; set; } = "";
        [StringLength(50)]
        public string? Graphic { get; set; }

        public ICollection<RobotWeld> RobotWelds { get; set; }
    }
}
