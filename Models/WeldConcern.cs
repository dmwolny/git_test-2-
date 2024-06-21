namespace Van_Authentication.Models
{
    public class WeldConcern
    {
        public int WelConcernID { get; set; }
        public int AuditID { get; set; }
        public int WeldID { get; set; }
        public string Line { get; set; } = "";
        public int Station { get; set; }
        public int RobotNumber { get; set; }
        public int DefectID { get; set; }
        public int? TcpID { get; set; }
        public Audit Audit { get; set; }
        public Tcp? Tcp { get; set; }
    }
}
