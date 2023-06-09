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
		public string Name { get; set; } = null!;

		[ReadOnly(true)]
		public string? Description { get; set; }

		public int? Mark { get; set; }

		[MaxLength(5000, ErrorMessage = "Длина комментария не может превышать 5000 символов")]
		public string? Comment { get; set; }
	}
}
