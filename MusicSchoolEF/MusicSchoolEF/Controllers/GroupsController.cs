using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using MusicSchoolEF.Models.Db;
using MusicSchoolEF.Models.Defaults;
using MySqlConnector;
using static MusicSchoolEF.Helpers.StringHelper;

namespace MusicSchoolEF.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class GroupsController : Controller
    {
        private readonly Ms2Context _context;

        public GroupsController(Ms2Context context)
        {
            _context = context;
        }

        // GET: Groups
        public async Task<IActionResult> Index()
        {
              return _context.Groups != null ? 
                          View(await _context.Groups.ToListAsync()) :
                          Problem("Entity set 'Ms2Context.Groups'  is null.");
        }

        // GET: Groups/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups
                .FirstOrDefaultAsync(m => m.Name == id);
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        // GET: Groups/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Students = await _context.Users
                .Where(u => u.Role.Name == Roles.Student)
                .ToListAsync();
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Group group, List<uint> studentIds)
        {
            if (ModelState.IsValid)
            {
                group.Name = group.Name.MyTrim();

                // Получаем объекты студентов из БД на основе выбранных идентификаторов
                List<User> selectedStudents = await _context.Users
                    .Where(u => studentIds.Contains(u.Id))
                    .ToListAsync();

                group.Students = selectedStudents;

                _context.Groups.Add(group);
                // Обработка случая добавления дубликата
                try
                {
                    await _context.SaveChangesAsync();
                } 
                catch (DbUpdateException)
                {
                    TempData["ErrorMessage"] = "Ошибка: группа с таким названием уже существует";
                    ViewBag.Students = await _context.Users.Where(u => u.Role.Name == Roles.Student).ToListAsync();
                    return View(group);
                }

                return RedirectToAction(nameof(Index));
            }
            ViewBag.Students = await _context.Users.Where(u => u.Role.Name == Roles.Student).ToListAsync();
            return View(group);
        }

        // note : закомментированы т.к. изменение первичного ключа происходит напрямую без `id`, что некорректно
        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var group = await _context.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }

            var students = await _context.Users
                .Where(u => u.Role.Name == Roles.Student)
                .ToListAsync();

            ViewBag.Students = students;
            ViewBag.GroupStudents = group.Students;

            return View(group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Name")] Group group, List<uint> studentIds)
        {
            //if (id != group.Name)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                // Получаем объекты студентов из БД на основе выбранных идентификаторов
                List<User> selectedStudents = await _context.Users
                    .Where(u => studentIds.Contains(u.Id))
                    .ToListAsync();

                // Получаем группу по исходному имени
                Group groupByOriginalName = await _context.Groups
                    .Include(g => g.Students)
                    .SingleOrDefaultAsync(g => g.Name == id)
                    ?? throw new NullReferenceException();

                group.Name = group.Name.MyTrim();
                group.Students = selectedStudents;

                // Если у новое название группы не совпадает с прежним, то удаляем группу с прежним названием и добавляем с новым
                if (id != group.Name)
                {
                    _context.Remove(groupByOriginalName);
                    _context.Groups.Add(group);
                }
                // Если совпадает, то просто изменяем выбранных студентов
                else
                {
                    groupByOriginalName.Students = selectedStudents;
                }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    group.Name = id;
                    TempData["ErrorMessage"] = "Ошибка: группа с таким названием уже существует";
                    ViewBag.Students = await _context.Users.Where(u => u.Role.Name == Roles.Student).ToListAsync();
                    ViewBag.GroupStudents = group.Students;
                    return View(group);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Students = await _context.Users.Where(u => u.Role.Name == Roles.Student).ToListAsync();
            ViewBag.GroupStudents = group.Students;
            return View(group);
        }


        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups
                .FirstOrDefaultAsync(m => m.Name == id);
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Groups == null)
            {
                return Problem("Entity set 'Ms2Context.Groups'  is null.");
            }
            var @group = await _context.Groups.FindAsync(id);
            if (@group != null)
            {
                _context.Groups.Remove(@group);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupExists(string id)
        {
          return (_context.Groups?.Any(e => e.Name == id)).GetValueOrDefault();
        }
    }
}
