using System.ComponentModel.DataAnnotations;

namespace MusicSchoolAsp.Models.ViewModels
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Укажите логин")]
		[MinLength(1, ErrorMessage = "Логин должен иметь длину не менее 1 символов")]
		[MaxLength(255, ErrorMessage = "Логин должен иметь длину не более 255 символов")]
		[Display(Name = "Логин")]
		public string Login { get; set; } = null!;

		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Укажите пароль")]
		[MinLength(1, ErrorMessage = "Пароль должен иметь длину не менее 1 символов")]
		[MaxLength(255, ErrorMessage = "Пароль должен иметь длину не более 255 символов")]
		[Display(Name = "Пароль")]
		public string Password { get; set; } = null!;
	}
}
