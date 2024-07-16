using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Van_Authentication.Models
{
    public class WorkStation
    {
        [Key]
        public int WorkStationId { get; set; }
        [DisplayName("Workstation")]
        public string WorkStationName { get; set; } = "";

        public ICollection<PartModel>? PartModels { get; set; }

    }
}
