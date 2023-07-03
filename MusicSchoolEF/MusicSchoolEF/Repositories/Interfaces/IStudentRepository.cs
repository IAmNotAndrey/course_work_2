using MusicSchoolAsp.Models.Db;

namespace MusicSchoolAsp.Repositories.Interfaces
{
	public interface IStudentRepository
	{
		Task<List<User>> GetStudentsAssignedTasksByTeacherAsync(uint teacherId);
		Task<List<User>> GetAllStudentsAsync();
	}
}
