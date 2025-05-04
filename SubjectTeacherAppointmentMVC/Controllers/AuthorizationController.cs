using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SubjectTeacherAppointmentMVC.Models.DataBase;
using SubjectTeacherAppointmentMVC.Models;
using SubjectTeacherAppointmentMVC.Utils;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using SubjectTeacherAppointmentMVC.Models.DataBase.Tables;
using Microsoft.EntityFrameworkCore;

namespace SubjectTeacherAppointmentMVC.Controllers {

	public class AuthorizationController : Controller {
		private readonly ILogger<AuthorizationController> _logger;
		private ApplicationContext db;


		public AuthorizationController(ILogger<AuthorizationController> logger, ApplicationContext context) {
			_logger = logger;
			db = context;
		}

		#region UtilityMethods

		#endregion

		/// <summary>
		/// Страница аутентификации.
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> Login() {
			if (HttpContext.User.Identity?.IsAuthenticated == true) {
				return RedirectToAction("Index", "Home");
			}

			return View();
		}

		/// <summary>
		/// Аутентификация пользователя.
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> Login(User user) {
			if (HttpContext.User.Identity?.IsAuthenticated == true) {
				return RedirectToAction("Index", "Home");
			}

			user.Login = user.Login?.Trim() ?? "";
			var userDB = db.Users.FirstOrDefault(u => u.Login.ToLower() == user.Login.ToLower());
			if (userDB == null) {
				user.Login = null;
				user.Password = null;
				return View(user);
			}
			if (!Enumerable.SequenceEqual(userDB.PasswordHashed, UserInfo.EncodePassword(user.Password, userDB.UserID))) {
				user.Password = null;
				return View(user);
			}

			var rememberMe = user.IsAdmin;
			var claims = new List<Claim> {
				new Claim(ClaimTypes.NameIdentifier, ((Guid)userDB.UserID) + ""),
				new Claim(ClaimTypes.Name, userDB.Login),
				new Claim(ClaimTypes.Role, userDB.IsAdmin ? "Admin" : "User")
			};
			ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

			await HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				new ClaimsPrincipal(claimsIdentity),
				new AuthenticationProperties() { IsPersistent = rememberMe, ExpiresUtc = DateTime.Now.AddYears(1) });

			return RedirectToAction("Index", "Home");
		}

		/// <summary>
		/// Страница регистрации.
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> Register() {
			if (HttpContext.User.Identity?.IsAuthenticated == true) {
				return RedirectToAction("Index", "Home");
			}

			return View();
		}

		/// <summary>
		/// Регистрация пользователя.
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> Register(User user) {
			if (HttpContext.User.Identity?.IsAuthenticated == true) {
				return RedirectToAction("Index", "Home");
			}

			user.Login = user.Login.Trim();
			var userDB = db.Users.FirstOrDefault(u => u.Login.ToLower() == user.Login.ToLower());
			if (userDB != null) {
				user.Login = null;
				return View(user);
			}

			user.PasswordHashed = new byte[32];

			if (user.Login.ToLower() == "admin") {
				user.Login = user.Login.ToLower();
				user.IsAdmin = true;
			} else {
				user.IsAdmin = false;
			}

			var addedUserDB = db.Users.Add(user);
			addedUserDB.Entity.PasswordHashed = UserInfo.EncodePassword(user.Password, addedUserDB.Entity.UserID);

			db.SaveChanges();
			return RedirectToAction("Login");
		}

		/// <summary>
		/// Выход пользователя.
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> Logout() {
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Login");
		}

		/// <summary>
		/// Не хватает прав доступа.
		/// </summary>
		/// <param name="resourceName"></param>
		/// <returns></returns>
		public async Task<IActionResult> AccessDenied(string resourceName) {
			return View((Object)resourceName);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error() {
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
