using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicSchoolEF.Models.Db;
using MusicSchoolEF.Models.Defaults;
using MusicSchoolEF.Models.ViewModels;
using MusicSchoolEF.Repositories.Interfaces;
using System.Security.Claims;
using static MusicSchoolEF.Helpers.PasswordHelper;

namespace MusicSchoolEF.Controllers
{
    public class AccountController : Controller
    {
		private readonly IUserRepository _userRepository;

		public AccountController(IUserRepository userRepository)
		{
			_userRepository = userRepository;
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

			var user = await _userRepository.GetUserByLoginAsync(model.Login);

			if (user == null)
			{
				// Добавляем сообщение об ошибке в модель состояния
				ModelState.AddModelError("Login", "Пользователь не был найден");
				// Возвращаем форму входа с сообщением об ошибке
				return View(model);
			}
			if (!IsPasswordValid(model.Password, user.Password))
			{
				// Добавляем сообщение об ошибке в модель состояния
				ModelState.AddModelError("Password", "Неверный пароль");
				// Возвращаем форму входа с сообщением об ошибке
				return View(model);
			}

			var result = Authenticate(user);
			// Сохраняем `id` пользователя в куки
			Response.Cookies.Append("UserId", user.Id.ToString());

			// Выполняем аутентификацию пользователя
			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
				new ClaimsPrincipal(result));

			return RedirectToUserRole(user.Role, user.Id);
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

		private IActionResult RedirectToUserRole(string role, uint userId)
		{
			return role switch
			{
				Roles.Admin => RedirectToAction("Index", "Admin", new { id = userId }),
				Roles.Teacher => RedirectToAction("Index", "Teacher", new { id = userId }),
				Roles.Student => RedirectToAction("Index", "Student", new { id = userId }),
				_ => RedirectToAction("Error", "Home"),
			};
		}
	}
}
