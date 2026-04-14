using AutoMapper;
using Madrasty.Entites;
using Madrasty_API.DTOs;
using Madrasty_API.DTOs.Course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Madrasty_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController(Madrasty_API.UnitOfWork.UnitOfWork uof, IMapper mapper)
        : ControllerBase
    {
        [HttpGet]
        [Route("GetAllCourses")]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await uof.CourseRepository.GetAllAsync();
            return Ok(mapper.Map<List<GetAllCoursesDTO>>(courses));
        }

        [HttpGet]
        [Route("GetCourseById/{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            var course = await uof.CourseRepository.GetByIdAsync(id);
            if (course is null)
                return NotFound($"No Course with id {id} was found");

            return Ok(mapper.Map<GetAllCoursesDTO>(course));
        }

        [HttpPost]
        [Route("AddCourse")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> AddCourse([FromBody] AddCourseDTO courseDTO)
        {
            var checkTopic = await uof.TopicRepository.GetByIdAsync(courseDTO.TopicId);
            if (checkTopic is null)
                return NotFound($"No Topic with id {courseDTO.TopicId} was found");

            if (!ModelState.IsValid)
                return BadRequest("INVALID Format");

            var course = mapper.Map<Course>(courseDTO);
            await uof.CourseRepository.AddAsync(course);
            uof.SaveChanges();

            return Ok(mapper.Map<GetAllCoursesDTO>(course));
        }

        [HttpPut]
        [Route("UpdateCourse/{id}")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] UpdateCourseDTO courseDTO)
        {
            var course = await uof.CourseRepository.GetByIdAsync(courseDTO.Id);
            if (course is null)
                return NotFound($"No Course with id {courseDTO.Id} was found");

            var checkTopic = await uof.TopicRepository.GetByIdAsync(courseDTO.TopicId);
            if (checkTopic is null)
                return NotFound($"No Topic with id {courseDTO.TopicId} was found");

            if (!ModelState.IsValid || id != courseDTO.Id)
                return BadRequest("INVALID Format");

            mapper.Map(courseDTO, course);
            await uof.CourseRepository.UpdateAsync(course);
            uof.SaveChanges();

            return Ok(mapper.Map<GetAllCoursesDTO>(course));
        }

        [HttpDelete]
        [Route("DeleteCourse/{id}")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await uof.CourseRepository.GetByIdAsync(id);
            if (course is null)
                return NotFound($"No Course with id {id} was found");

            uof.CourseRepository.DeleteAsync(id);
            uof.SaveChanges();

            return Ok($"Course with id {id} has been deleted successfully");
        }

        [HttpPost]
        [Route("AddCourseToDepartment")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> AddCourseToDepartment(int id, int courseId)
        {
            var course = await uof.CourseRepository.GetByIdAsync(courseId);
            if (course is null)
                return NotFound($"No Course with id {courseId} was found");

            var department = await uof.DepartmentRepository.GetByIdAsync(id);
            if (department is null)
                return NotFound($"No Department with id {id} was found");

            if (department.Courses.Any(x => x.Name.ToLower() == course.Name.ToLower()))
                return BadRequest($"Course with name {course.Name} already exists in Department with id {id}");

            department.Courses.Add(course);
            await uof.DepartmentRepository.UpdateAsync(department);
            uof.SaveChanges();

            return Ok($"Course with id {courseId} has been added to Department with id {id} successfully");
        }

        [HttpDelete]
        [Route("DeleteCourseFromDepartment")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> DeleteCourseFromDepartment(int id, int courseId)
        {
            var course = await uof.CourseRepository.GetByIdAsync(courseId);
            if (course is null)
                return NotFound($"No Course with id {courseId} was found");

            var department = await uof.DepartmentRepository.GetByIdAsync(id);
            if (department is null)
                return NotFound($"No Department with id {id} was found");

            if (!department.Courses.Any(x => x.Name.ToLower() == course.Name.ToLower()))
                return BadRequest($"Course with name {course.Name} does not exist in Department with id {id}");

            department.Courses.Remove(course);
            await uof.DepartmentRepository.UpdateAsync(department);
            uof.SaveChanges();

            return Ok($"Course with id {courseId} has been removed from Department with id {id} successfully");
        }

        [HttpGet]
        [Route("GetCoursesByDepartmentId")]
        public async Task<IActionResult> GetCoursesByDepartmentId(int deptId)
        {
            var department = await uof.DepartmentRepository.GetByIdAsync(deptId);
            if (department is null)
                return NotFound($"No Department with id {deptId} was found");
            var courses = department.Courses;
            return Ok(mapper.Map<List<GetAllCoursesDTO>>(courses));
        }

        [HttpPost]
        [Route("AddCourseToStudent")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> AddCourseToStudent(int courseId, int studentId)
        {
            var course = await uof.CourseRepository.GetByIdAsync(courseId);
            if (course is null)
                return NotFound($"No Course with id {courseId} was found");

            var student = await uof.StudentRepository.GetByIdAsync(studentId);
            if (student is null)
                return NotFound($"No Student with id {studentId} was found");

            if (student.StudentCourses.Any(x => x.CourseId == courseId))
                return BadRequest($"Course with id {courseId} already exists for Student with id {studentId}");
            student.StudentCourses.Add(new StudentCourse { CourseId = courseId, StudentId = studentId });
            await uof.StudentRepository.UpdateAsync(student);

            uof.SaveChanges();


            return Ok($"Course with id {courseId} has been added to Student with id {studentId} successfully");
        }

        [HttpPut]
        [Route("UpdateCourseGrade")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> UpdateCourseGrade(UpdateCourseGradeDTO dto)
        {
            var course = await uof.CourseRepository.GetByIdAsync(dto.CourseId);
            if (course is null)
                return NotFound($"No Course with id {dto.CourseId} was found");

            var student = await uof.StudentRepository.GetByIdAsync(dto.StudentId);
            if (student is null)
                return NotFound($"No Student with id {dto.StudentId} was found");

            var studentCourse = student.StudentCourses.FirstOrDefault(x => x.CourseId == dto.CourseId);
            if (studentCourse is null)
                return BadRequest($"Course with id {dto.CourseId} does not exist for Student with id {dto.StudentId}");

            studentCourse.Grade = dto.Grade;
            await uof.StudentRepository.UpdateAsync(student);
            uof.SaveChanges();


            return Ok($"Grade for Course with id {dto.CourseId} has been updated to {dto.Grade} for Student with id {dto.StudentId} Successfully");
        }
    }
}
