using Netrift.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Microsoft.EntityFrameworkCore;
using Netrift.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Netrift.Infrastructure.Identity.IdentityEntities;
using Netrift.Domain.Abstractions.IdentityAbstractions;
using Netrift.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
  // SqLite Database
  builder.Services.AddDbContext<AppDbContext>(options =>
  {
    options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
    options.EnableSensitiveDataLogging();
  });

  builder.Services.AddEndpointsApiExplorer();
  builder.Services.AddSwaggerGen();
}

builder.Services.AddLogging();

builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
builder.Services.ConfigureApplicationCookie(options =>
{
  // Configure the events for cookie authentication
  options.Events = new CookieAuthenticationEvents
  {
    // When the user is not authenticated, return 401 instead of redirecting
    OnRedirectToLogin = context =>
        {
          context.Response.StatusCode = StatusCodes.Status401Unauthorized;
          return Task.CompletedTask;
        },
    // When the user is authenticated but not authorized, return 403 instead of redirecting
    OnRedirectToAccessDenied = context =>
        {
          context.Response.StatusCode = StatusCodes.Status403Forbidden;
          return Task.CompletedTask;
        }
  };
});

builder.Services.AddSerilog(options =>
{
  options.ReadFrom.Configuration(builder.Configuration);

});

builder.Services.AddCleanArchitectureLayers();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
  options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddScoped<IIdentityService, IdentityService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  // Swagger
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseSerilogRequestLogging(options =>
{
  options.EnrichDiagnosticContext = (diagContext, httpContext) =>
  {
    diagContext.Set("RemoteIP", httpContext.Connection.RemoteIpAddress);
    diagContext.Set("Agent", httpContext.Request.Headers["User-Agent"]);
  };

  options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed} for {RemoteIP} {Agent}";
});
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (IServiceScope serviceScope = app.Services.CreateScope())
{
  AppDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
  dbContext.Database.Migrate();
}

app.Run();
