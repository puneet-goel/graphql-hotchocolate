using GraphQL.Schema.Subscriptions;
using HotChocolate.Subscriptions;

namespace GraphQL.Schema.Mutation;

public class Mutation
{
    private readonly List<CourseResult> _courses;

    public Mutation()
    {
        _courses = new();
    }

    public async Task<CourseResult> CreateCourse(CourseInputType courseInputType, [Service] ITopicEventSender topicEventSender)
    {
        CourseResult course = new()
        {
            Id = new Guid(),
            Name = courseInputType.Name,
            Subject = courseInputType.Subject,
            InstructorId = courseInputType.InstructorId
        };

        _courses.Add(course);
        await topicEventSender.SendAsync(nameof(Subscription.CourseCreated), course);
        return course;
    }

    public async Task<CourseResult> UpdateCourse(Guid Id, CourseInputType courseInputType, [Service] ITopicEventSender topicEventSender)
    {
        CourseResult course = _courses.FirstOrDefault(ele => ele.Id == Id);

        if(course == null)
        {
            throw new GraphQLException(new Error("not found", "NOT_FOUND"));
        }

        course.Name = courseInputType.Name;
        course.Subject = courseInputType.Subject;
        course.InstructorId = courseInputType.InstructorId;


        string customEvent = $"{course.Id}_{nameof(Subscription.CourseUpdated)}"; 
        await topicEventSender.SendAsync(customEvent, course);

        return course;
    }

    public bool DeleteCourse(Guid Id)
    {
        return _courses.RemoveAll(ele => ele.Id == Id) >= 1;
    }
}
