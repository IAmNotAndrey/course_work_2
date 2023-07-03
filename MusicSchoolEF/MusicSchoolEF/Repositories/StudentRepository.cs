using Microsoft.EntityFrameworkCore;
using MusicSchoolAsp.Models.Db;
using MusicSchoolAsp.Models.Defaults;
using MusicSchoolAsp.Repositories.Interfaces;

namespace MusicSchoolAsp.Repositories
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
				// Ищем, привязано ли к ученику хотя бы одно задание, создателем которого является учитель
				.Where(s => s.StudentNodeConnections.Any(snc => snc.Node.Owner == teacherId))
				.ToListAsync();
		}

		public async Task<List<User>> GetAllStudentsAsync()
		{
			return await _dbContext.Users
				//.Include(u => u.StudentNodeConnections)
				//.ThenInclude(snc => snc.NodeNavigation)
				.Where(u => u.Role.Name == Roles.Student)
				.ToListAsync();
		}
	}
}
