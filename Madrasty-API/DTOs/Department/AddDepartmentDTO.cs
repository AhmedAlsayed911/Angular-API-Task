using System.ComponentModel.DataAnnotations;

namespace Madrasty_API.DTOs.Department
{
    public class AddDepartmentDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;
    }
}
