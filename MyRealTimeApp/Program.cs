using API.GraphQL;
using Core.Application.Interfaces;
using HotChocolate.Subscriptions;
using Microsoft.EntityFrameworkCore;
using MyRealTimeApp.API.GraphQL;
using MyRealTimeApp.Core.Application.Interfaces;
using MyRealTimeApp.Core.Application.Services;
using MyRealTimeApp.Infrastructure.Data;
using MyRealTimeApp.Infrastructure.RealTime;
using Infrastructure.Repositories;

//The application entry point where all the dependencies are registered.
//Note the specific order of UseWebSockets() before MapGraphQL().

var builder = WebApplication.CreateBuilder(args);

// Grab the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");



// 1. Dependency Injection for Infrastructure
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// 2. Dependency Injection for Application Core
builder.Services.AddScoped<IProductService, ProductService>();

// 3. Register the SSMS Background Listener
builder.Services.AddSingleton<IHostedService>(sp =>
    new SqlListener(connectionString!, sp.GetRequiredService<ITopicEventSender>()));

// 4. Configure GraphQL Subscriptions AND Queries
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()               // <--- ADD THIS LINE
    .AddSubscriptionType<Subscription>()
    .AddMutationType<Mutation>()         // <-- ADDED THE MUTATION HERE
    .AddInMemorySubscriptions();

var app = builder.Build();

app.UseRouting();

// CRITICAL: WebSockets must be enabled for GraphQL Subscriptions to work
app.UseWebSockets();
app.MapGraphQL();

app.Run();