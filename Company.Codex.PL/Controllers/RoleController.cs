using AutoMapper;
using Company.Codex.DAL.Models;
using Company.Codex.PL.ViewModels.Role;
using Company.Codex.PL.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.Codex.PL.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleController(UserManager<ApplicationUser> userManager,
               RoleManager<IdentityRole> roleManager,
               IMapper mapper
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        // Index , Create  , Update , Delete , Edit
        //[Authorize]
        public async Task<IActionResult> Index(string searchInput)
        {
            var roles = Enumerable.Empty<RoleViewModel>();
            if (string.IsNullOrEmpty(searchInput))
            {
                roles = await _roleManager.Roles.Select(R => new RoleViewModel()
                {
                    Id = R.Id,
                    RoleName = R.Name
                }).ToListAsync();
            }
            else
            {

                roles = await _roleManager.Roles.Where(R => R.Name
                                  .ToLower()
                                  .Contains(searchInput.ToLower()))
                                  .Select(R => new RoleViewModel()
                                  {
                                      Id = R.Id,
                                      RoleName = R.Name
                                  }).ToListAsync();
            }
            return View(roles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if(ModelState.IsValid)
            {
                var role = new IdentityRole()
                {
                    Name = model.RoleName,
                };

                await _roleManager.CreateAsync(role);

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id is null) return BadRequest();
            var DBRole = await _roleManager.FindByIdAsync(id);
            if (DBRole is null) return NotFound();

            //var role = new RoleViewModel()
            //{
            //    Id = DBRole.Id,
            //    RoleName = DBRole.Name,
            //};

            var result = _mapper.Map<RoleViewModel>(DBRole); // Error Here Want To Ask The Instructor About It but in user not role 

            return View(viewName, result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string? id, RoleViewModel model)
        {
            try
            {
                if (id != model.Id) return BadRequest();

                if (ModelState.IsValid)
                {

                    var DBRole = await _roleManager.FindByIdAsync(id);
                    if (DBRole is null) return NotFound();


                    DBRole.Name = model.RoleName;


                    var flag = await _roleManager.UpdateAsync(DBRole);
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
        public async Task<IActionResult> Delete([FromRoute] string? id, RoleViewModel model)
        { 
            try
            {
                if (id != model.Id) return BadRequest();

                if (ModelState.IsValid)
                {

                    var DBRole = await _roleManager.FindByIdAsync(id);
                    if (DBRole is null) return NotFound();


                    var flag = await _roleManager.DeleteAsync(DBRole);
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
        public async Task<IActionResult> AddOrRemoveUser(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);  
            if(role is null)
            {
                return BadRequest();
            }

            ViewData["RoleId"] = roleId;

            var usersInRole = new List<UsersInRoleViewModel>();
            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var userInRole = new UsersInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,

                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userInRole.IsSelected = true;
                }
                else
                {
                    userInRole.IsSelected = false;
                }

                usersInRole.Add(userInRole);
            }
            return View(usersInRole);
        }


        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUser(string roleId,List<UsersInRoleViewModel> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) return BadRequest();

            if(ModelState.IsValid)
            {
                foreach(var user in users)
                {
                    var appuser = await _userManager.FindByIdAsync(user.UserId);

                    if(appuser is not null)
                    {
                        if (user.IsSelected && !(await _userManager.IsInRoleAsync(appuser,role.Name)))
                        {
                            await _userManager.AddToRoleAsync(appuser, role.Name);
                        }
                        else if (!user.IsSelected && await _userManager.IsInRoleAsync(appuser, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(appuser, role.Name);
                        }
                    }
                }
                return RedirectToAction(nameof(Edit), new { id= roleId});
            }
            return View(users);
        }
    }
}
