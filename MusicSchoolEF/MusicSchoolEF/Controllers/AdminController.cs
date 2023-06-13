using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicSchoolEF.Models.Db;
using MusicSchoolEF.Models.Defaults;

namespace MusicSchoolEF.Controllers
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

        public async Task<IActionResult> Index(uint id)
        {
            var students = await _dbContext.Users
                .Where(u => u.Role == Roles.Student)
                .ToListAsync();
            var teachers = await _dbContext.Users
               .Where(u => u.Role == Roles.Teacher)
               .ToListAsync();

            var model =
            (
                Students: students,
                Teachers: teachers
            );

            return View(model);
        }
    }
}
