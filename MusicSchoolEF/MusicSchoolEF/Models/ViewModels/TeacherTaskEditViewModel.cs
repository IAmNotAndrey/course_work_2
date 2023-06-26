using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MusicSchoolEF.Models.ViewModels
{
	public class TeacherTaskEditViewModel
	{
		[ReadOnly(true)]
		public uint TaskId { get; set; }

		[Required(ErrorMessage = "Укажите название")]
		[MinLength(2, ErrorMessage = "Название должно иметь длину не менее 2 символов")]
		[MaxLength(255, ErrorMessage = "Название должно иметь длину не более 255 символов")]
		[Display(Name = "Название")]
		public string Name { get; set; } = null!;

		[MaxLength(5000, ErrorMessage = "Описание должно иметь длину не более 5000 символов")]
		[Display(Name = "Описание")]
		public string? Description { get; set; }

		//[ReadOnly(true)]
		[DefaultValue(0)]
		[Display(Name = "Приоритет")]
		public uint Priority { get; set; }
	}
}
