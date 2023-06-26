using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MusicSchoolEF.Models.ViewModels
{
	public class StudentTaskEditViewModel
	{
		[ReadOnly(true)]
		public uint StudentId { get; set; }

		[ReadOnly(true)]
		public uint TaskId { get; set; }

		[ReadOnly(true)]
		[Display(Name = "Название")]
		public string Name { get; set; } = null!;

		[ReadOnly(true)]
		[Display(Name = "Описание")]
		public string? Description { get; set; }

		[Display(Name = "Оценка")]
		public int? Mark { get; set; }

		[MaxLength(5000, ErrorMessage = "Длина комментария не может превышать 5000 символов")]
		[Display(Name = "Комментарий")]
		public string? Comment { get; set; }
	}
}
