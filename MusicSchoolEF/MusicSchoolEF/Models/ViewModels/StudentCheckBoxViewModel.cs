using MusicSchoolAsp.Models.Db;
using System.Collections.Generic;

namespace MusicSchoolAsp.Models.ViewModels
{
    public class StudentCheckBoxViewModel
    {
        public uint Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsChecked { get; set; }

        public static List<StudentCheckBoxViewModel> GetStudentCheckBoxListByGroups(List<GroupCheckBoxViewModel> groupModels)
        {
            List<StudentCheckBoxViewModel> students = new();

            foreach (GroupCheckBoxViewModel gm in groupModels)
            {
                if (gm.State == 0)
                    continue;

                foreach (uint studentId in gm.StudentIds)
                {
                    bool settingState = gm.State == 1;
					int index = students.FindIndex(s => s.Id == studentId);
                    // Если уже есть в `students`
                    if (index != -1)
                    {
						StudentCheckBoxViewModel existingStudentModel = students[index];
						// note Если есть противоречия в установке `IsChecked`, то ставим true
                        if (existingStudentModel.IsChecked || settingState)
							students[index].IsChecked = true;
						continue;
                    }
                    // Если ещё нет в `students`
                    students.Add(new StudentCheckBoxViewModel()
                    {
                        Id = studentId,
                        Name = String.Empty,
                        IsChecked = settingState
					});
				}
			}

            return students;
		}
	}
}
