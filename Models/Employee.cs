namespace Van_Authentication.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string MasterNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Status { get; set; }
        public string? Job { get; set; }
        public string? Shift { get; set; }
        public int? Dept { get; set; }
        public int? Zone { get; set; }
        public int? Team { get; set; }
    }
}
