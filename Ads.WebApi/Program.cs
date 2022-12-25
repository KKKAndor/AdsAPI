using Ads.Application;
using Ads.Application.Common.Mappings;
using Ads.Application.Interfaces;
using Ads.Infrastructure;
using Ads.WebApi.Extensions;
using Ads.WebApi.Middleware;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.AddAutoMapper(con =>
{
    con.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
    con.AddProfile(new AssemblyMappingProfile(typeof(IAdsDbContext).Assembly));
});

builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddControllers();


builder.Services.AddSwaggerGen(config =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    config.IncludeXmlComments(xmlPath);
});



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var serviceProvider = scope.ServiceProvider;
        try
        {
            var dbService = serviceProvider.GetRequiredService<AdsDbContext>();
            DbInitializer.Initialize(dbService);
        }
        catch (Exception exception)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(exception, "An error occured while app initialization");
        }
    }
}

app.UseSwagger();
app.UseSwaggerUI(config =>
{
    config.RoutePrefix = string.Empty;
    config.SwaggerEndpoint("swagger/v1/swagger.json", "Ads API");
});
app.UseCustomExceptionHandler();
app.UseHsts();
app.UseRouting();
app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
