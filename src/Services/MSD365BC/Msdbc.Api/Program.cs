using BuildingBlocks.Behaviours;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

// Register for Fluent Validator
builder.Services.AddValidatorsFromAssembly(assembly);

// Register Carter for Exposing HTTP Endpoints
builder.Services.AddCarter();

builder.Services.AddEndpointsApiExplorer(); // Enables support for minimal APIs builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Use Carter
app.MapCarter();

app.Run();
