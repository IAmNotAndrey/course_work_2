using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicSchoolEF.Helpers.TreeBuilders;
using MusicSchoolEF.Models.Db;
using MusicSchoolEF.Models.Defaults;

namespace MusicSchoolEF.Controllers
{
    [Authorize(Roles = $"{Roles.Admin}, {Roles.Student}")]
    [Route("Student/{id:int}/{action=Index}")]
	public class StudentController : Controller
	{
		private readonly Ms2Context _dbContext;
        //private User _student = null!;

		public StudentController(Ms2Context dbContext)
		{
			_dbContext = dbContext;
		}

        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            User student = await _dbContext.Users
                // note добавить методы `include` по необходимости
                .SingleAsync(u => u.Id == id);

            return View(student);
        }

        public async Task<IActionResult> Tasks(int id)
        {
			// Находим все занятия, `Student` которых равен текущему ученику (`Student` == `id`)
			var allStudentTasks = await _dbContext.StudentNodeConnections
                .Include(snc => snc.NodeNavigation) // Подключение отображения `Node`
                .Include(snc => snc.NodeNavigation.InverseParentNavigation) // Подключение потомков
                .Include(snc => snc.NodeNavigation.OwnerNavigation) // Подключение учителя
                .Where(snc => snc.Student == id) // Возвращение по `id` студента `StudentNodeConnection`
                .ToListAsync();

            var tree = TreeBuilder.GetStudentNodesTree(allStudentTasks);

            return View(tree);
        }
    }
}
