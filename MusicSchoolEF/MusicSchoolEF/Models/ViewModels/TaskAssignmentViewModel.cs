using MusicSchoolEF.Models.Db;

namespace MusicSchoolEF.Models.ViewModels
{
    public enum CheckedState
    {
        Checked,
        Unchecked,
        Intermediate
    }

    public class TaskAssignmentViewModel
    {
        public Node Task { get; set; } = null!;
        public List<StudentViewModel> Students { get; set; } = null!;
        //public List<GroupViewModel> Groups { get; set; } = null!;
    }

    public class StudentViewModel
    {
        public uint Id { get; set; }
        public string Name { get; set; } = null!;
        //public CheckedState CheckedState { get; set; }
        public bool IsChecked { get; set; }
    }

    //public class GroupViewModel
    //{
    //    public string Name { get; set; } = null!;
    //    public CheckedState CheckedState { get; set; }
    //}
}
