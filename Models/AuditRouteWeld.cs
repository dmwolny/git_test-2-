using System.Diagnostics;

namespace Van_Authentication.Models
{
    public class AuditRouteWeld
    {
        public int AuditRouteId { get; set; }
        public int WeldId { get; set; }

        public Weld Weld { get; set; } = default!;
        public AuditRoute AuditRoute { get; set; } = default!;
    }
}
