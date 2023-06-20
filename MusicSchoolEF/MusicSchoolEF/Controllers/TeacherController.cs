using Microsoft.AspNetCore.Mvc;
using MusicSchoolEF.Helpers.TreeBuilders;
using MusicSchoolEF.Models.Db;
using MusicSchoolEF.Models.Defaults;
using MusicSchoolEF.Models.ViewModels;
using MusicSchoolEF.Helpers.HtmlStrings.TreeRenders;
using Microsoft.AspNetCore.Authorization;
using MusicSchoolEF.Repositories.Interfaces;
using MusicSchoolEF.Helpers.ReportBuilders;
using OfficeOpenXml;
using System.Threading.Tasks;
using static MusicSchoolEF.Repositories.UserRepositoryExtensions;

namespace MusicSchoolEF.Controllers
{
    [Authorize(Roles = $"{Roles.Admin}, {Roles.Teacher}")]
	[Route("Teacher/{id:int}/{action=Index}")]
    public class TeacherController : Controller
	{
		//private readonly Ms2Context _dbContext;
		private readonly IUserRepository _userRepository;
		private readonly INodeRepository _nodeRepository;
		private readonly IStudentRepository _studentRepository;
		private readonly IStudentNodeConnectionRepository _studentNodeConnectionRepository;
		private readonly IGroupRepository _groupRepository;

		public TeacherController(
			IUserRepository userRepository, 
			INodeRepository nodeRepository,
			IStudentRepository studentRepository,
			IStudentNodeConnectionRepository studentNodeConnectionRepository,
			IGroupRepository groupRepository)
		{
			//_dbContext = dbContext;
			_userRepository = userRepository;
			_nodeRepository = nodeRepository;
			_studentRepository = studentRepository;
			_studentNodeConnectionRepository = studentNodeConnectionRepository;
			_groupRepository = groupRepository;
		}

		public async Task<IActionResult> Index(uint id)
		{
			User teacher = await _userRepository.GetUserByIdAsync(id) 
				?? throw new Exception("Пользователь по заданному `id` не был найден");

			return View(teacher);
		}

		#region Операции с заданиями, создаталем которых является авторизованный учитель

		[HttpGet]
		public async Task<IActionResult> Tasks(uint id)
		{
			// Находим все занятия, `Owner` которых равен текущему учителю (`Owner` == `id`)
			List<Node> allTeacherTasks = await _nodeRepository.GetNodesByOwnerIdAsync(id);
			TreeNode<Node> tree = TreeBuilder.GetTeacherNodesTree(allTeacherTasks);

			return View(tree);
		}

		#region Форма для редактирования вершины учителя

		//bug возможен переход к методу через url, минуя кнопку выбора элемента => непредсказуемое поведение
		// Например, доступ к вершинам, создателем которых не является авторизированный учитель
		// todo сделать возможным переход только через кнопки
		[HttpGet]
		public async Task<IActionResult> EditTask(uint? taskId)
		{
			if (!taskId.HasValue)
			{
				TempData["ErrorMessage"] = "Ошибка: не выбрано задание.";
				return RedirectToAction("Tasks");
			}

			Node node = await _nodeRepository.GetNodeByIdAsync(taskId.Value)
				?? throw new NullReferenceException("`Node` по заданному `id` не был найден");

			var viewModel = new TeacherTaskEditViewModel()
			{
				TaskId = node.Id,
				Name = node.Name,
				Description = node.Description,
				Priority = node.Priority,
			};
			return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> EditTask(TeacherTaskEditViewModel model)
		{
			if (ModelState.IsValid)
			{
				Node? editingNode = await _nodeRepository.GetNodeByIdAsync(model.TaskId)
					?? throw new NullReferenceException("`Node` по заданному `id` не был найден");

				await _nodeRepository.EditNodeNameAndDescriptionAsync(
					editingNode,
                    model
                );

				return RedirectToAction("Tasks");
			}
			return View(model);
		}

		#endregion

		#region Добавление дочерней вершины

		[HttpGet]
		public IActionResult AddTask(uint? parentId)
		{
			var viewModel = new TeacherTaskAddViewModel()
			{
				ParentId = parentId
			};
			return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> AddTask(TeacherTaskAddViewModel model, uint id)
		{
			if (ModelState.IsValid)
			{
				// Так как мы создаём новую вершину, то свойство `Id` мы не задействуем: оно создаётся БД автоматически
				Node newNode = new Node()
				{
					Name = model.Name,
					Owner = id,
					Description = model.Description,
					ParentId = model.ParentId,
					Priority = model.Priority
				};

				await _nodeRepository.AddAsync(newNode);

				return RedirectToAction("Tasks");
			}
			return View(model);
		}

		#endregion

		#region Удаление вершины и её потомков

		[HttpDelete]
		public async Task<IActionResult> DeleteTask(uint id, uint? taskId)
		{
			if (!taskId.HasValue)
			{
				// Удаляем абсолютно ВСЕ вершины
				List<Node> deletingNodes = await _nodeRepository.GetNodesByOwnerAndParentAsync(id, null);
				await _nodeRepository.RemoveRangeAsync(deletingNodes);

				return Ok();
			}

			Node deletingNode = await _nodeRepository.GetNodeByIdAsync(taskId.Value)
				?? throw new NullReferenceException("`Node` по заданному `id` не был найден");
		
			await _nodeRepository.RemoveAsync(deletingNode);

			return Ok();
		}

		#endregion

		#endregion

		#region Оценивание заданий учеников

		public async Task<IActionResult> TaskAssessment(uint id)
		{
			// Поиск всех учеников, которым заданы задания, создателем которых является авторизованный учитель
			// + Сортируем их по ФИО
			var allStudents = (
				(IQueryable<User>)
				await _studentRepository
				.GetStudentsAssignedTasksByTeacherAsync(id))
				.GetSortedUsersByFullName();

			//// Создаём пару (студент-его дерево заданий)
			//var pairs = new List<(User Student, TreeNode<StudentNodeConnection> Tree)>();
			//foreach (var s in allStudents)
			//{
			//	// Находим все задания студента, создателем которого является авторизованный учитель
			//	var collection = s.StudentNodeConnections
			//		.Where(snc => snc.NodeNavigation.Owner == id)
			//		.ToList();
			//	// Создаём дерево для студента
			//	var tree = TreeBuilder.GetStudentNodesTree(collection);
			//	pairs.Add((s, tree));
			//}
			//return View(pairs);

			return View(allStudents);
		}

		[HttpGet]
		public async Task<string> GetStudentTaskTree(uint id, uint studentId)
		{
			// Находим все задание студента, создателем которого является авторизованный учитель 
			List<StudentNodeConnection> allTasks = await _studentNodeConnectionRepository
				.GetStudentNodeConnectionsByStudentIdAndTeacherIdAsync(studentId, id);

			var tree = TreeBuilder.GetStudentNodesTree(allTasks);
			var html = TreeRender.RenderTree(tree, "Задания");

			return html;
		}

		[HttpGet]
		public async Task<IActionResult> EditStudentTask(uint? studentId, uint? nodeId)
		{
			if (!studentId.HasValue || !nodeId.HasValue)
			{
				TempData["ErrorMessage"] = "Ошибка: не выбран студент или задание.";
				return RedirectToAction("TaskAssessment");
			}

			StudentNodeConnection? studentTask = await _studentNodeConnectionRepository.GetStudentNodeConnectionByPrimaryKey(studentId.Value, nodeId.Value)
				?? throw new NullReferenceException("`StudentNodeConnection` по заданному `studentId` и `nodeId` не был найден");

			var viewModel = new StudentTaskEditViewModel()
			{
				TaskId = nodeId.Value,
				StudentId = studentId.Value,
				Name = studentTask.Node.Name,
				Description = studentTask.Node.Description,
				Mark = studentTask.Mark,
				Comment = studentTask.Comment
			};

			return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> EditStudentTask(StudentTaskEditViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				uint nodeId = viewModel.TaskId;
				uint studentId = viewModel.StudentId;
				// Находим вершину по первичному ключу
				StudentNodeConnection? editingSnc = await _studentNodeConnectionRepository.GetStudentNodeConnectionByPrimaryKey(studentId, nodeId)
					?? throw new NullReferenceException("`StudentNodeConnection` не был найден");

				// Изменяем значения по форме
				await _studentNodeConnectionRepository.EditAsync(editingSnc, viewModel.Mark, viewModel.Comment);
				// Возвращаемся на страницу с оцениванием заданий
				return RedirectToAction("TaskAssessment", "Teacher");
			}
			return View(viewModel);
		}
		#endregion

		#region Назначение заданий

		[HttpGet]
		public async Task<IActionResult> TaskAssignment(uint id)
		{
			// Находим все КОРНЕВЫЕ занятия, `Owner` которых равен текущему учителю (`Owner` == `id`)
			List<Node> allTeacherTasks = await _nodeRepository.GetNodesByOwnerAndParentAsync(id, null);

			return View(allTeacherTasks);
		}

		[HttpGet]
		public async Task<IActionResult> AssignTask(uint? taskId)
		{
            if (!taskId.HasValue)
            {
                TempData["ErrorMessage"] = "Ошибка: не выбрано задание.";
                return RedirectToAction("TaskAssignment");
            }

			Node task = await _nodeRepository.GetNodeByIdAsync(taskId.Value)
				?? throw new NullReferenceException();
			// Находим всех студентов и сортируем по ФИО
			var students = 
				((IQueryable<User>)
				await _studentRepository.GetAllStudentsAsync())
				.GetSortedUsersByFullName();

			// Создание ViewModel и заполнение списков
			var viewModel = new StudentTaskAssignmentViewModel
			{
				Task = task,
				Students = students.Select(s => new StudentCheckBoxViewModel
                {
					Id = s.Id,
					Name = $"{s.Surname} {s.FirstName} {s.Patronymic}",
					// Устанавливаем, привязано ли к студенту текущее задание
					IsChecked = s.StudentNodeConnections.Any(n => n.NodeId == taskId)
				}).ToList(),
            };

			return View(viewModel);
		}
        [HttpPost]
		public async Task<IActionResult> AssignTask(StudentTaskAssignmentViewModel model)
		{
			// Получить состояние чекбоксов студентов из модели
			List<StudentCheckBoxViewModel> students = model.Students;

			Node task = await _nodeRepository.GetNodeByIdAsync(model.Task.Id)
				?? throw new NullReferenceException();
			// Находим всех потомков данного задания
			List<Node> nodeAndDescendants = Node.GetNodeAndDescendants(task);

			await _studentNodeConnectionRepository.UpdateTable(students, nodeAndDescendants);

			return RedirectToAction("TaskAssignment");
		}

		[HttpGet]
		public async Task<IActionResult> AssignGroupTask(uint? taskId)
		{
            if (!taskId.HasValue)
            {
                TempData["ErrorMessage"] = "Ошибка: не выбрано задание.";
                return RedirectToAction("TaskAssignment");
            }

            Node task = await _nodeRepository.GetNodeByIdAsync(taskId.Value)
                ?? throw new NullReferenceException();

            List<Group> groups = await _groupRepository.GetAllGroupsAsync();

            // Создание ViewModel и заполнение списков
            var viewModel = new GroupTaskAssignmentViewModel
            {
                Task = task,
                Groups = groups.Select(g => new GroupCheckBoxViewModel
                {
                    Name = g.Name,
					StudentIds = g.Students.Select(s => s.Id).ToList(),
                    // Устанавливаем, привязано ли к студенту текущее задание
					State = g.Students.All(s => s.StudentNodeConnections.Any(snc => snc.NodeId == taskId)) 
						? 1 // Все ученики привязаны
						: (g.Students.Any(s => s.StudentNodeConnections.Any(snc => snc.NodeId == taskId))
							? 0 // Некоторые привязаны
							: -1) // Никто не привязан
                }).ToList(),
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AssignGroupTask(GroupTaskAssignmentViewModel model)
        {
            // Получить состояний групп из модели
			List<GroupCheckBoxViewModel> groupModels = model.Groups;

            Node task = await _nodeRepository.GetNodeByIdAsync(model.Task.Id)
                ?? throw new NullReferenceException();
            // Находим всех потомков данного задания
            List<Node> nodeAndDescendants = Node.GetNodeAndDescendants(task);
			// Устанавливаем `IsChecked` для студентов
			List<StudentCheckBoxViewModel> students = StudentCheckBoxViewModel.GetStudentCheckBoxListByGroups(groupModels);

			await _studentNodeConnectionRepository.UpdateTable(students, nodeAndDescendants);

			return RedirectToAction("TaskAssignment");
        }

        #endregion

        [HttpGet]
        public IActionResult TryGenerateStudentTaskReport(uint? studentId)
        {
            if (!studentId.HasValue)
            {
                TempData["ErrorMessage"] = "Ошибка: не выбран ученик.";
                return RedirectToAction("TaskAssessment", "Teacher");
            }

			return RedirectToAction("GenerateStudentReport", "Report", new { id = studentId.Value });
        }
    }
}
