namespace Van_Authentication.Models.DTO
{
    public class GetRbtInfo
    {
        public int RobotID { get; set; }
        public string Line { get; set; } = "";
        public int Station { get; set; }
        public int RobotNumber { get; set; }
        public string Style { get; set; } = "";
        public string Graphic { get; set; } = "default.PNG";
    }
}
