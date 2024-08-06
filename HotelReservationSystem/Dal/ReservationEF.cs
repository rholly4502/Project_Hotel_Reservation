using HotelReservationSystem.Models;

namespace HotelReservationSystem.Dal
{
    public class ReservationEF : IReservation
    {
        private HotelReservationSystemContext _dbContext;

        public ReservationEF(HotelReservationSystemContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Reservation Add(Reservation entity)
        {
            try
            {
                _dbContext.Reservations.Add(entity);
                _dbContext.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Reservation> GetAll()
        {
            var results = _dbContext.Reservations.ToList();
            return results;
        }

        public Reservation GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Reservation> GetRoomByType()
        {
            throw new NotImplementedException();
        }

        public Reservation Update(Reservation entity)
        {
            throw new NotImplementedException();
        }
    }
}