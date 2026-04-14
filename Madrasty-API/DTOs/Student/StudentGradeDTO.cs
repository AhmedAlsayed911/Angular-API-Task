namespace Madrasty_API.DTOs.Student
{
    public class StudentGradeDTO
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int Grade { get; set; }
    }
}