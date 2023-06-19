using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicSchoolEF.Helpers.ReportBuilders;
using MusicSchoolEF.Helpers.TreeBuilders;
using MusicSchoolEF.Models.Db;
using MusicSchoolEF.Models.Defaults;
using MusicSchoolEF.Repositories.Interfaces;
using OfficeOpenXml;

namespace MusicSchoolEF.Controllers
{
	[Authorize(Roles = $"{Roles.Admin}, {Roles.Teacher}, {Roles.Student}")]
	public class ReportController : Controller
	{
		private readonly IStudentNodeConnectionRepository _studentNodeConnectionRepository;

		public ReportController(IStudentNodeConnectionRepository studentNodeConnectionRepository) 
		{ 
			_studentNodeConnectionRepository = studentNodeConnectionRepository;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> GenerateStudentReport(uint id)
		{
			// Находим все занятия, `Student` которых равен текущему ученику (`Student` == `id`)
			var allStudentTasks = await _studentNodeConnectionRepository
				.GetStudentNodeConnectionsByStudentIdAsync(id);

			TreeNode<StudentNodeConnection> tree = allStudentTasks != null
				? TreeBuilder.GetStudentNodesTree(allStudentTasks)
				: new TreeNode<StudentNodeConnection>();

			// note Лицензия, без которой выдаёт ошибку
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

			var report = ReportBuilder.GetStudentTaskExcelReport(tree);
			var fileBytes = report.GetAsByteArray();
			var fileName = $"report.xlsx";

			return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
		}
	}
}
