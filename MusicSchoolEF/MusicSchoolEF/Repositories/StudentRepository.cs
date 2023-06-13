using Microsoft.EntityFrameworkCore;
using MusicSchoolEF.Models.Db;
using MusicSchoolEF.Models.Defaults;
using MusicSchoolEF.Repositories.Interfaces;

namespace MusicSchoolEF.Repositories
{
	public class StudentRepository : IStudentRepository
	{
		private readonly Ms2Context _dbContext;

		public StudentRepository(Ms2Context dbContext)
		{
			_dbContext = dbContext;
		}

		/// <summary>
		///  Поиск всех учеников, которым заданы задания, создателем которых является учитель с `teacherId`
		/// </summary
		public async Task<List<User>> GetStudentsAssignedTasksByTeacherAsync(uint teacherId)
		{
			return await _dbContext.Users
				//.Include(s => s.StudentNodeConnections)
				//.ThenInclude(snc => snc.NodeNavigation)
				// Ищем, привязано ли к ученику хотя бы одно задание, создаталем которого является учитель
				.Where(s => s.StudentNodeConnections.Any(snc => snc.NodeNavigation.Owner == teacherId))
				.ToListAsync();
		}

		public async Task<List<User>> GetAllStudentsAsync()
		{
			return await _dbContext.Users
				//.Include(u => u.StudentNodeConnections)
				//.ThenInclude(snc => snc.NodeNavigation)
				.Where(u => u.Role == Roles.Student)
				.ToListAsync();
		}
	}
}
