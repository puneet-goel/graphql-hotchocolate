using GraphQL.DTOs;
using GraphQL.Services.Courses;

namespace GraphQL.Schema.Queries
{
    public class Query
    {
        private readonly CourseRepository _courseRepository;

        public Query(CourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<IEnumerable<CourseDTO>> GetCourses()
        {
            return await _courseRepository.GetAll();
        }

        public async Task<CourseDTO> GetCourseByIdAsync(Guid id)
        {
            return await _courseRepository.GetById(id);
        }

        public string Instructions => "First graphql query";

        [GraphQLDeprecated("this query is deprecated")]
        public string InstructionsDeprecated => "First graphql query";
    }
}
