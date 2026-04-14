namespace Madrasty_API.DTOs.Course
{
    public class GetAllCoursesDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Duration { get; set; }
        public int TopicId { get; set; }
        public string TopicName { get; set; } = string.Empty;
    }
}
