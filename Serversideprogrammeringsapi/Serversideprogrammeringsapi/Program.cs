using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serversideprogrammeringsapi.Database;
using Serversideprogrammeringsapi.Database.Models;
using Serversideprogrammeringsapi.Env;
using Serversideprogrammeringsapi.Identity;
using Serversideprogrammeringsapi.Identity.Repo;
using Serversideprogrammeringsapi.Repo.OneTimePasswordRepo;
using Serversideprogrammeringsapi.Schema.Mutations;
using Serversideprogrammeringsapi.Schema.Query;
using Serversideprogrammeringsapi.Services.AuthService;
using Serversideprogrammeringsapi.Services.ExternalContactService;

string dbString = EnvHandler.UserDBString();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;
IWebHostEnvironment env = builder.Environment;


IConfigurationBuilder configBuilder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

IConfigurationRoot configuration = configBuilder.Build();

IdentityConfiguration.Configure(configuration, services);

services.AddPooledDbContextFactory<ApiDbContext>(o => o.UseSqlServer(EnvHandler.UserDBString()));
services.AddPooledDbContextFactory<ToDoDbContext>(o => o.UseSqlServer(EnvHandler.ToDoDBString()));

services.AddScoped<ApiDbContext>(p => p.GetRequiredService<IDbContextFactory<ApiDbContext>>().CreateDbContext());
services.AddScoped<ToDoDbContext>(p => p.GetRequiredService<IDbContextFactory<ToDoDbContext>>().CreateDbContext());
services.AddLogging();
services.AddSingleton<IExternalContactService, ExternalContactService>();


services.TryAddTransient<IHttpContextAccessor, HttpContextAccessor>();
services.AddScoped<SignInManager<ApiUser>>();
services.AddTransient<IUserManager, UserManager>();
services.AddTransient<IRefreshTokenRepo, RefreshTokenRepo>();


services.AddScoped<IOTPRepo, OTPRepo>();
services.AddScoped<IAuthService, AuthService>();

services
    .AddGraphQLServer()
    .RegisterDbContext<ApiDbContext>(DbContextKind.Pooled)
    .RegisterDbContext<ToDoDbContext>(DbContextKind.Pooled)
    .RegisterService<IAuthService>()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddTypeExtension<AuthMutations>()
    .ModifyOptions(o => o.EnableOneOf = true)
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    .AddAuthorization()
    .InitializeOnStartup();




if (env.IsDevelopment())
{
    services.AddSingleton<IMailSenderService, DummyMailSenderService>();
}
else // Production serive
{
    services.AddSingleton<IMailSenderService, SimpleSmtpMailSenderService>();
}

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

app.MapGraphQL().WithOptions(new HotChocolate.AspNetCore.GraphQLServerOptions()
{
    EnableBatching = true,
});

InitialData.PopulateTestData(app).Wait();

app.Run();