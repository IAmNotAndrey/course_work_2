using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicSchoolAsp.Helpers;
using MusicSchoolAsp.Models.Db;
using MusicSchoolAsp.Models.Defaults;
using MusicSchoolAsp.Repositories;
using System.Collections.Immutable;
using static MusicSchoolAsp.Repositories.UserRepositoryExtensions;

namespace MusicSchoolAsp.Controllers
{
	[Authorize(Roles = Roles.Admin)]
	[Route("Admin/{id:int}/{action=Index}")]
	public class AdminController : Controller
	{
		private readonly Ms2Context _dbContext;

		public AdminController(Ms2Context dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IActionResult> Index()
		{
			//var students = await _dbContext.Users
			//    .Where(u => u.Role == Roles.Student)
			//    .ToListAsync();
			//var teachers = await _dbContext.Users
			//   .Where(u => u.Role == Roles.Teacher)
			//   .ToListAsync();

			IQueryable<User> studentsAndTeachers = _dbContext.Users
				.Where(u => u.Role.Name == Roles.Teacher || u.Role.Name == Roles.Student)
				.GetSortedUsersByFullName();

			//var model =
			//(
			//    Students: students,
			//    Teachers: teachers
			//);
			return View(studentsAndTeachers);
		}

		public IActionResult Search(string query)
		{
			if (string.IsNullOrEmpty(query))
				return RedirectToAction("Index");

			query = query.MyTrim().ToLower();
			//         var students = _dbContext.Users
			//             .AsEnumerable()
			//	.Where(u => u.Role == Roles.Student
			//                 && u.FullName.Contains(query))
			//             .ToList();
			//var teachers = _dbContext.Users
			//	.AsEnumerable()
			//	.Where(u => u.Role == Roles.Teacher 
			//                 && u.FullName.Contains(query))
			//             .ToList();
			IQueryable<User> studentsAndTeachers = _dbContext.Users
				.AsEnumerable()
				.Where(u => (u.Role.Name == Roles.Student
				|| u.Role.Name == Roles.Teacher)
				&& u.FullName.ToLower().Contains(query))
				.AsQueryable()
				.GetSortedUsersByFullName();

			//var model =
			//(
			//	Students: students,
			//	Teachers: teachers
			//);
			return View("Index", studentsAndTeachers);
		}
	}
}
