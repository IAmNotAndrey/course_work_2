using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicSchoolAsp.Helpers.Converters;
using MusicSchoolAsp.Helpers.ReportBuilders;
using MusicSchoolAsp.Helpers.TreeBuilders;
using MusicSchoolAsp.Models.Db;
using MusicSchoolAsp.Models.Defaults;
using MusicSchoolAsp.Repositories.Interfaces;
using OfficeOpenXml;

namespace MusicSchoolAsp.Controllers
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
		public async Task<IActionResult> GenerateStudentReport(uint id, ReportExtension reportExtension)
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
			byte[] fileBytes;
			string fileName;

			switch (reportExtension)
			{
				case ReportExtension.Xlsx:
					fileBytes = await report.GetAsByteArrayAsync();
					fileName = $"report.xlsx";

					return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

				case ReportExtension.Csv:
					fileBytes = report.ConvertToCsv();
					fileName = $"report.csv";

					return File(fileBytes, "text/csv", fileName);

				default:
					return BadRequest();
			}
		}
	}
}
