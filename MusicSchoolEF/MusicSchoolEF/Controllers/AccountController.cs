using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicSchoolEF.Models.Db;
using MusicSchoolEF.Models.Defaults;
using MusicSchoolEF.Models.ViewModels;
using System.Security.Claims;
using static MusicSchoolEF.Helpers.HashPasswordHelper;

namespace MusicSchoolEF.Controllers
{
    public class AccountController : Controller
    {
        private readonly Ms2Context _dbContext;

        public AccountController(Ms2Context dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Login() => View(new LoginViewModel());

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Возвращаем форму входа с сообщениями об ошибках
                return View(model);
            }

            var user = await _dbContext.Users
                .SingleOrDefaultAsync(u => u.Login == model.Login);

            if (user == null)
            {
                // Добавляем сообщение об ошибке в модель состояния
                ModelState.AddModelError("Login", "Пользователь не был найден");
                // Возвращаем форму входа с сообщением об ошибке
                return View(model);
            }
            if (user.Password != GetHashPassword(model.Password))
            {
                // Добавляем сообщение об ошибке в модель состояния
                ModelState.AddModelError("Password", "Неверный пароль");
                // Возвращаем форму входа с сообщением об ошибке
                return View(model);
            }

            var result = Authenticate(user);
            // Сохранить id пользователя в куки
            Response.Cookies.Append("UserId", user.Id.ToString());

            // Выполняем аутентификацию пользователя
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(result));

            return user.Role switch
            {
                Roles.Admin => RedirectToAction("Index", "Admin", new { id = user.Id }),
                Roles.Teacher => RedirectToAction("Index", "Teacher", new { id = user.Id }),
                Roles.Student => RedirectToAction("Index", "Student", new { id = user.Id }),

                _ => RedirectToAction("Error", "Home"),
            };
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private static ClaimsIdentity Authenticate(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role),
            };
            return new ClaimsIdentity(claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }
    }
}
