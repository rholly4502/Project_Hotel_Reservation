namespace HotelReservationSystem.Models
{
    public class ReservationDetailsViewModel
    {
        public int ReservationID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public int RoomID { get; set; }
        public int RoomNumber { get; set; }
        public string RoomType { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string Status { get; set; }
        public string AvailabilityStatus { get; set; }
        public decimal Price { get; set; }
    }
}
