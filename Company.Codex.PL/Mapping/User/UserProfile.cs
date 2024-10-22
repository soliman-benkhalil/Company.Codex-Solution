using AutoMapper;
using Company.Codex.DAL.Models;
using Company.Codex.PL.ViewModels.EmployeeViews;
using Company.Codex.PL.ViewModels.Users;

namespace Company.Codex.PL.Mapping.User
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserViewModel, ApplicationUser>().ReverseMap();
        }
    }
}
