using AutoMapper;
using Madrasty.Entites;
using Madrasty_API.DTOs.Course;

namespace Madrasty_API.Profiles
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, GetAllCoursesDTO>()
                .ForMember(dest => dest.TopicName, opt => opt.MapFrom(src => src.Topic.Name));

            CreateMap<AddCourseDTO, Course>();
            CreateMap<UpdateCourseDTO, Course>();
        }
    }
}
