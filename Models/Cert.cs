using System.ComponentModel;

namespace Van_Authentication.Models
{
    public class Cert
    {
        public int CertId { get; set; }
        public string Requestor { get; set; } = "";
        public string Type { get; set; } = "";
        public string Line { get; set; } = "";
        public int Station { get; set; }
        [DisplayName("Robot #")]
        public int RobotNumber { get; set; }
        public string Style { get; set; } = "";
        public string Failure { get; set; } = "";
        [DisplayName("Failure Date")]
        public DateTime FailureDate { get; set; }
        [DisplayName("Repair Date")]
        public DateTime RepairDate { get; set; }
        public string Trade { get; set; } = "";
        public string Cleanpoint { get; set; } = "";
        public string Auditor { get; set; } = "";
        public string InspectedUnit { get; set; } = "";
        public string Status { get; set; } = "";
        public string Result { get; set; } = "";

    }
}
