using System.ComponentModel;

namespace Van_Authentication.Models
{
    public class RepairProcedure
    {
        public int RepairProcedureId { get; set; }
        [DisplayName("Repair Procedure Description")]
        public string RepairProcedureName { get; set; } = "";
    }
}
