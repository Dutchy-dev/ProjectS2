using ClassLibrary.Domain.Models;
using ClassLibrary.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Generators;
using WebApplication.Presentation.Models;

namespace WebApplication.Presentation.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = _userService.GetByUsername(model.Username);
            
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError("", "Ongeldige inloggegevens");
                return View(model);
            }

            // Login gelukt → user ID opslaan in session
            HttpContext.Session.SetInt32("UserId", user.Id);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // Check of gebruikersnaam al bestaat
            if (_userService.UserExists(model.Username))
            {
                ModelState.AddModelError("Username", "Gebruikersnaam bestaat al");
                return View(model);
            }

            _userService.CreateUser(model.Username, model.Password);
            return RedirectToAction("Login");
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
