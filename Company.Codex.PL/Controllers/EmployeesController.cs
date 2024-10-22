using AutoMapper;
using Company.Codex.BLL.Interfaces;
using Company.Codex.DAL.Models;
using Company.Codex.PL.Helpers;
using Company.Codex.PL.ViewModels.EmployeeViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace Company.Codex.PL.Controllers
{
	[Authorize]
	public class EmployeesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public EmployeesController(

            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //[HttpGet,HttpPost]
        public async Task<IActionResult> Index(string searchInput)
         {
            var employees = Enumerable.Empty<Employee>();
            if(string.IsNullOrEmpty(searchInput))
            {
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            else
            {
                employees = await _unitOfWork.EmployeeRepository.GetByNameAsync<Employee>(searchInput);
            }

            // Auto Mapping 
            var result = _mapper.Map<IEnumerable<EmployeeViewModel>>(employees);   // <distination>(The Base Class)

            //var employees = _employeeRepository.GetAll();
            //// View Dicttionary : [Extra Infromation] Transfer data from action to view [one way]
            //// 1. ViewData -> property u inherited it from class controller  -> Dictionary
            //string Message = "Hello World";

            //ViewData["Message"] = Message + " From View Data"; // returns datatype of object
            //// 2. ViewBag  -> property u inherited it from class controller  -> dynamic 

            //ViewBag.lol = Message + " From View Bag"; // Override Here and to make 2 variables change viewbag.Message

            //// 3. TempData -> property u inherited it from class controller  -> Dictionary
            //// For Transfering Data Between 2 Requests 

            //TempData["Message01"] = Message + " From Temp Data";

            ////ViewData["Message"] = Message + " From View Data";  

            //// The Difference Between 1 and 2 is Syntax 
            
            //// 3 The 2 Requests Must Be Sequential .. One After The Other Directly 


            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var department = await _unitOfWork.DepartmentRepository.GetAllAsync(); // Extra Information 
            // View's Dictionary 

            ViewData["Departments"] = department;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {

            if(ModelState.IsValid)
            {
                if(model.Image is not null)
                 model.ImageName = DocumentSettings.UploadFile(model.Image, "images");

                // Casting : EmployeeViewModel  ->  Employee   

                // Manual Mapping 

                //Employee employee = new Employee()
                //{
                //    Id = model.Id,
                //    Name = model.Name,
                //    Age = model.Age,
                //    Address = model.Address,
                //    Salary = model.Salary,
                //    PhoneNumber = model.PhoneNumber,
                //    Email = model.Email,
                //    IsActive = model.IsActive,
                //    IsDeleted = model.IsDeleted,
                //    DateOfCreation = model.DateOfCreation,
                //    HiringDate = model.HiringDate,
                //    WorkForId = model.WorkForId,
                //    WorkFor = model.WorkFor,
                //};

                // Auto Mapping 
                var employee = _mapper.Map<Employee>(model);
                var count = await _unitOfWork.EmployeeRepository.AddAsync(employee);
                if(count > 0)
                {
                    TempData["Message"] = "Employee is Created Sucessfully";
                }
                else
                {
                    TempData["Message"] = "Employee is Not Created Sucessfully";
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Details(int? id,string viewName="Details")
        {
            if (id is null) return BadRequest();
            var employees = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            if(employees is null) return NotFound();

            var result = _mapper.Map<EmployeeViewModel>(employees);

            return View(viewName, result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var department = await _unitOfWork.DepartmentRepository.GetAllAsync(); // Extra Information 
            // View's Dictionary 

            ViewData["Departments"] = department;



            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int? id ,EmployeeViewModel model)
        { 
            try
            {
                if (id != model.Id) return BadRequest();

                if(ModelState.IsValid)
                {


                    //Employee employee = new Employee()
                    //{
                    //    Id = model.Id,
                    //    Name = model.Name,
                    //    Age = model.Age,
                    //    Address = model.Address,
                    //    Salary = model.Salary,
                    //    PhoneNumber = model.PhoneNumber,
                    //    Email = model.Email,
                    //    IsActive = model.IsActive,
                    //    IsDeleted = model.IsDeleted,
                    //    DateOfCreation = model.DateOfCreation,
                    //    HiringDate = model.HiringDate,
                    //    WorkForId = model.WorkForId,
                    //    WorkFor = model.WorkFor,
                    //};

                    // Auto Mapping 


                    //var employeeInDb = await _unitOfWork.EmployeeRepository.GetAsync(model.Id);

                    if (model.ImageName is not null && model.Image is not null)
                    {
                        DocumentSettings.DeleteFile(model.ImageName, "images");
                    }

                    if(model.Image is not null)
                    {
                        model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                    }

                    var employee = _mapper.Map<Employee>(model);
                    var count = await _unitOfWork.EmployeeRepository.UpdateAsync(employee);
                    if(count > 0)
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
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int? id,EmployeeViewModel model)
        {
            try
            {

                if (id != model.Id) return BadRequest();

                if (ModelState.IsValid)
                {
                    // Manual Mapping
                    //Employee employee = new Employee()
                    //{
                    //    Id = model.Id,
                    //    Name = model.Name,
                    //    Age = model.Age,
                    //    Address = model.Address,
                    //    Salary = model.Salary,
                    //    PhoneNumber = model.PhoneNumber,
                    //    Email = model.Email,
                    //    IsActive = model.IsActive,
                    //    IsDeleted = model.IsDeleted,
                    //    DateOfCreation = model.DateOfCreation,
                    //    HiringDate = model.HiringDate,
                    //    WorkForId = model.WorkForId,
                    //    WorkFor = model.WorkFor,
                    //};

                    // Auto Mapping 
                    var employee = _mapper.Map<Employee>(model);
                    var count = await _unitOfWork.EmployeeRepository.DeleteAsync(employee); 
                    if (count > 0)
                    {
                       
                        
                            if(model.ImageName is not null)
                               DocumentSettings.DeleteFile(model.ImageName, "images");
                        
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["Error"] = "Failed to delete employee.";
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
