using System.ComponentModel;

namespace Van_Authentication.Models
{
    public class PartModel
    {
        public int PartModelId { get; set; }
        [DisplayName("Model/Part Name")]
        public string PartModelName { get; set; } = "";
        [DisplayName("Type")]
        public string PartModelType { get; set; } = "";
        [DisplayName("Lot Control")]
        public int LotControl {  get; set; }
        [DisplayName("Workstation")]
        public int WorkStationId { get; set; }
        public WorkStation? WorkStation { get; set; }
        public ICollection<AuditRoute>? AuditRoutes { get; set; } 
    }
}
