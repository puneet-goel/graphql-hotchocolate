using GraphQL.DTOs;
using GraphQL.Schema.Subscriptions;
using GraphQL.Services.Courses;
using HotChocolate.Subscriptions;

namespace GraphQL.Schema.Mutation;

public class Mutation
{
    private CourseRepository _courseRepository;

    public Mutation(CourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<CourseDTO> CreateCourse(CourseInputType courseInput, [Service] ITopicEventSender topicEventSender)
    {
        CourseDTO course = new()
        {
            Name = courseInput.Name,
            Subject = courseInput.Subject,
            InstructorId = courseInput.InstructorId
        };

        course = await _courseRepository.Create(course);

        CourseResult courseResult = new()
        {
            Id = course.Id,
            Name = courseInput.Name,
            Subject = courseInput.Subject,
            InstructorId = courseInput.InstructorId
        };

        await topicEventSender.SendAsync(nameof(Subscription.CourseCreated), course);
        return course;
    }

    public async Task<CourseDTO> UpdateCourse(Guid Id, CourseInputType courseInput, [Service] ITopicEventSender topicEventSender)
    {
        CourseDTO course = new()
        {
            Id = Id,
            Name = courseInput.Name,
            Subject = courseInput.Subject,
            InstructorId = courseInput.InstructorId
        };

        course = await _courseRepository.Update(course);

        CourseResult courseResult = new()
        {
            Id = course.Id,
            Name = course.Name,
            Subject = course.Subject,
            InstructorId = course.InstructorId
        };

        string customEvent = $"{course.Id}_{nameof(Subscription.CourseUpdated)}"; 
        await topicEventSender.SendAsync(customEvent, course);

        return course;
    }

    public async Task<bool> DeleteCourse(Guid Id)
    {
        try
        {
            return await _courseRepository.Delete(Id);
        } catch
        {
            return false;
        }
    }
}
