namespace Van_Authentication.Models.FourP
{
    public class Certifiy
    {
        public int Id { get; set; }
        public string Shift { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string MasterNumber { get; set; } = string.Empty;
        public int Dept { get; set; }
        public int Zone { get; set; }
        public string StationNo { get; set; } = string.Empty;
        public string StationName { get; set; } = string.Empty;
        public string? FourPFile { get; set; } = string.Empty;
    }
}
