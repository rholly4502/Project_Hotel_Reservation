using HotelReservationSystem.Dal;
using HotelReservationSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelReservationSystem.Controllers
{
    public class RoomsController : Controller
    {
        private readonly IRoom _roomEf;
        public RoomsController(IRoom roomEf)
        {
            _roomEf = roomEf;
        }

        // GET: RoomsController
        public ActionResult Index()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var rooms = _roomEf.GetAll();
            var model = new LayoutViewModel
            {
                IsLoggedIn = User.Identity.IsAuthenticated,
                Rooms = rooms,
                UserRole = userRole
            };
            return View(model);
        }

        // GET: RoomsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // untuk menampilkan data berdasarkan modal
        public ActionResult GetRoom(int id)
        {
            var room = _roomEf.GetById(id);
            if (room == null)
            {
                return NotFound();
            }
            return Json(room);
        }

        // GET: RoomsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoomsController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(Room room)
        {
            try
            {
                var result = _roomEf.Add(room);

                TempData["SuccessMessage"] = $"Room : {room.RoomNumber} - {room.RoomType} - {room.Price} - {room.AvailabilityStatus} added successfully";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["ErrorMessage"] = "Room not added";
                return View();
            }
        }

        // GET: RoomsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RoomsController/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(Room room)
        {
            try
            {
                _roomEf.Update(room);
                TempData["SuccessMessage"] = "Update Data Room Berhasil!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Update Data Room Gagal.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: RoomsController/Delete/5
        public ActionResult Delete(int id)
        {
            var room = _roomEf.GetById(id);
            return View(room);
        }

        // POST: RoomsController/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeletePost(int id)
        {
            try
            {
                _roomEf.Delete(id);
                TempData["SuccessMessage"] = $"Room with id:{id} deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting Room: " + ex.Message;
                return View();
            }
        }
    }
}
