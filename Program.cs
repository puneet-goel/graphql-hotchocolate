using GraphQL.Schema.Mutation;
using GraphQL.Schema.Queries;
using GraphQL.Schema.Subscriptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddGraphQLServer()
    .AddInMemorySubscriptions()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>();


var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGraphQL();

app.UseWebSockets();

app.Run();
