using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serversideprogrammeringsapi.Database.Models;
using Serversideprogrammeringsapi.Identity;
using Serversideprogrammeringsapi.Identity.Repo;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;

ConfigurationManager configuration = builder.Configuration;

IWebHostEnvironment env = builder.Environment;

services.AddPooledDbContextFactory<ApiDbContext>(o => o.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
services.AddScoped<ApiDbContext>(p => p.GetRequiredService<IDbContextFactory<ApiDbContext>>().CreateDbContext());
services.AddLogging();

IdentityConfiguration.Configure(configuration, services);

services.AddGraphQLServer()
     .AddFiltering()
    .AddSorting()
    .AddProjections()
    .AddAuthorization()
    .InitializeOnStartup();


services.TryAddTransient<IHttpContextAccessor, HttpContextAccessor>();
services.AddTransient<IUserManager, UserManager>();
services.AddTransient<IRefreshTokenRepo, RefreshTokenRepo>();

var app = builder.Build();

if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

app.UseCors(corsPolicyBuilder => corsPolicyBuilder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    .WithExposedHeaders("X-Pagination"));


app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => { endpoints.MapGraphQL(); });


app.Run();