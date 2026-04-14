namespace Madrasty_API.DTOs.Student
{
    public class GetAllStudentsDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int Age { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public List<StudentGradeDTO> Grades { get; set; } = new();
    }
}
