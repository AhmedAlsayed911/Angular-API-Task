using System.ComponentModel.DataAnnotations;

namespace Madrasty_API.DTOs
{
    public class UpdateCourseGradeDTO
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        [Range(1, 100)]
        public int Grade { get; set; }
    }
}
