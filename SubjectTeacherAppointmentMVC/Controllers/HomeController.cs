using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubjectTeacherAppointmentMVC.Models;
using SubjectTeacherAppointmentMVC.Models.DataBase;
using SubjectTeacherAppointmentMVC.Models.DataBase.DataSets;
using SubjectTeacherAppointmentMVC.Utils;
using SubjectTeacherAppointmentMVC.Models.DataBase.Tables;
using System.Linq;

namespace SubjectTeacherAppointmentMVC.Controllers {

	[Authorize]
	public class HomeController : Controller {
		private readonly ILogger<HomeController> _logger;
		private ApplicationContext db;

		public HomeController(ILogger<HomeController> logger, ApplicationContext context) {
			_logger = logger;
			db = context;
		}

		#region UtilityMethods

		/// <summary>
		/// Проверка наличия доступа у текущего пользователя
		/// </summary>
		/// <returns></returns>
		private bool isUserAuthorized() {
			return HttpContext.User.FindFirst(ClaimTypes.Role)?.Value == "Admin";
		}

		#endregion

		/// <summary>
		/// Отображение главной страницы
		/// </summary>
		/// <returns></returns>
		public IActionResult Index() {
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error() {
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		/// <summary>
		/// Отображение предметов
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> Subjects() {
			var subjects = db.Subjects
				.OrderBy((s => s.Name))
				.ThenBy(s => s.CreationDate)
				.ToList();
			foreach (var subject in subjects) {
				subject.HoursPerWeekTotal = db.SubjectTeachers
					.Where(t => t.SubjectID == subject.SubjectID)
					.Sum(t => t.HoursPerWeek);
			}
			return View(subjects);
		}


		/// <summary>
		/// Добавление предмета
		/// </summary>
		/// <returns></returns>
		public IActionResult AddSubject() {
			if (!isUserAuthorized()) {
				return RedirectToAction("AccessDenied", "Authorization");
			}
			return View();
		}

		/// <summary>
		/// Добавление/изменение предмета
		/// </summary>
		/// <param name="subject"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> AddSubject(Subject subject) {
			if (!isUserAuthorized()) {
				return RedirectToAction("AccessDenied", "Authorization", new { resourceName = subject?.Name });
			}

			if (subject.SubjectID == null)
				db.Subjects.Add(subject);
			else {
				var subjectDB = db.Subjects.FirstOrDefault(f => f.SubjectID == subject.SubjectID);
				subjectDB.Name = subject.Name;
			}
			db.SaveChanges();

			return RedirectToAction("Subjects");
		}

		/// <summary>
		/// Редактирование предмета
		/// </summary>
		/// <param name="ID"></param>
		/// <returns></returns>
		public async Task<IActionResult> EditSubject(Guid ID) {
			var subject = await db.Subjects.FirstOrDefaultAsync(f => f.SubjectID == ID);
			if (!isUserAuthorized()) {
				return RedirectToAction("AccessDenied", "Authorization", new { resourceName = subject?.Name });
			}


			return View("AddSubject", subject);
		}

		/// <summary>
		/// Удаление предмета
		/// </summary>
		/// <param name="ID"></param>
		/// <returns></returns>
		public async Task<IActionResult> RemoveSubject(Guid ID) {
			var subjectDB = db.Subjects.FirstOrDefault(s => s.SubjectID == ID);
			if (!isUserAuthorized()) {
				return RedirectToAction("AccessDenied", "Authorization", new { resourceName = subjectDB?.Name });
			}

			if (subjectDB != null) {
				var subjectTeachersDB = db.SubjectTeachers.Where(t => t.SubjectID == ID).ToList();

				if (subjectTeachersDB != null) {
					db.SubjectTeachers.RemoveRange(subjectTeachersDB);
				}

				db.Subjects.Remove(subjectDB);
				db.SaveChanges();
			}
			return RedirectToAction("Subjects");
		}

		/// <summary>
		/// Преподаватели
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> Teachers() {
			var teachers = db.Teachers
				.ToList()
				.OrderBy(t => Formatting.getTeacherDisplayString(t))
				.ThenBy(t => t.CreationDate);
			return View(teachers);
		}

		/// <summary>
		/// Преподаватель
		/// </summary>
		/// <param name="ID"></param>
		/// <returns></returns>
		public async Task<IActionResult> Teacher(Guid ID) {
			var teacher = await db.Teachers.FirstOrDefaultAsync(f => f.TeacherID == ID);

			return View("Teacher", teacher);
		}


		/// <summary>
		/// Добавление преподавателя
		/// </summary>
		/// <returns></returns>
		public IActionResult AddTeacher() {
			if (!isUserAuthorized()) {
				return RedirectToAction("AccessDenied", "Authorization");
			}
			return View();
		}

		/// <summary>
		/// Добавление/изменение преподавателя
		/// </summary>
		/// <param name="teacher"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> AddTeacher(Teacher teacher) {
			if (!isUserAuthorized()) {
				return RedirectToAction("AccessDenied", "Authorization", new { resourceName = teacher?.LastName });
			}

			if(teacher.PhotoFormFile != null) {
				using (var fileStream = new MemoryStream()) {
					teacher.PhotoFormFile.CopyTo(fileStream);
					teacher.Photo = fileStream.ToArray();
				}
			}

			if (teacher.TeacherID == null)
				db.Teachers.Add(teacher);
			else {
				var teacherDB = db.Teachers.FirstOrDefault(f => f.TeacherID == teacher.TeacherID);
				teacherDB.LastName = teacher.LastName;
				teacherDB.Surname = teacher.Surname;
				teacherDB.Name = teacher.Name;
				teacherDB.Birthday = teacher.Birthday;
				teacherDB.Sex = teacher.Sex;
				teacherDB.Photo = teacher.Photo ?? teacherDB.Photo;
				teacherDB.Notes = teacher.Notes;
			}
			db.SaveChanges();

			return RedirectToAction("Teacher", new { ID = teacher.TeacherID });
		}

		/// <summary>
		/// Редактирование преподавателя
		/// </summary>
		/// <param name="ID"></param>
		/// <returns></returns>
		public async Task<IActionResult> EditTeacher(Guid ID) {
			var teacher = await db.Teachers.FirstOrDefaultAsync(f => f.TeacherID == ID);
			if (!isUserAuthorized()) {
				return RedirectToAction("AccessDenied", "Authorization", new { resourceName = teacher?.LastName });
			}


			return View("AddTeacher", teacher);
		}

		/// <summary>
		/// Удаление преподавателя
		/// </summary>
		/// <param name="ID"></param>
		/// <returns></returns>
		public async Task<IActionResult> RemoveTeacher(Guid ID) {
			var teacherDB = db.Teachers.FirstOrDefault(s => s.TeacherID == ID);
			if (!isUserAuthorized()) {
				return RedirectToAction("AccessDenied", "Authorization", new { resourceName = teacherDB?.LastName });
			}

			if (teacherDB != null) {
				var subjectTeachersDB = db.SubjectTeachers.Where(t => t.TeacherID == ID).ToList();

				if (subjectTeachersDB != null) {
					db.SubjectTeachers.RemoveRange(subjectTeachersDB);
				}

				db.Teachers.Remove(teacherDB);
				db.SaveChanges();
			}
			return RedirectToAction("Teachers");
		}

		/// <summary>
		/// Преподаватели предмета
		/// </summary>
		/// <param name="ID">Идентификатор предмета</param>
		/// <returns></returns>
		public async Task<IActionResult> SubjectTeachers(Guid ID) {
			var subject = db.Subjects.FirstOrDefault(s => s.SubjectID == ID);
			var subjectTeachers = db.SubjectTeachers.Where(f => f.SubjectID == ID).ToList();

			foreach(var subjectTeacher in subjectTeachers) {
				subjectTeacher.Teacher = db.Teachers.FirstOrDefault(t => t.TeacherID == subjectTeacher.TeacherID);
			}
			var orderedSubjectTeachers = subjectTeachers
				.ToList()
				.OrderBy(t => Formatting.getTeacherDisplayString(t.Teacher))
				.ThenBy(t => t.Teacher.CreationDate);
			return View("SubjectTeachers", new Tuple<Subject, IEnumerable<SubjectTeacher>>(subject, orderedSubjectTeachers));
		}

		/// <summary>
		/// Изменение преподавателей предмета
		/// </summary>
		/// <param name="ID">Идентификатор предмета</param>
		/// <returns></returns>
		public async Task<IActionResult> EditSubjectTeachers(Guid ID) {
			var subject = db.Subjects.FirstOrDefault(s => s.SubjectID == ID);
			var subjectTeachers = db.SubjectTeachers.Where(f => f.SubjectID == ID).ToList();
			if (!isUserAuthorized()) {
				return RedirectToAction("AccessDenied", "Authorization", new { resourceName = subject?.Name });
			}

			foreach(var subjectTeacher in subjectTeachers) {
				subjectTeacher.Teacher = db.Teachers.FirstOrDefault(t => t.TeacherID == subjectTeacher.TeacherID);
			}
			var orderedSubjectTeachers = subjectTeachers
				.ToList()
				.OrderBy(t => Formatting.getTeacherDisplayString(t.Teacher))
				.ThenBy(t => t.Teacher.CreationDate);
			var teachers = db.Teachers.ToList().OrderBy(t => Formatting.getTeacherDisplayString(t)).ThenBy(t => t.CreationDate);
			return View("EditSubjectTeachers", new Tuple<Subject, IEnumerable<SubjectTeacher>, IEnumerable<Teacher>>(subject, orderedSubjectTeachers, teachers));
		}

		/// <summary>
		/// Изменение преподавателей предмета
		/// </summary>
		/// <param name="subjectTeachers">Идентификатор предмета</param>
		/// <returns></returns>
		[HttpPost, DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = Int32.MaxValue, ValueLengthLimit = Int32.MaxValue, ValueCountLimit = Int32.MaxValue)]
		public async Task<IActionResult> EditSubjectTeachers([Bind(Prefix = "SubjectTeacher")] List<SubjectTeacher> subjectTeachers) {
			if (!isUserAuthorized()) {
				return RedirectToAction("AccessDenied", "Authorization");
			}

			var subjectId = subjectTeachers?.FirstOrDefault()?.SubjectID;
			subjectTeachers?.RemoveAll(t => (t.SubjectID ?? Guid.Empty) == Guid.Empty || (t.TeacherID ?? Guid.Empty) == Guid.Empty);
			var subjectTeachersForDeletionDB = db.SubjectTeachers.ToList().Where(t => t.SubjectID == subjectId && !(subjectTeachers?.Any(s => s.SubjectTeacherID == t.SubjectTeacherID) ?? false));
			db.SubjectTeachers.RemoveRange(subjectTeachersForDeletionDB);

			foreach (var subjectTeacher in subjectTeachers ?? new List<SubjectTeacher>()) {
				var subjectTeacherDB = db.SubjectTeachers.FirstOrDefault(t => t.SubjectTeacherID == subjectTeacher.SubjectTeacherID);
				if (subjectTeacherDB != null) {
					db.Entry(subjectTeacherDB).CurrentValues.SetValues(subjectTeacher);
				} else {
					db.SubjectTeachers.Add(subjectTeacher);
				}
			}

			db.SaveChanges();

			return RedirectToAction("SubjectTeachers", new { ID = subjectId });
		}

		#region UserSettings

		/// <summary>
		/// Обновление информации пользователя.
		/// </summary>
		/// <returns></returns>
		public IActionResult EditUser() {
			return View();
		}

		/// <summary>
		/// Обновление информации пользователя.
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> EditUser(UserInfoUpdate userInfoUpdate) {
			var userDB = db.Users.FirstOrDefault(u => u.UserID + "" == HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
			if (userDB == null) {
				return View();
			}
			if (!Enumerable.SequenceEqual(userDB.PasswordHashed, UserInfo.EncodePassword(userInfoUpdate.CurrentPassword, userDB.UserID))) {
				userInfoUpdate.CurrentPassword = null;
				return View(userInfoUpdate);
			}
			if (!string.IsNullOrEmpty(userInfoUpdate.NewPassword)) {
				var newPasswordArr = UserInfo.EncodePassword(userInfoUpdate.NewPassword, userDB.UserID);
				userDB.PasswordHashed = newPasswordArr;
			}
			db.SaveChanges();
			return View();
		}

		#endregion

		#region AdminViews

		/// <summary>
		/// Список пользователей.
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> Users() {
			if (HttpContext.User.FindFirstValue(ClaimTypes.Role) != "Admin") {
				return RedirectToAction("AccessDenied", "Authorization");
			}
			var usersDB = db.Users.ToList();
			return View(usersDB);
		}

		/// <summary>
		/// Выдача админских прав.
		/// </summary>
		/// <param name="users"></param>
		/// <returns></returns>
		[HttpPost, DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = Int32.MaxValue, ValueLengthLimit = Int32.MaxValue, ValueCountLimit = Int32.MaxValue)]
		public async Task<IActionResult> Users([Bind(Prefix = "Users")] List<User> users) {
			if (!isUserAuthorized()) {
				return RedirectToAction("AccessDenied", "Authorization");
			}

			var usersDB = db.Users.ToList();

			foreach (var user in users) {
				var userDB = usersDB.FirstOrDefault(u => u.UserID == user.UserID);
				if (userDB == null || userDB.Login == "admin" && HttpContext.User.Identity.Name != "admin") {
					continue;
				}

				if (userDB.Login != "admin" && userDB.Login != HttpContext.User.Identity?.Name) {
					userDB.IsAdmin = user.IsAdmin;
				}
			}

			db.SaveChanges();

			return RedirectToAction("Users");
		}

		#endregion

	}
}
