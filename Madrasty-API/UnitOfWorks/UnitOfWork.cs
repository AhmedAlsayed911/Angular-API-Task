using Madrasty.Entites;
using Madrasty_API.Repositories;

namespace Madrasty_API.UnitOfWork
{
    public class UnitOfWork
    {
        private readonly ApplicationDbContext dbContext;
        private IGenericRepository<Student> studentRepository;
        private IGenericRepository<Department> departmentRepository;
        private IGenericRepository<Course> courseRepository;
        private IGenericRepository<Topic> topicRepository;
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IGenericRepository<Student> StudentRepository
        {
            get
            {
                if (studentRepository is null)
                    studentRepository = new GenericRepository<Student>(dbContext);
                return studentRepository;
            }
        }
        public IGenericRepository<Department> DepartmentRepository
        {
            get
            {
                if (departmentRepository is null)
                    departmentRepository = new GenericRepository<Department>(dbContext);
                return departmentRepository;
            }
        }
        public IGenericRepository<Course> CourseRepository
        {
            get
            {
                if (courseRepository is null)
                    courseRepository = new GenericRepository<Course>(dbContext);
                return courseRepository;
            }
        }
        public IGenericRepository<Topic> TopicRepository
        {
            get
            {
                if (topicRepository is null)
                    topicRepository = new GenericRepository<Topic>(dbContext);
                return topicRepository;
            }
        }
        public void SaveChanges()
        {
            dbContext.SaveChanges();
        }
    }
}
