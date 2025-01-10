using GraphQL.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Services
{
    public class SchoolDbContext: DbContext
    {
        public DbSet<CourseDTO> Courses { get; set; }
        public DbSet<InstructorDTO> Instructors { get; set; }
        public DbSet<StudentDTO> Students { get; set; }

        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
        {

        }
    }
}
