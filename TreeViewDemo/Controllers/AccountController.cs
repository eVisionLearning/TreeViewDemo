using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TreeViewDemo.Data;
using TreeViewDemo.Models;

namespace TreeViewDemo.Controllers
{
    public class AccountController(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AppUserLoginModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = context.AppUsers.FirstOrDefault(m => m.LoginId == model.LoginId);
            if (user == null)
            {
                ModelState.AddModelError("LoginId", "Login Id not exists.");
                return View(model);
            }

            if (CoreHandler.GetInstance().Decrypt(user.Password) != model.Password + user.Id)
            {
                ModelState.AddModelError("Password", "Invalid Password");
                return View(model);
            }
            else
            {
                var loginHistory = new AppUserLoginHistory()
                {
                    Token = Path.GetRandomFileName().Replace(".", ""),
                    UserId = user.Id,
                };

                context.Add(loginHistory);
                await context.SaveChangesAsync();
                Response.Cookies.Append(Globals.AuthenticationCookieName, loginHistory.Token, new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddDays(30),
                    HttpOnly = true,
                    MaxAge = new TimeSpan(30, 0, 0, 0)
                });

                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> Logout()
        {
            var existingToken = _httpContextAccessor?.HttpContext?.Request.Cookies[Globals.AuthenticationCookieName];
            if (!string.IsNullOrEmpty(existingToken))
            {
                var existingLogin = await context.AppUserLoginHistories.FirstOrDefaultAsync(m => m.Token == existingToken);
                if (existingLogin != null) context.Remove(existingLogin);
                await context.SaveChangesAsync();
            }
            Response.Cookies.Append(Globals.AuthenticationCookieName, "logout-cookie", new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(-30)
            });

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(AppUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            context.Add(model);
            await context.SaveChangesAsync();
            model.Password = CoreHandler.GetInstance().Encrypt(model.Password + model.Id);
            await context.SaveChangesAsync();

            return RedirectToAction("Login", "Account");
        }
    }
}