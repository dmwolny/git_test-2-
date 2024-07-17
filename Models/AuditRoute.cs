using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Van_Authentication.Models
{
    public class AuditRoute
    {
        [Key]
        public int AuditRouteId { get; set; }
        [DisplayName("Route Name")]
        public string AuditRouteName { get; set; } = "";
        [DisplayName("Model - Part")]
        public int PartModelId { get; set; }

        public PartModel? PartModel { get; set; }
        public ICollection<AuditRouteWeld>? AuditRouteWelds { get; set; }
    }
}
