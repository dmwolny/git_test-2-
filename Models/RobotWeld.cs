namespace Van_Authentication.Models
{
    public class RobotWeld
    {
        public int RobotID { get; set; }
        public int WeldID { get; set; }
        public Robot Robot { get; set; } = default!;
        public Weld Weld { get; set; } = default!;
    }
}
