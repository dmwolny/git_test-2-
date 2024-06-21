using System.ComponentModel.DataAnnotations;

namespace Van_Authentication.Models
{
    public class Tcp
    {
        [Key]
        public int TcpId { get; set; }
        [Required]
        [StringLength(3,MinimumLength = 2, ErrorMessage = "Status must be between ")]
        public string? Status { get; set; }
        [StringLength(100)]
        public string? Production { get; set; }
        public int Repaired { get; set; }
        [StringLength(12)]
        public string? FirstRepaired { get; set; }
        public string? LastRepaired { get; set; }
        [StringLength(100)]
        public string? Maintenance { get; set; }
        public string? RootCause { get; set; }
        public string? CorrectiveAction { get; set; }
        public string? Engineering { get; set; }
    }
}
