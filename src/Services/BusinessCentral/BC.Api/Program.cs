using Microsoft.Net.Http.Headers;

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

// Registering Http Clients
builder.Services.AddHttpClient("MSD365BC", httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration["MSD365Api"]!);

    httpClient.DefaultRequestHeaders.Add(
        HeaderNames.Accept, "application/json");
    httpClient.DefaultRequestHeaders.Add(
        HeaderNames.UserAgent, "BC.Api");
});


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
