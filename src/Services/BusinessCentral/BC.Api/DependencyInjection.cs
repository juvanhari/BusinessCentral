using BuildingBlocks.Exceptions.Handlers;

namespace BC.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfraStructureServices
            (this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database");
            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            return services;
        }

        public static IServiceCollection AddApplicationServices
            (this IServiceCollection services, IConfiguration configuration, Assembly assembly)
        {
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                options.AddOpenBehavior(typeof(ValidationBehavior<,>));
                options.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });

            // Register for Fluent Validator
            services.AddValidatorsFromAssembly(assembly);

            return services;
        }

        public static IServiceCollection AddPresentationServices
            (this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BusinessCentralSettings>(configuration.GetSection(nameof(BusinessCentralSettings)));
            services.AddSingleton<IBusinessCentralToken, BusinessCentralToken>();

            services.Configure<MSD365Settings>(configuration.GetSection(nameof(MSD365Settings)));
            services.AddSingleton<IMSDToken, MSD365Token>();

            // Register Carter for Exposing HTTP Endpoints
            services.AddCarter();
            services.AddExceptionHandler<CustomExceptionHandler>();
            services.AddEndpointsApiExplorer(); // Enables support for minimal APIs builder.Services.AddSwaggerGen();
            services.ConfigureSwaggerSettings();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAd"));

            // Registering Http Clients
            services.AddHttpClient("MSD365BC", httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["MSD365Api"]!);

                httpClient.DefaultRequestHeaders.Add(
                    HeaderNames.Accept, "application/json");
                httpClient.DefaultRequestHeaders.Add(
                    HeaderNames.UserAgent, "BC.Api");
            });


            return services;
        }

        private static IServiceCollection ConfigureSwaggerSettings
          (this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
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

            return services;
        }

    }
}
