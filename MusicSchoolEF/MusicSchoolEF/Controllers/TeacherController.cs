using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicSchoolEF.Helpers;
using MusicSchoolEF.Helpers.HtmlStrings;
using MusicSchoolEF.Helpers.TreeBuilders;
using MusicSchoolEF.Helpers.TreeRenders;
using MusicSchoolEF.Models.Db;
using MusicSchoolEF.Models.Defaults;
using MusicSchoolEF.Models.ViewModels;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MusicSchoolEF.Helpers.HtmlStrings.TreeRenders;
using Microsoft.AspNetCore.Authorization;

namespace MusicSchoolEF.Controllers
{
	[Authorize(Roles = $"{Roles.Admin}, {Roles.Teacher}")]
	[Route("Teacher/{id:int}/{action}")]
	public class TeacherController : Controller
	{
		private readonly Ms2Context _dbContext;

		public TeacherController(Ms2Context dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IActionResult> Index(uint id)
		{
			User teacher = await _dbContext.Users
				.SingleAsync(u => u.Id == id);

			return View(teacher);
		}

		#region Область, отвечающая за операции с заданиями, создаталем которых является авторизированный учитель

		//public IActionResult Tasks(uint id)
		//{
		//	return View(id);
		//}

		[HttpGet]
		public async Task<IActionResult> Tasks(uint id)
		{
			// Находим все занятия, `Owner` которых равен текущему учителю (`Owner` == `id`)
			var allTeacherTasks = await _dbContext.Nodes
				.Include(node => node.InverseParentNavigation) // Подключаем детей у задания
				.Where(node => node.Owner == id)
				.ToListAsync();

			var tree = TreeBuilder.GetTeacherNodesTree(allTeacherTasks);

			//var model = new TeacherTaskViewModel()
			//{
			//	TaskTree = tree,
			//	EditViewModel = new TeacherTaskEditViewModel()
			//};
			return View(tree);
		}

		//#region Дерево вершин учителя

		///// <param name="id">`id` учителя</param>
		//[HttpGet]
		//public async Task<IActionResult> _TaskTree(uint id)
		//{
		//	// Находим все занятия, `Owner` которых равен текущему учителю (`Owner` == `id`)
		//	var allTeacherTasks = await _dbContext.Nodes
		//		.Include(node => node.InverseParentNavigation) // Подключаем детей у задания
		//		.Where(node => node.Owner == id)
		//		.ToListAsync();

		//	var tree = TreeBuilder.GetTeacherNodesTree(allTeacherTasks);

		//	return View(tree);
		//}

		//#endregion


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
			// Находим `node` по `taskId`
			var node = await _dbContext.Nodes
				.SingleAsync(n => n.Id == taskId.Value);

			var viewModel = new TeacherTaskEditViewModel()
			{
				TaskId = node.Id,
				Name = node.Name,
				Description = node.Description
			};
			return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> EditTask(TeacherTaskEditViewModel model)
		{
			if (ModelState.IsValid)
			{
				Node editingNode = await _dbContext.Nodes
					.SingleAsync(n => n.Id == model.TaskId);

				editingNode.Name = model.Name;
				editingNode.Description = model.Description;

				await _dbContext.SaveChangesAsync();

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
					Parent = model.ParentId,
					Priority = model.Priority
				};

				await _dbContext.Nodes.AddAsync(newNode);
				await _dbContext.SaveChangesAsync();

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
				var deletingNodes = await _dbContext.Nodes
					.Where(n => n.Owner == id && n.Parent == null)
					.ToArrayAsync();
				_dbContext.Nodes.RemoveRange(deletingNodes);
				await _dbContext.SaveChangesAsync();

				return Ok();
			}
			var deletingNode = await _dbContext.Nodes
				.SingleAsync(n => n.Id == taskId.Value);

			_dbContext.Nodes.Remove(deletingNode);
			await _dbContext.SaveChangesAsync();
			return Ok();
		}

		#endregion

		//[HttpPost]
		//public async Task<IActionResult> Tasks(TeacherTaskViewModel model)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		// Находим задание по `id` из БД
		//		// note Вершина должна наверняка существовать, поэтому нет проверки на `null`
		//		Node editingTask = await _dbContext.Nodes
		//			.SingleAsync(n => n.Id == model.EditViewModel.Id);
		//		// Изменяем значения вершины
		//		editingTask.Name = model.EditViewModel.Name;
		//		editingTask.Description = model.EditViewModel.Description;

		//		await _dbContext.SaveChangesAsync();

		//		return Ok();
		//	}
		//	// todo Возвращаем форму входа с сообщениями об ошибках
		//	return View(model);
		//}
		#endregion

		#region Оценивание заданий учеников

		public async Task<IActionResult> TaskAssessment(uint id)
		{
			// Поиск всех учеников, которым заданы задания, создателем которых является авторизованный учитель
			var allStudents = await _dbContext.Users
				.Include(s => s.StudentNodeConnections)
				.ThenInclude(snc => snc.NodeNavigation)
				// Ищем, привязано ли к ученику хотя бы одно задание, создаталем которого является учитель
				.Where(s => s.StudentNodeConnections.Any(snc => snc.NodeNavigation.Owner == id))
				// Сортируем по ФИО
				.OrderBy(s => s.Surname)
				.ThenBy(s => s.FirstName)
				.ThenBy(s => s.Patronymic)
				.ToListAsync();

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
			// Находим все задание студента, создателем которого является авториизированный учитель 
			var allTasks = await _dbContext.StudentNodeConnections
				.Include(snc => snc.NodeNavigation)
				.Where(snc => snc.Student == studentId
							&& snc.NodeNavigation.Owner == id)
				.ToListAsync();

			var tree = TreeBuilder.GetStudentNodesTree(allTasks);
			var html = TreeRender.RenderTree(tree, "Задания");

			return html;

			//string html = "";
			//// Составляем html-строку списка `<li>`
			//foreach (var task in tasks)
			//{
			//	html += $@"<li 
			//		class='student-task-node' 
			//		data-student_id='{task.Student}' 
			//		data-task_id='{task.Node}'
			//		data-task_name='{task.NodeNavigation.Name}' 
			//		data-task_description='{task.NodeNavigation.Description}' 
			//		data-task_mark='{task.Mark}'
			//		data-task_comment='{task.Comment}'
			//		>{task.NodeNavigation.Name}</li>";
			//}

			// Возвращаем HTML-код в виде строки
			//return html;
		}

		[HttpGet]
		public async Task<IActionResult> EditStudentTask(uint? studentId, uint? nodeId)
		{
			if (!studentId.HasValue || !nodeId.HasValue)
			{
				TempData["ErrorMessage"] = "Ошибка: не выбран студент или задание.";
				return RedirectToAction("TaskAssessment");
			}

			var studentTask = await _dbContext.StudentNodeConnections
				.Include(snc => snc.NodeNavigation)
				.SingleAsync(snc => snc.Student == studentId && snc.Node == nodeId);

			var viewModel = new StudentTaskEditViewModel()
			{
				TaskId = nodeId.Value,
				StudentId = studentId.Value,
				Name = studentTask.NodeNavigation.Name,
				Description = studentTask.NodeNavigation.Description,
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
				uint? nodeId = viewModel.TaskId;
				uint? studentId = viewModel.StudentId;
				// Находим вершину по первичному ключу
				var editingNode = await _dbContext.StudentNodeConnections
					.SingleAsync(snc => snc.Node == nodeId && snc.Student == studentId);
				// Изменяем значения по форме
				editingNode.Mark = viewModel.Mark;
				editingNode.Comment = viewModel.Comment;
				// Сохраняем изменения в БД
				await _dbContext.SaveChangesAsync();
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
			var allTeacherTasks = await _dbContext.Nodes
				.Include(node => node.InverseParentNavigation) // Подключаем детей у задания
				.Where(node => node.Owner == id && !node.Parent.HasValue)
				.ToListAsync();

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

            Node task = await _dbContext.Nodes
				.SingleAsync(n => n.Id == taskId);

			var students = await _dbContext.Users
				.Include(u => u.StudentNodeConnections)
				.ThenInclude(snc => snc.NodeNavigation)
				.Where(u => u.Role == "student")
				.ToListAsync();

			var groups = await _dbContext.Groups
				.ToListAsync();

			// Создание ViewModel и заполнение списков
			var viewModel = new TaskAssignmentViewModel
			{
				Task = task,
				Students = students.Select(s => new StudentViewModel
				{
					Id = s.Id,
					Name = $"{s.Surname} {s.FirstName} {s.Patronymic}",
					// Устанавливаем, привязано ли к студенту текущее задание
					IsChecked = s.StudentNodeConnections.Any(n => n.NodeNavigation.Id == taskId)
				}).ToList()
			};

			// todo сделать представление с логикой
			return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> AssignTask(TaskAssignmentViewModel model)
		{
			// Получить состояние чекбоксов студентов из модели
			List<StudentViewModel> students = model.Students;

			Node task = await _dbContext.Nodes
				.Include(n => n.InverseParentNavigation)
				.SingleAsync(n => n.Id == model.Task.Id);

			// Находим всех потомков данного задания
			List<Node> nodeAndDescendants = Node.GetNodeAndDescendants(task);
			foreach (var student in students)
			{
				// Пытаемся подписываем на все них выбранных студентов
				foreach (var node in nodeAndDescendants)
				{
					bool isInDb = _dbContext.StudentNodeConnections.Any(snc => snc.Node == node.Id && snc.Student == student.Id);
					if (student.IsChecked)
					{
						// Если ещё не добавлено, то добавляем
						if (!isInDb)
						{
							await _dbContext.StudentNodeConnections.AddAsync(
								new StudentNodeConnection()
								{
									Node = node.Id,
									Student = student.Id
								});
						}
					}
					// Пытаемся отписать не выбранных студентов
					else
					{
						// Если уже есть, то удаляем
						if (isInDb)
						{
							_dbContext.StudentNodeConnections.Remove(
								_dbContext.StudentNodeConnections.Single(snc => snc.Node == node.Id && snc.Student == student.Id)
							);
						}
					}
				}

			}
			await _dbContext.SaveChangesAsync();

			return RedirectToAction("TaskAssignment");
		}

		#endregion
	}
}
