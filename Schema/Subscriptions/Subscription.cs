using GraphQL.Schema.Mutation;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;

namespace GraphQL.Schema.Subscriptions
{
    public class Subscription
    {
        [Subscribe]
        public CourseResult CourseCreated([EventMessage] CourseResult course) => course;

        [SubscribeAndResolve]
        public ValueTask<ISourceStream<CourseResult>> CourseUpdated(Guid courseId, [Service] ITopicEventReceiver topicEventReceiver){
            string topic = $"{courseId}_{nameof(CourseUpdated)}";
            return topicEventReceiver.SubscribeAsync<CourseResult>(topic);
        }
    }
}
