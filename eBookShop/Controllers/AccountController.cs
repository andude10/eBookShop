using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using eBookShop.ViewModels;
using eBookShop.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using eBookShop.Data;
using eBookShop.Repositories;

namespace eBookShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersRepository _usersRepository;
        public AccountController(IDbContextFactory<AppDbContext> contextFactory)
        {
            _usersRepository = new UsersRepository(contextFactory);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid) return View(model);
            
            var user = _usersRepository.FindUser(model.Email, model.Password);
            if (user != null)
            {
                Authenticate(model.Email);
 
                return RedirectToAction("About", "Home");
            }
            
            ModelState.AddModelError("", "Incorrect login or (and) password");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterModel model)
        {
            if (!ModelState.IsValid) return View(model);
            
            var user = _usersRepository.FindUser(model.Email, model.Password);
            if (user == null)
            {
                _usersRepository.Create(new User { Email = model.Email, Password = model.Password, Name = model.Name });

                Authenticate(model.Email);
 
                return RedirectToAction("About", "Home");
            }

            ModelState.AddModelError("", "Incorrect login or (and) password");
            return View(model);
        }
 
        private void Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };

            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
 
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}