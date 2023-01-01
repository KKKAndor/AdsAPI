using Ads.Application;
using Ads.Application.Common.Mappings;
using Ads.WebApi.Extensions;
using Ads.WebApi.Middleware;
using System.Reflection;
using Ads.Domain.Interfaces;
using Ads.Persistence;
using Ads.Persistence.Interfaces;
using Ads.Persistence.Repositories;
using Ads.Persistence.UnitOfWork;
using Ads.WebApi.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddTransient(typeof(IMainRepository), typeof(MainRepository));
builder.Services.AddTransient<IAdRepository, AdRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(con =>
{
    con.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
    con.AddProfile(new AssemblyMappingProfile());
});

builder.Services.AddApplication();

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(config =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    config.IncludeXmlComments(xmlPath);
});

builder.Services.AddHostedService<BackgroundAdService>();


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
