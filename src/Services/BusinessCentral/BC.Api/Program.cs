var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;

builder.Services.Configure<BusinessCentralSettings>(builder.Configuration.GetSection(nameof(BusinessCentralSettings)));
builder.Services.AddSingleton<IBusinessCentralToken, BusinessCentralToken>();

builder.Services.Configure<MSD365Settings>(builder.Configuration.GetSection(nameof(MSD365Settings)));
builder.Services.AddSingleton<IMSDToken, MSD365Token>();

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

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token in the text input below.\n\nExample: Bearer eyJhbGciOiJIUzI1NiIsInR..."
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthentication();
app.UseAuthorization();

// Use Carter
app.MapCarter();
app.Run();
