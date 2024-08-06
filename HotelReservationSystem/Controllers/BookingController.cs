using HotelReservationSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HotelReservationSystem.Controllers
{
    public class BookingController : Controller
    {
        private readonly HotelReservationSystemContext _context;

        public BookingController(HotelReservationSystemContext context)
        {
            _context = context;
        }
        // GET: BookingController
        public ActionResult Index()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var model = new LayoutViewModel
            {
                IsLoggedIn = User.Identity.IsAuthenticated,
                UserRole = userRole
            };
            var bookingDetails = (from r in _context.Reservations
                                      join c in _context.Customers on r.CustomerId equals c.CustomerId
                                      join ro in _context.Rooms on r.RoomId equals ro.RoomId
                                      orderby r.CheckInDate descending
                                      select new ReservationDetailsViewModel
                                      {
                                          ReservationID = r.ReservationId,
                                          CustomerID = r.CustomerId,
                                          CustomerName = c.Name,
                                          CustomerEmail = c.Email,
                                          RoomID = r.RoomId,
                                          RoomNumber = ro.RoomNumber,
                                          RoomType = ro.RoomType,
                                          CheckInDate = r.CheckInDate,
                                          CheckOutDate = r.CheckOutDate,
                                          Status = r.Status,
                                          AvailabilityStatus = ro.AvailabilityStatus,
                                          Price = ro.Price
                                      }).ToList();
            ViewBag.BookingDetails = bookingDetails;
            return View(model);
        }

        // POST: Booking/ConfirmBooking/{id}
        [HttpPost]
        public async Task<IActionResult> ConfirmBooking(int id)
        {
            try
            {
                var reservation = await _context.Reservations.FindAsync(id);
                if (reservation != null)
                {
                    reservation.Status = "Confirmed";
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Booking confirmed successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Booking not found.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to confirm booking: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Booking/CancelBooking/{id}
        [HttpPost]
        public async Task<IActionResult> CancelBooking(int id)
        {
            try
            {
                var reservation = await _context.Reservations.FindAsync(id);
                if (reservation != null)
                {
                    reservation.Status = "Cancelled";
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Booking cancelled successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Booking not found.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to cancel booking: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }


        // GET: BookingController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BookingController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BookingController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: BookingController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BookingController/Edit/5
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

        // GET: BookingController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BookingController/Delete/5
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
