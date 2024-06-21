namespace Van_Authentication.Models.ViewModels
{
    public class RobotWeldVM
    {
        public Robot Robots { get; set; }
        public IEnumerable<Weld> Welds { get; set; }
        public IEnumerable<Part> Parts { get; set; }
    }
}
