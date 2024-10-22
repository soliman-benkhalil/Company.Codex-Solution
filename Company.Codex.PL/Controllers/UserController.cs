
using AutoMapper;
using Company.Codex.DAL.Models;
using Company.Codex.PL.Helpers;
using Company.Codex.PL.ViewModels.EmployeeViews;
using Company.Codex.PL.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.Codex.PL.Controllers
{
    [Authorize(Roles ="Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager,
               RoleManager<IdentityRole> roleManager,
               IMapper mapper
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

		// Get , GetAll , Add , Update , Delete
		public async Task<IActionResult> Index(string searchInput)
		{
			var users = Enumerable.Empty<UserViewModel>();

			// Fetch users without roles first
			var userList = await _userManager.Users
				.Where(U => string.IsNullOrEmpty(searchInput) || U.Email.ToLower().Contains(searchInput.ToLower()))
				.Select(U => new UserViewModel
				{
					Id = U.Id,
					FirstName = U.FirstName,
					LastName = U.LastName,
					Email = U.Email
				})
				.ToListAsync();

			// Fetch roles for each user asynchronously
			foreach (var user in userList)
			{
				user.Roles = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(user.Id));
			}

			return View(userList);
		}




		public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id is null) return BadRequest();
            var DbUser = await _userManager.FindByIdAsync(id);
            if (DbUser is null) return NotFound();

            //var user = new UserViewModel()
            //{
            //    Id = DbUser.Id,
            //    FirstName = DbUser.FirstName,
            //    LastName = DbUser.LastName,
            //    Email = DbUser.Email,
                
            //    Roles = _userManager.GetRolesAsync(DbUser).Result,
               
                
            //};

            var result = _mapper.Map<UserViewModel>(DbUser); // Error Here Want To Ask The Instructor About It 

            return View(viewName, result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string? id, UserViewModel model)
        {
            try
            {
                if (id != model.Id) return BadRequest();

                if (ModelState.IsValid)
                {

                    var DBuser = await _userManager.FindByIdAsync(id);
                    if (DBuser is null) return NotFound();


                    //var employee = _mapper.Map<Employee>(model);

                    DBuser.FirstName = model.FirstName;
                    DBuser.LastName = model.LastName;
                    DBuser.Email = model.Email;


                    var flag = await _userManager.UpdateAsync(DBuser);
                    if (flag.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string? id, UserViewModel model)
        {
            try
            {

                if (id != model.Id) return BadRequest();

                if (ModelState.IsValid)
                {

                    var DBuser = await _userManager.FindByIdAsync(id);
                    if (DBuser is null) return NotFound();

                    var flag = await _userManager.DeleteAsync(DBuser);
                    if (flag.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(model);
        }
    }
}
