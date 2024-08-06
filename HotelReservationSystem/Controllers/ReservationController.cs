using HotelReservationSystem.Dal;
using HotelReservationSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HotelReservationSystem.Controllers
{

    public class ReservationController : Controller
    {
        private readonly IReservation _reservationEf;
        private readonly HotelReservationSystemContext _DbContext;
        public ReservationController(IReservation reservationEf, HotelReservationSystemContext context)
        {
            _reservationEf = reservationEf;
            _DbContext = context;
        }
        // GET: ReservationController
        public async Task<ActionResult> Index()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            var customer = await _DbContext.Customers.SingleOrDefaultAsync(c => c.Name == userName);

            var model = new LayoutViewModel
            {
                Rooms = _DbContext.Rooms.ToList(),
                IsLoggedIn = User.Identity.IsAuthenticated,
                UserRole = userRole, // This will be "Staff" or "Customer"
                CustomerId = customer.CustomerId,
            };
            return View(model);
        }

        // GET: ReservationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ReservationController/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddReservation(Reservation model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Simpan data reservasi ke database
                    _DbContext.Reservations.Add(model);
                    _DbContext.SaveChanges();

                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }

            return Json(new { success = false, message = "Invalid data" });
        }


        // POST: ReservationController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(Reservation reservation)
        {
            try
            {
                var result = _reservationEf.Add(reservation);

                TempData["SuccessMessage"] = $"Room : {reservation.ReservationId} added successfully";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["ErrorMessage"] = "Room not added";
                return View();
            }
        }

        // GET: ReservationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReservationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ReservationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReservationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
