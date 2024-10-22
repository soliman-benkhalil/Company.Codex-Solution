using AutoMapper;
using Company.Codex.BLL.Interfaces;
using Company.Codex.BLL.Repositories;
using Company.Codex.DAL.Models;
using Company.Codex.PL.ViewModels.DepartmentViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.Codex.PL.Controllers
{
    [Authorize]
    public class DepartmentsController : Controller
    {
        private readonly IMapper _mapper;

        private readonly IUnitOfWork _unitOfWork;

        public DepartmentsController(
            IMapper mapper,
            IUnitOfWork unitOfWork)  // Ask CLR To Create Object From DepartmentRepository

        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        //[HttpGet,HttpPost]
        public async Task<IActionResult> Index(string searchInput)
        {
            var departments = Enumerable.Empty<Department>();
            if (string.IsNullOrEmpty(searchInput))
            {
                departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            }
            else
            {
                departments = await _unitOfWork.DepartmentRepository.GetByNameAsync<Department>(searchInput);
            }

            var result = _mapper.Map<IEnumerable<DepartmentViewModel>>(departments); // Auto Mapping

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var department = _mapper.Map<Department>(model);  // Auto Mapping
                var count = await _unitOfWork.DepartmentRepository.AddAsync(department);
                if (count > 0)
                {
                    TempData["Message"] = "Department is Created Sucessfully";
                }
                else
                {
                    TempData["Message"] = "Department is Not Created Sucessfully";
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Details(int? id,string viewName = "Details")
        {
            if (id is null) return BadRequest(); // 400

            var department = await _unitOfWork.DepartmentRepository.GetAsync(id.Value); // Value Here Rerurns The Value In The Nullable Variable

            if (department is null) return NotFound(); // 404

            var result = _mapper.Map<DepartmentViewModel>(department);

            return View(viewName, result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Used With Action That Is Post And Used To Prevent Any External Tool From Send A Request Like Postman
        public async Task<IActionResult> Edit([FromRoute]int? id, DepartmentViewModel model) //  [FromForm]Department model -> Error Because Here We Force It To Get Only From The Form And If It Is Not Existed Will Consider It As Default Value (0 For Id)
        {
            try
            {
                if (id != model.Id) return BadRequest(); // To Check If An Error Through Inspect

                if (ModelState.IsValid)
                {
                    var department = _mapper.Map<Department>(model);

                    var count = await _unitOfWork.DepartmentRepository.UpdateAsync(department);
                    if (count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception Ex)
            {

                ModelState.AddModelError(string.Empty, Ex.Message);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Delete([FromRoute] int? id, DepartmentViewModel model) 
        {
            try
            {
                if (id != model.Id) return BadRequest(); 

                if (ModelState.IsValid)
                {
                    var department = _mapper.Map<Department>(model);

                    var count = await _unitOfWork.DepartmentRepository.DeleteAsync(department);
                    if (count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception Ex)
            {

                ModelState.AddModelError(string.Empty, Ex.Message);
            }

            return View(model);
        }
    }
}
