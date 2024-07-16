using System.ComponentModel;

namespace Van_Authentication.Models
{
    public class AuditRoute
    {

        public int AuditRouteId { get; set; }
        public string AuditRouteName { get; set; } = "";
        public int PartModelId { get; set; }

        public PartModel PartModel { get; set; } = default!;
        public ICollection<AuditRouteWeld> AuditRouteWelds { get; set; } = default!;
    }
}
