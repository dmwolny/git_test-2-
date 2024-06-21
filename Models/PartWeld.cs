namespace Van_Authentication.Models
{
    public class PartWeld
    {

        public string PartID { get; set; } = "";
        public int WeldID { get; set; }
        public Part Part { get; set; }
        public Weld Weld { get; set; }
    }
}
