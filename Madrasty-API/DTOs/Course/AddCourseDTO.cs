using System.ComponentModel.DataAnnotations;

namespace Madrasty_API.DTOs.Course
{
    public class AddCourseDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Range(1, 3)]
        public int Duration { get; set; }

        [Range(1, int.MaxValue)]
        public int TopicId { get; set; }
    }
}
