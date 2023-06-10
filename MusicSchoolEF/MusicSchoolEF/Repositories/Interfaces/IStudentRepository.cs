using MusicSchoolEF.Models.Db;

namespace MusicSchoolEF.Repositories.Interfaces
{
	public interface IStudentRepository
	{
		Task<List<User>> GetStudentsAssignedTasksByTeacherAsync(uint teacherId);
		Task<List<User>> GetAllStudentsAsync();
	}
}
