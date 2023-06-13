using MusicSchoolEF.Models.Db;

namespace MusicSchoolEF.Models.ViewModels
{
	public class StudentTaskAssignmentViewModel
	{
		public Node Task { get; set; } = null!;
		public List<StudentCheckBoxViewModel> Students { get; set; } = null!;
	}
}
