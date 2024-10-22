using System.ComponentModel.DataAnnotations;

namespace Company.Codex.PL.ViewModels.Auth
{
	public class ResetPasswordViewModel
	{
		[Required(ErrorMessage = "Password is Required !!")]
		[DataType(DataType.Password)]
		[MinLength(5, ErrorMessage = "Password Min Length is 5")]
		public string Password { get; set; }
		[Required(ErrorMessage = "ConfirmPassword is Required !!")]
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Confirmed Password Does not Match The Password !!")]
		public string ConfirmPassword { get; set; }
	}
}
