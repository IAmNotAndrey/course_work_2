using MusicSchoolAsp.Models.Db;
using System.ComponentModel.DataAnnotations;

namespace MusicSchoolAsp.Models.ViewModels
{
    public class GroupCheckBoxViewModel
    {
        //public Group Group { get; set; } = null!;
        [Display(Name = "Название")]
        public string Name { get; set; } = null!;
        public int State { get; set; } // -1: false, 0: intermediate, 1: true
        public List<uint> StudentIds { get; set; } = new List<uint>();
        //public bool IsChecked { get; set; } // -1: false, 0: intermediate, 1: true
    }
}
