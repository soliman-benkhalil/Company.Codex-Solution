using AutoMapper;
using Company.Codex.DAL.Models;
using Company.Codex.PL.ViewModels.EmployeeViews;

namespace Company.Codex.PL.Mapping
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
            //CreateMap<Employee, EmployeeViewModel>();
            // Mapp Between The Attibute That Has The Same Name And DataType 


        }
    }
}
