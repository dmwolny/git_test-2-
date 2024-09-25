using System.ComponentModel.DataAnnotations;

namespace Van_Authentication.Models.Notes
{
    public class MaintenanceNote
    {
        [Required]
        public string String { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Date)]
        public DateOnly Date { get; set; }
        public string? Safety { get; set; }
        public string? Quality { get; set; }
        public string? Delivery { get; set; }
        public string? Cost { get; set; }
        public string? Morale { get; set; }
    }
}
