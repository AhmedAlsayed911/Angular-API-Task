namespace Madrasty_API.DTOs.Department
{
    public class GetAllDepartmentsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }
}
