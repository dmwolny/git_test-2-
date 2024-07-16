using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Van_Authentication.Models
{
    public class Tcp
    {
        [Key]
        [DisplayName("TCP #")]
        public int TcpId { get; set; }
        [Required]
        [StringLength(6,MinimumLength = 2, ErrorMessage = "Status must be between ")]
        public string? Status { get; set; }
        [StringLength(100)]
        public string? Production { get; set; }
        [DisplayName("Number of units repaired")]
        public int Repaired { get; set; }
        [StringLength(12, ErrorMessage = "Length of Barcode is too long.")]
        [DisplayName("Barcode of 1st unit repaired")]
        public string? FirstRepaired { get; set; }
        [StringLength(12,ErrorMessage = "Length of barcode is too long.")]
        [DisplayName("Barcode of last unit repaired")]
        public string? LastRepaired { get; set; }
        [DisplayName("Repair Procedure")]
        public string? RepairProcedure { get; set; }
        [DisplayName("Notes")]
        public string? ProductionNotes { get; set; }
        [StringLength(100)]
        public string? Maintenance { get; set; }
        [DisplayName("Root Cause")]
        public string? RootCause { get; set; }
        [DisplayName("Corrective Action")]
        public string? CorrectiveAction { get; set; }
        [DisplayName("Notes")]
        public string? MaintenanceNotes { get; set; }
        public string? Engineering { get; set; }
        [DisplayName("Notes")]
        public string? EngineeringNotes { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DisplayName("TCP created at")]
        public DateTime? CreatedAt { get; set; }

        public ICollection<WeldConcern>? WeldConcerns { get; set; }
    }
}
