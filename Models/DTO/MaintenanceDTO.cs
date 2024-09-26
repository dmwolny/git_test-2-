using System.ComponentModel.DataAnnotations;

namespace Van_Authentication.Models.DTO
{
    public class MaintenanceDTO
    {
        public int Id { get; set; }
        [Required]
        public string Shift { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Date)]
        public DateOnly Date { get; set; }
        public string? Safety { get; set; }
        public string? Quality { get; set; }
        public string? Delivery { get; set; }
        public string? Cost { get; set; }
        public string? Morale { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}