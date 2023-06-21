using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicSchoolEF.Helpers.ReportBuilders;
using MusicSchoolEF.Helpers.TreeBuilders;
using MusicSchoolEF.Models.Db;
using MusicSchoolEF.Models.Defaults;
using MusicSchoolEF.Repositories.Interfaces;
using OfficeOpenXml;

namespace MusicSchoolEF.Controllers
{
    //[Authorize(Roles = $"{Roles.Admin}, {Roles.Student}")]
    [Route("Student/{id:int}/{action=Index}")]
	[Authorize(Roles = $"{Roles.Admin}, {Roles.Student}")]
	public class StudentController : Controller
	{
        private readonly IStudentNodeConnectionRepository _studentNodeConnectionRepository;
        private readonly IUserRepository _userRepository;

		public StudentController(
            IStudentNodeConnectionRepository studentNodeConnectionRepository,
            IUserRepository userRepository
            )
		{
			_studentNodeConnectionRepository = studentNodeConnectionRepository;
			_userRepository = userRepository;
		}

        [HttpGet]
		public async Task<IActionResult> Index(uint id)
        {
            User? student = await _userRepository.GetUserByIdAsync(id) 
                ?? throw new NullReferenceException("Пользователь по заданному `id` не был найден");

            return View(student);
        }

        [HttpGet]
		public async Task<IActionResult> Tasks(uint id)
        {
            // Находим все занятия, `Student` которых равен текущему ученику (`Student` == `id`)
            var allStudentTasks = await _studentNodeConnectionRepository
                .GetStudentNodeConnectionsByStudentIdAsync(id);

            var tree = allStudentTasks != null 
                ? TreeBuilder.GetStudentNodesTree(allStudentTasks) 
                : new TreeNode<StudentNodeConnection>();

            return View(tree);
        }

        //      [HttpGet]
        //public async Task<IActionResult> GenerateReport(uint id)
        //      {
        //          // Находим все занятия, `Student` которых равен текущему ученику (`Student` == `id`)
        //          var allStudentTasks = await _studentNodeConnectionRepository
        //              .GetStudentNodeConnectionsByStudentIdAsync(id);

        //          TreeNode<StudentNodeConnection> tree = allStudentTasks != null
        //              ? TreeBuilder.GetStudentNodesTree(allStudentTasks)
        //              : new TreeNode<StudentNodeConnection>();

        //          // note Лицензия, без которой выдаёт ошибку
        //	ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //	var report = ReportBuilder.GetStudentTaskExcelReport(tree);
        //	var fileBytes = report.GetAsByteArray();
        //	var fileName = $"report.xlsx";

        //	return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        //   }

        [HttpGet]
        public IActionResult GenerateXlsxReport(uint id)
        {
			return RedirectToAction("GenerateStudentReport", "Report", new { id = id, reportExtension = ReportExtension.Xlsx });
		}

		[HttpGet]
		public IActionResult GenerateCsvReport(uint id)
		{
			return RedirectToAction("GenerateStudentReport", "Report", new { id = id, reportExtension = ReportExtension.Csv });
		}
	}
}
