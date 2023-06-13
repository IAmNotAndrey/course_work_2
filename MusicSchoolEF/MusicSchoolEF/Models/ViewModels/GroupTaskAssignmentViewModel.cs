using MusicSchoolEF.Models.Db;

namespace MusicSchoolEF.Models.ViewModels
{
    public class GroupTaskAssignmentViewModel
    {
        public Node Task { get; set; } = null!;
        public List<GroupCheckBoxViewModel> Groups { get; set; } = null!;
    }
}
