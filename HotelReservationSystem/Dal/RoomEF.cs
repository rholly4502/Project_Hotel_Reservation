using HotelReservationSystem.Models;

namespace HotelReservationSystem.Dal
{
    public class RoomEF : IRoom
    {
        private HotelReservationSystemContext _dbContext;

        public RoomEF(HotelReservationSystemContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Room Add(Room entity)
        {
            try
            {
                _dbContext.Rooms.Add(entity);
                _dbContext.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var deleteRoom = GetById(id);
                if (deleteRoom != null)
                {
                    _dbContext.Rooms.Remove(deleteRoom);
                    _dbContext.SaveChanges();
                }
                else
                {
                    throw new ArgumentException("Room not found");
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public IEnumerable<Room> GetAll()
        {
            var results = _dbContext.Rooms.ToList();
            return results.ToList();
        }

        public Room GetById(int id)
        {
            var result = (from r in _dbContext.Rooms
                          where r.RoomId == id
                          select r).FirstOrDefault();
            return result;
        }

        public Room Update(Room entity)
        {
            try
            {
                var updateRoom = GetById(entity.RoomId);
                if (updateRoom != null)
                {
                    updateRoom.RoomNumber = entity.RoomNumber;
                    updateRoom.Price = entity.Price;
                    updateRoom.AvailabilityStatus = entity.AvailabilityStatus;
                    updateRoom.RoomType = entity.RoomType;
                    _dbContext.SaveChanges();
                }
                else
                {
                    throw new ArgumentException("Category not found");
                }
                return updateRoom;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
