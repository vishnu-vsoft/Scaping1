namespace Scaping1.Models
{
    public class StatusHistory
    {
        public int OrderNumber { get; set; }
        public string Station { get; set; } = string.Empty;
        public string MileStone { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Pcs { get; set; } = string.Empty;
        public string ActualPieces { get; set; } = string.Empty;
        public string Weight { get; set; } = string.Empty;
        public string FlightNo { get; set; } = string.Empty;
        public string AirlineCode { get; set; } = string.Empty;
        public string FlightDate { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public string ULD { get; set; } = string.Empty;
        public string EventDateTime { get; set; } = string.Empty;
    }
}
