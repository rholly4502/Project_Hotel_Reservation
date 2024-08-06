// MonthlyReservationReport.cs
namespace HotelReservationSystem.Models
{
    public class MonthlyReservationReport
    {
        public string YearMonth { get; set; }
        public int TotalReservations { get; set; }
        public int ConfirmedReservations { get; set; }
        public int CancelledReservations { get; set; }
    }
}
