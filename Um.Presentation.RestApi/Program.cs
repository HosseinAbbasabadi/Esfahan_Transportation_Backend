using Autofac;
using System.IdentityModel.Tokens.Jwt;
using System.IO.Compression;
using System.Security.Claims;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using PhoenixFramework.Autofac;
using PhoenixFramework.Core;
using Um.Presentation.RestApi;
using UserManagement.Infrastructure.Config;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddRazorPages();

builder.Services.AddLogging();
builder.Services.AddRazorPages();
builder.Services.Configure<GzipCompressionProviderOptions>
    (options => options.Level = CompressionLevel.Fastest);

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.AddHttpContextAccessor();

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

if (allowedOrigins is not null)
    builder.Services.AddCors(options => options
        .AddPolicy("UserManagement",
            builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithOrigins(allowedOrigins)
        ));

builder.Services.AddSignalR();

builder.Services.AddControllers().AddNewtonsoftJson();

var authorities = builder.Configuration.GetSection("IdentityAuthorities");
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = authorities["0"];
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserManagementApi", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "UserManagementApi");
    });
});

var connectionString = builder.Configuration.GetConnectionString("Application");

if (string.IsNullOrWhiteSpace(connectionString))
    throw new Exception("Please Set Connection String");

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule<PhoenixFrameworkModule>();
    containerBuilder.RegisterModule(new UserManagementModule(connectionString));
});

var app = builder.Build();
var autofacContainer = app.Services.GetAutofacRoot();
ServiceLocator.SetCurrent(new AutofacServiceLocator(autofacContainer));

app.UseResponseCompression();

app.UseStaticFiles();
app.UseDeveloperExceptionPage();
IdentityModelEventSource.ShowPII = true;

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("UserManagement");

app.UseAuthentication();
app.UseAuthorization();

app.ConfigureExceptionHandler();
app.UseAntiXssMiddleware();

app.MapControllers().RequireAuthorization("UserManagementApi");
app.MapRazorPages();
app.MapDefaultControllerRoute();

app.Run();