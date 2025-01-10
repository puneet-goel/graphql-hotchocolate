using GraphQL.Schema.Mutation;
using GraphQL.Schema.Queries;
using GraphQL.Schema.Subscriptions;
using GraphQL.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddGraphQLServer()
    .AddInMemorySubscriptions()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>();

IConfiguration configuration = new ConfigurationBuilder().Build();
string connectionString = configuration.GetConnectionString("default");
builder.Services.AddPooledDbContextFactory<SchoolDbContext>(o => o.UseSqlite(connectionString));

var app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    IDbContextFactory<SchoolDbContext> contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<SchoolDbContext>>();

    using (SchoolDbContext context = contextFactory.CreateDbContext())
    {
        context.Database.Migrate();
    }
}

app.MapGet("/", () => "Hello World!");
app.MapGraphQL();

app.UseWebSockets();

app.Run();
