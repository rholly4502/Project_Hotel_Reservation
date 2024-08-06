using HotelReservationSystem.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelReservationSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly HotelReservationSystemContext _context;

        public AccountController(HotelReservationSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Validasi login dengan memeriksa database
            var account = _context.Accounts.SingleOrDefault(a => a.Username == username && a.Password == password);

            if (account != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, account.UserRole) // Tambahkan klaim UserRole
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "Home");
            }

            // Jika login gagal, kembalikan ke halaman login dengan pesan error
            ModelState.AddModelError("", "Username atau password salah");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string email, string password, string userRole)
        {
            var existingAccount = _context.Accounts.SingleOrDefault(a => a.Username == username);
            if (existingAccount != null)
            {
                return Json(new { success = false, message = "Username sudah ada. Silakan gunakan username lain." });
            }

            var newAccount = new Account
            {
                Username = username,
                Password = password,
                UserRole = userRole
            };

            _context.Accounts.Add(newAccount);
            await _context.SaveChangesAsync();

            var newCustomer = new Customer
            {
                Name = username,
                Email = email
            };

            _context.Customers.Add(newCustomer);
            await _context.SaveChangesAsync();

            var claims = new List<Claim>
    {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, userRole)
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return Json(new { success = true });
        }

    }
}
