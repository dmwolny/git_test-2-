using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Van_Authentication.Models
{
    public class WeldConcern
    {
        [Key]
        [DisplayName("Weld Audit ID")]
        public int WeldConcernID { get; set; }
        [DisplayName("Audit ID")]
        public int AuditID { get; set; }
        [DisplayName("Weld ID")]
        public int WeldID { get; set; }
        [DisplayName("Weld Type")]
        public string WeldType { get; set; } = "";
        [DisplayName("Nugget Size")]
        public float NuggetSize { get; set; }
        public string? Line { get; set; } = "";
        public int Station { get; set; }
        [DisplayName("Robot #")]
        public int RobotNumber { get; set; }
        public string Style { get; set; } = "";
        public string Defect { get; set; } = "";
        [DisplayName("TCP #")]
        public int? TcpID { get; set; }
        public Audit? Audit { get; set; }
        public Tcp? Tcp { get; set; }
    }
}
