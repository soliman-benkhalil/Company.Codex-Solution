using System.ComponentModel.DataAnnotations;

namespace Company.Codex.PL.ViewModels.Auth
{
	public class SignInViewModel
	{
		[Required(ErrorMessage = "Email is Required !!")]
		[EmailAddress(ErrorMessage = "Invalid Email !!")]
		public string Email { get; set; }
		[Required(ErrorMessage = "Password is Required !!")]
		[DataType(DataType.Password)]
		[MinLength(5, ErrorMessage = "Password Min Length is 5")]
		public string Password { get; set; }

        public bool RememberMe { get; set; }

    }
}
