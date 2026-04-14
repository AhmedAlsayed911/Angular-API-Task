using System.ComponentModel.DataAnnotations;

namespace Madrasty_API.DTOs.Student
{
    public class AddStudentDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Range(1, 60)]
        public int Age { get; set; }

        [Range(1, int.MaxValue)]
        public int DepartmentId { get; set; }
    }
}
