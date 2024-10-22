using AutoMapper;
using Company.Codex.DAL.Models;
using Company.Codex.PL.ViewModels.DepartmentViews;

namespace Company.Codex.PL.Mapping
{
    public class DepartmentProfile : Profile 
    {
        public DepartmentProfile()
        {
            CreateMap<DepartmentViewModel, Department>().ReverseMap();
        }
    }
}
