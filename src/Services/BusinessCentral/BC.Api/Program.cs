using BC.Api;
using BC.Api.Extensions;
using BC.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;


builder.Services.
     AddApplicationServices(builder.Configuration, assembly)
    .AddInfraStructureServices(builder.Configuration)
    .AddPresentationServices(builder.Configuration);



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.IntialiseDatabaseAsync();
}

app.UseBCHeaderMiddleWare();
app.UseAuthentication();
app.UseAuthorization();

// Use Carter
app.MapCarter();
app.UseExceptionHandler(options => { });
app.Run();
