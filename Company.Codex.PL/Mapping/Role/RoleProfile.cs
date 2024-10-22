using AutoMapper;
using Company.Codex.DAL.Models;
using Company.Codex.PL.ViewModels.Role;
using Company.Codex.PL.ViewModels.Users;
using Microsoft.AspNetCore.Identity;

namespace Company.Codex.PL.Mapping.Role
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleViewModel, IdentityRole>()
                .ReverseMap().ForMember(d => d.RoleName, options => options.MapFrom(s => s.Name));
        }
    }
}
