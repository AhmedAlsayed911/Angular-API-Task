using AutoMapper;
using Madrasty.Entites;
using Madrasty_API.DTOs.Department;

namespace Madrasty_API.Profiles
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<Department, GetAllDepartmentsDTO>();
            CreateMap<AddDepartmentDTO, Department>();
            CreateMap<UpdateDepartmentDTO, Department>();
        }
    }
}
