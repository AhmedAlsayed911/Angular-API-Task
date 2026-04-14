using AutoMapper;
using Madrasty.Entites;
using Madrasty_API.DTOs.Student;

namespace Madrasty_API.Profiles
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<Student, GetAllStudentsDTO>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dest => dest.Grades, opt => opt.MapFrom(src => src.StudentCourses));

            CreateMap<StudentCourse, StudentGradeDTO>()
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name));

            CreateMap<AddStudentDTO, Student>();
            CreateMap<UpdateStudentDTO, Student>();
        }
    }
}
