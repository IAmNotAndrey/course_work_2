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
using MusicSchoolEF.Repositories;
using MusicSchoolEF.Repositories.Interfaces;
using MySqlConnector;
using NuGet.Packaging;
using static MusicSchoolEF.Helpers.StringHelper;

namespace MusicSchoolEF.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class GroupsController : Controller
    {
        private readonly Ms2Context _context;
        private readonly IStudentRepository _studentRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;

        public GroupsController(Ms2Context context, IStudentRepository studentRepository, IGroupRepository groupRepository, IUserRepository userRepository)
        {
            _context = context;
            _studentRepository = studentRepository;
            _groupRepository = groupRepository;
            _userRepository = userRepository;
        }

        // GET: Groups
        public async Task<IActionResult> Index()
        {
              return _context.Groups != null ? 
                          View(await _context.Groups.ToListAsync()) :
                          Problem("Entity set 'Ms2Context.Groups'  is null.");
        }

        // GET: Groups/Details/5
        public async Task<IActionResult> Details(uint? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        // GET: Groups/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Students = await _studentRepository.GetAllStudentsAsync();
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Id")] Group group, List<uint> studentIds)
        {
            // Если не выбраны ученики, привязанные к группе
            if (studentIds.Count == 0)
            {
                TempData["ErrorMessage"] = "Ошибка: группа не может быть пустой";
                ViewBag.Students = await _studentRepository.GetAllStudentsAsync();
                return View(group);
            }
            // Проверяем на уникальность набора студентов у изменяемой группы
            if (_userRepository.DoesConsistOfSameStudents(
                studentIds,
                _context
                    .Groups
                    .Select(g => g.Students.Select(s => s.Id))))
            {
                TempData["ErrorMessage"] = "Ошибка: группа с таким набором студентов уже существует";
                ViewBag.Students = await _studentRepository.GetAllStudentsAsync();
                return View(group);
            }

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
                    ViewBag.Students = await _studentRepository.GetAllStudentsAsync();
                    return View(group);
                }

                return RedirectToAction(nameof(Index));
            }
            ViewBag.Students = await _studentRepository.GetAllStudentsAsync();
            return View(group);
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(uint? id)
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

            ViewBag.Students = await _studentRepository.GetAllStudentsAsync();
            ViewBag.GroupStudents = group.Students;

            return View(group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id, [Bind("Name,Id")] Group group, List<uint> studentIds)
        {
            //if (id != group.Name)
            //{
            //    return NotFound();
            //}
            if (studentIds.Count == 0)
            {
                TempData["ErrorMessage"] = "Ошибка: группа не может быть пустой";
                ViewBag.Students = await _studentRepository.GetAllStudentsAsync();
                ViewBag.GroupStudents = group.Students;
                return View(group);
            }
            // Проверяем на уникальность набора студентов у изменяемой группы, исключая саму группу из сравнения по её Id в БД
            if (_userRepository.DoesConsistOfSameStudents(
                studentIds,
               _context
                .Groups
                .Where(g => g.Id != group.Id)
                .Select(g => g.Students.Select(s => s.Id))))
            {
                TempData["ErrorMessage"] = "Ошибка: группа с таким набором студентов уже существует";
                ViewBag.Students = await _studentRepository.GetAllStudentsAsync();
                ViewBag.GroupStudents = group.Students;
                return View(group);
            }

            if (ModelState.IsValid)
            {
                // Получаем объекты студентов из БД на основе выбранных идентификаторов
                List<User> selectedStudents = await _context.Users
                    .Where(u => studentIds.Contains(u.Id))
                    .ToListAsync();
                //// Получаем группу по исходному имени
                //Group groupByOriginalName = await _context.Groups
                //    .SingleOrDefaultAsync(g => g.Id == id)
                //    ?? throw new NullReferenceException();

                //group.Name = group.Name.MyTrim();
                //group.Students = selectedStudents;

                //// Если у новое название группы не совпадает с прежним, то удаляем группу с прежним названием и добавляем с новым
                //if (id != group.Id)
                //{
                //    _context.Remove(groupByOriginalName);
                //    _context.Groups.Add(group);
                //}
                //// Если совпадает, то просто изменяем выбранных студентов
                //else
                //{
                //    groupByOriginalName.Students = selectedStudents;
                //}

                group.Students = selectedStudents;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    TempData["ErrorMessage"] = "Ошибка: группа с таким названием уже существует";
                    ViewBag.Students = await _studentRepository.GetAllStudentsAsync();
                    ViewBag.GroupStudents = group.Students;
                    return View(group);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Students = await _studentRepository.GetAllStudentsAsync();
            ViewBag.GroupStudents = group.Students;
            return View(group);
        }


        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(uint? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            if (_context.Groups == null)
            {
                return Problem("Entity set 'Ms2Context.Groups' is null.");
            }
            var @group = await _context.Groups.FindAsync(id);
            if (@group != null)
            {
                _context.Groups.Remove(@group);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupExists(uint id)
        {
            return (_context.Groups?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
