namespace Madrasty.Entites;

public partial class DepartmentCourse
{
    public int DepartmentId { get; set; }

    public int CourseId { get; set; }

    public virtual Department Department { get; set; }

    public virtual Course Course { get; set; }
}
