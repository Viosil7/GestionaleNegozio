using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GestionaleNegozio.Models;

namespace GestionaleNegozio.Controllers
{
    public class StaffController : BaseController
    {
        private readonly StaffDao _staffDao;

        public StaffController(IConfiguration configuration) : base(configuration)
        {
            _staffDao = new StaffDao(_connectionString);
        }

        [Authorize]
        public ActionResult Index()
        {
            var staff = _staffDao.GetAll();
            return View(staff);
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            var staff = _staffDao.GetById(id);
            if (staff == null) return NotFound();
            return View(staff);
        }

        [Authorize(Roles = "Manager")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public IActionResult Create(Staff staff)
        {
            if (ModelState.IsValid)
            {
                if (!IsValidPassword(staff.Passw))
                {
                    ModelState.AddModelError("Passw", "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.");
                    return View(staff);
                }

                if (_staffDao.GetByUsername(staff.Username) != null)
                {
                    ModelState.AddModelError("Username", "Username already exists");
                    return View(staff);
                }

                _staffDao.Insert(staff);
                return RedirectToAction(nameof(Index));
            }
            return View(staff);
        }

        private bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8)
                return false;

            bool hasUpper = false;
            bool hasLower = false;
            bool hasNumber = false;
            bool hasSpecial = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c)) hasUpper = true;
                else if (char.IsLower(c)) hasLower = true;
                else if (char.IsDigit(c)) hasNumber = true;
                else if (!char.IsLetterOrDigit(c)) hasSpecial = true;
            }

            return hasUpper && hasLower && hasNumber && hasSpecial;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var staff = _staffDao.Authenticate(model.Username, model.Password);
                if (staff != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, staff.Username),
                        new Claim(ClaimTypes.Role, staff.Ruolo),
                        new Claim(ClaimTypes.NameIdentifier, staff.Id.ToString())
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe,
                        ExpiresUtc = model.RememberMe ?
                            DateTimeOffset.UtcNow.AddDays(30) :
                            DateTimeOffset.UtcNow.AddHours(2)
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        principal,
                        authProperties
                    );

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid username or password");
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Manager")]
        public ActionResult Edit(int id)
        {
            var staff = _staffDao.GetById(id);
            if (staff == null) return NotFound();
            return View(staff);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public ActionResult Edit(Staff staff)
        {
            if (ModelState.IsValid)
            {
                var existingStaff = _staffDao.GetById(staff.Id);

                if (!IsValidPassword(staff.Passw))
                {
                    ModelState.AddModelError("Passw", "Password must be at least 8 characters and contain: uppercase letter, lowercase letter, number, and special character.");
                    return View(staff);
                }

                if (existingStaff.Ruolo == "Manager" && staff.Ruolo != "Manager" && _staffDao.IsLastManager(staff.Id))
                {
                    ModelState.AddModelError("Ruolo", "Cannot change role of the last manager.");
                    return View(staff);
                }

                var userWithSameUsername = _staffDao.GetByUsername(staff.Username);
                if (userWithSameUsername != null && userWithSameUsername.Id != staff.Id)
                {
                    ModelState.AddModelError("Username", "Username already exists");
                    return View(staff);
                }

                _staffDao.Update(staff);
                return RedirectToAction(nameof(Index));
            }
            return View(staff);
        }

        [Authorize(Roles = "Manager")]
        public ActionResult Delete(int id)
        {
            var staff = _staffDao.GetById(id);
            if (staff == null) return NotFound();
            return View(staff);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Manager")]
        public ActionResult DeleteConfirmed(int id)
        {
            var staff = _staffDao.GetById(id);
            if (staff.Ruolo == "Manager" && _staffDao.IsLastManager(id))
            {
                TempData["Error"] = "Cannot delete the last manager account.";
                return RedirectToAction(nameof(Index));
            }

            _staffDao.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
