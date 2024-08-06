using HotelReservationSystem.Dal;
using HotelReservationSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelReservationSystem.Controllers
{
    public class CustomersController : Controller
    {
        private ICustomer _customerEF;

        public CustomersController(ICustomer customerEF)
        {
            _customerEF = customerEF;
        }
        // GET: CustomersController
        public ActionResult Index()
        {
            var customers = _customerEF.GetAll();
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var model = new LayoutViewModel
            {
                IsLoggedIn = User.Identity.IsAuthenticated,
                Customers = customers,
                UserRole = userRole
            };
            return View(model);
        }

        // untuk menampilkan data berdasarkan modal
        public ActionResult GetCustomer(int id)
        {
            var customer = _customerEF.GetById(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Json(customer);
        }

        // GET: CustomersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CustomersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomersController/Create
        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            try
            {
                var results = _customerEF.Add(customer);
                TempData["SuccessMessage"] = "Customer created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error creating customer: " + ex.Message;
                return View();
            }
        }

        // GET: CustomersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CustomersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer customer)
        {
            try
            {
                _customerEF.Update(customer);
                TempData["SuccessMessage"] = "Customer updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while updating the customer.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: CustomersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CustomersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                _customerEF.Delete(id);
                TempData["SuccessMessage"] = "Customer deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting customer: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
