using MusicSchoolAsp.Models.Db;

namespace MusicSchoolAsp.Models.ViewModels
{
    public class GroupTaskAssignmentViewModel
    {
        public Node Task { get; set; } = null!;
        public List<GroupCheckBoxViewModel> Groups { get; set; } = null!;
    }
}
