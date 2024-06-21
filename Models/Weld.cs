using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Van_Authentication.Models
{
    public class Weld
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DisplayName("Weld ID")]
        public int WeldID { get; set; }
        [Required]
        [StringLength(1)]
        [DisplayName("Weld Type")]
        public string WeldType { get; set; } = "";
        [DisplayName("Nugget Size")]
        public float NuggetSize { get; set; }

        public ICollection<RobotWeld> RobotWelds { get; set; }
        public ICollection<PartWeld> PartWelds { get; set; }

    }
}
