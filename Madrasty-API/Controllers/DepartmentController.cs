using AutoMapper;
using Madrasty.Entites;
using Madrasty_API.DTOs.Department;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Madrasty_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController(Madrasty_API.UnitOfWork.UnitOfWork uof, IMapper mapper)
        : ControllerBase
    {
        [HttpGet]
        [Route("GetAllDepartments")]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments = await uof.DepartmentRepository.GetAllAsync();
            return Ok(mapper.Map<List<GetAllDepartmentsDTO>>(departments));
        }

        [HttpGet]
        [Route("GetDepartmentById/{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var department = await uof.DepartmentRepository.GetByIdAsync(id);
            if (department is null)
                return NotFound($"No Department with id {id} was found");

            return Ok(mapper.Map<GetAllDepartmentsDTO>(department));
        }

        [HttpPost]
        [Route("AddDepartment")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> AddDepartment([FromBody] AddDepartmentDTO departmentDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest("INVALID Format");

            var department = mapper.Map<Department>(departmentDTO);
            await uof.DepartmentRepository.AddAsync(department);
            uof.SaveChanges();

            return Ok(mapper.Map<GetAllDepartmentsDTO>(department));
        }

        [HttpPut]
        [Route("UpdateDepartment/{id}")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] UpdateDepartmentDTO departmentDTO)
        {
            var department = await uof.DepartmentRepository.GetByIdAsync(departmentDTO.Id);
            if (department is null)
                return NotFound($"No Department with id {departmentDTO.Id} was found");

            if (!ModelState.IsValid || id != departmentDTO.Id)
                return BadRequest("INVALID Format");

            mapper.Map(departmentDTO, department);
            await uof.DepartmentRepository.UpdateAsync(department);
            uof.SaveChanges();

            return Ok(mapper.Map<GetAllDepartmentsDTO>(department));
        }

        [HttpDelete]
        [Route("DeleteDepartment/{id}")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await uof.DepartmentRepository.GetByIdAsync(id);
            if (department is null)
                return NotFound($"No Department with id {id} was found");

            uof.DepartmentRepository.DeleteAsync(id);
            uof.SaveChanges();

            return Ok($"Department with id {id} has been deleted successfully");
        }
    }
}
