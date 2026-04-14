namespace Madrasty.Entites;

public partial class StudentCourse
{
    public int StudentId { get; set; }

    public int CourseId { get; set; }

    public int Grade { get; set; }

    public virtual Course Course { get; set; }

    public virtual Student Student { get; set; }
}