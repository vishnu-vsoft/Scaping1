namespace Scaping1.Models
{
    public class AWBStatus
    {
        public string LastActivity { get; set; } = string.Empty;//
        public string LastActivityDate { get; set; } = string.Empty;//
        public string DONo { get; set; } = string.Empty;
        public string DownloadLink { get; set; } = string.Empty;
        public string AWB { get; set; } = string.Empty;//
        public string Pieces { get; set; } = string.Empty;//
        public string Status { get; set; } = string.Empty;
        public string Weight { get; set; } = string.Empty;
        public string Flights { get; set; } = string.Empty;
        public List<FlightDetail> FlightDetails { get; set; } = new List<FlightDetail>();
        public string ULDNo { get; set; } = string.Empty;
        public string HAWBNo { get; set; } = string.Empty;
        public string IssuedTo { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public string LastActivityDescription { get; set; } = string.Empty;
        public List<StatusHistory> StatusHistories { get; set; } = new List<StatusHistory>();
        public List<FlightSchedules> FlightSchedules { get; set; } = new List<FlightSchedules>();
    }
}
