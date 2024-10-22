using Company.Codex.DAL.Models;
using Company.Codex.PL.Helpers;
using Company.Codex.PL.ViewModels.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.ObjectModelRemoting;

namespace Company.Codex.PL.Controllers
{
	public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		#region SignUp
		[HttpGet]
		public IActionResult SignUp()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SignUp(SignUpViewModel model)
		{
			// Code ot Registiration
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByNameAsync(model.UserName);

				if (user is null)
				{
					// Mapping : SignUpViewModel To ApplicationUser 

					user = await _userManager.FindByEmailAsync(model.Email);
					if (user is null)
					{
						user = new ApplicationUser()
						{
							UserName = model.UserName,
							Email = model.Email,
							FirstName = model.FirstName,
							LastName = model.LastName,
							IsAgree = model.IsAgree,
						};

						var result = await _userManager.CreateAsync(user, model.Password);

						if (result.Succeeded)
						{
							return RedirectToAction("SignIn");
						}

						foreach (var error in result.Errors)
						{
							ModelState.AddModelError(string.Empty, error.Description);
						}
					}
					ModelState.AddModelError(string.Empty, "Email Is Already Exisited ^^");
				}
				ModelState.AddModelError(string.Empty, "UserName Is Already Exisited ^^");
			}
			return View(model);
		}
		#endregion

		#region LogIn
		[HttpGet]
		public IActionResult SignIn()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SignIn(SignInViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var user = await _userManager.FindByEmailAsync(model.Email);

					if (user is not null)
					{
						var flag = await _userManager.CheckPasswordAsync(user, model.Password);

						if (flag)
						{
							// SignIn

							var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
							// If model.rememberMe is True It means That the Data of The Userin Login Will Be Saved For A Duration in Cookies Of The Web
							// PasswordSignInAsync Here Made The User To Have Token And It Allows The User To LogIn And Save It In The browser And Then Back with any request later

							if (result.Succeeded)
							{
								return RedirectToAction(nameof(HomeController.Index), "Home");
							}
						}
					}
					ModelState.AddModelError(string.Empty, "Invalid LogIn");
				}
				catch (Exception ex)
				{

					ModelState.AddModelError(string.Empty, ex.Message);
				}
			}
			return View(model);
		}
		#endregion

		#region SignOut
		public new async Task<IActionResult> SignOut() // Warning here Because there is a built-in function from controller base
		{
			await _signInManager.SignOutAsync();

			return RedirectToAction(nameof(SignIn));
		}
		#endregion

		#region ForgetPassword
		[HttpGet]
		public IActionResult ForgetPassword()
		{
			return View();
		}
		#endregion

		#region ResetPassword
		[HttpPost]
		public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);

				if (user is not null)
				{
					// Generate Token

					var token = await _userManager.GeneratePasswordResetTokenAsync(user);

					// Create Reset Password URL
					var url = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme); // Schema Is The Schema of the url that is saved in json file and eahc request it is sent
																																  // Reset Password & Reset Email tokens are speical ones and need to make register for a serivce AddDefaultTokenProviders AddDefaultTokenProviders
																																  //https://localhost:7254;http://localhost:5087/Account/ResetPassword?email=soliman12@gmail.com&token	

					// create email
					var email = new Email()
					{
						To = model.Email,
						Subject = "Reset Password",
						Body = url
					};
					// send email

					EmailSettings.SendEmail(email);

					return RedirectToAction(nameof(CheckYourInbox));


				}
				ModelState.AddModelError(string.Empty, "Invalid Reset Password, Please Try Again !");
			}
			return View(model);
		}

		[HttpGet]
		public IActionResult CheckYourInbox()
		{
			return View();
		}


		[HttpGet]
		public IActionResult ResetPassword(string email, string token)
		{
			TempData["email"] = email;
			TempData["token"] = token;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var email = TempData["email"] as string;
				var token = TempData["token"] as string;

				var user = await _userManager.FindByEmailAsync(email);
				if (user is not null)
				{
					var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
					if (result.Succeeded)
					{
						return RedirectToAction(nameof(SignIn));
					}
				}
			}
			ModelState.AddModelError(string.Empty, "Invalid Operation , Please Try Again");
			return View(model);

		} 
		#endregion

		public IActionResult AccessDenied()
		{
			return View();
		}
	}
}
