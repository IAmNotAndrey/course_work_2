using MusicSchoolEF.Models.Db;
using MusicSchoolEF.Models.ViewModels;

namespace MusicSchoolEF.Repositories.Interfaces
{
	public interface IStudentNodeConnectionRepository
	{
		Task<List<StudentNodeConnection>> GetStudentNodeConnectionsByStudentIdAsync(uint studentId);
		Task<List<StudentNodeConnection>> GetStudentNodeConnectionsByStudentIdAndTeacherIdAsync(uint studentId, uint teacherId);
		Task<StudentNodeConnection?> GetStudentNodeConnectionByPrimaryKey(uint studentId, uint nodeId);
		Task EditAsync(StudentNodeConnection editingSnc, int? mark, string? comment);
		Task AddAsync(StudentNodeConnection studentNodeConnection);
		Task RemoveAsync(StudentNodeConnection studentNodeConnection);
		Task UpdateTable(List<StudentCheckBoxViewModel> students, List<Node> nodeAndDescendants);
	}
}
