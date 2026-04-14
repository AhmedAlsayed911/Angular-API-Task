using AutoMapper;
using Madrasty.Entites;
using Madrasty_API.DTOs;
using Madrasty_API.DTOs.Student;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Madrasty_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController(Madrasty_API.UnitOfWork.UnitOfWork uof, IMapper mapper)
        : ControllerBase
    {
        [HttpGet]
        [Route("GetAllStudents")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await uof.StudentRepository.GetAllAsync();

            return Ok(mapper.Map<List<GetAllStudentsDTO>>(students));
        }

        [HttpGet]
        [Route("GetStudentById/{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await uof.StudentRepository.GetByIdAsync(id);
            if (student is null)
                return NotFound($"No Student with id {id} was found");

            return Ok(mapper.Map<GetAllStudentsDTO>(student));
        }

        [HttpPost]
        [Route("AddStudent")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> AddStudent([FromBody] AddStudentDTO studentDTO)
        {
            var checkDept = await uof.DepartmentRepository.GetByIdAsync(studentDTO.DepartmentId);
            if (checkDept is null)
                return NotFound($"No Department with id {studentDTO.DepartmentId} was found");

            if (!ModelState.IsValid)
                return BadRequest("INVALID Format");

            var student = mapper.Map<Student>(studentDTO);
            await uof.StudentRepository.AddAsync(student);
            uof.SaveChanges();

            return Ok(mapper.Map<GetAllStudentsDTO>(student));
        }

        [HttpPut]
        [Route("UpdateStudent/{id}")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] UpdateStudentDTO studentDTO)
        {
            var student = await uof.StudentRepository.GetByIdAsync(studentDTO.Id);
            if (student is null)
                return NotFound($"No Student with id {studentDTO.Id} was found");

            var checkDept = await uof.DepartmentRepository.GetByIdAsync(studentDTO.DepartmentId);
            if (checkDept is null)
                return NotFound($"No Department with id {studentDTO.DepartmentId} was found");

            if (!ModelState.IsValid || id != studentDTO.Id)
                return BadRequest("INVALID Format");

            mapper.Map(studentDTO, student);
            await uof.StudentRepository.UpdateAsync(student);
            uof.SaveChanges();

            return Ok(mapper.Map<GetAllStudentsDTO>(student));
        }

        [HttpDelete]
        [Route("DeleteStudent/{id}")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await uof.StudentRepository.GetByIdAsync(id);
            if (student is null)
                return NotFound($"No Student with id {id} was found");

            uof.StudentRepository.DeleteAsync(id);
            uof.SaveChanges();

            return Ok($"Student with id {id} has been deleted successfully");
        }

        [HttpGet]
        [Route("GetStudentsByDepartmentAndCourseId")]
        public async Task<IActionResult> GetStudentsByDepartmentAndCourseId(int departmentId, int courseId)
        {
            var students = await uof.StudentRepository.GetAllAsync();
            var studentsResult = students.Where(s => s.DepartmentId == departmentId &&
            s.StudentCourses.Any(sc => sc.CourseId == courseId)).ToList();

            if (studentsResult is null || studentsResult.Count == 0)
                return NotFound($"No Students were found for Department id {departmentId} and Course id {courseId}");

            return Ok(mapper.Map<List<GetAllStudentsDTO>>(studentsResult));
        }

        [HttpGet]
        [Route("GetStudentGrades/{id}")]
        [Authorize]
        public async Task<IActionResult> GetStudentGrades(int id)
        {
            var student = await uof.StudentRepository.GetByIdAsync(id);
            if (student is null)
                return NotFound($"No Student with id {id} was found");

            return Ok(mapper.Map<List<StudentGradeDTO>>(student.StudentCourses));
        }

        [HttpPut]
        [Route("UpdateStudentGrade")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> UpdateStudentGrade([FromBody] UpdateCourseGradeDTO dto)
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
