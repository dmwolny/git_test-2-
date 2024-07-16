using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Van_Authentication.Models
{
    public class Audit
    {
        [Key]
        public int AuditID { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm}")]
        [DisplayName("Audit Date & Time")]
        public DateTime CreatedAt { get; set; }
        [Required]
        [StringLength(1)]
        public string Shift { get; set; } = "";
        [Required]
        [StringLength(100)]
        public string Auditor { get; set; } = "";
        [Required]
        [StringLength(50)]
        public string Line { get; set; } = "";
        [Required]
        [StringLength(50)]
        public string Route { get; set; } = "";
        [Required]
        [StringLength(12, ErrorMessage = "Barcode can't be longer than 12 characters.")]
        public string Barcode { get; set; } = "";
        [Required]
        [Length(2,3)]
        public string Result { get; set; } = "";
        public string? Notes { get; set; }
        public string? Model { get; set; } = "";

        public ICollection<WeldConcern>? WeldConcerns { get; set; }
    }
}
