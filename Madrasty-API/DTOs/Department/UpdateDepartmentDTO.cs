using System.ComponentModel.DataAnnotations;

namespace Madrasty_API.DTOs.Department
{
    public class UpdateDepartmentDTO
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;
    }
}
