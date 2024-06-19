// using System;
// using Autofac;
// using Autofac.Extensions.DependencyInjection;
// using Castle.Windsor.Installer;
// using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Hosting;
// using Phoenix.SSO.IdentitySettings;
// using Phoenix.SSO.Validators;
// using PhoenixFramework.Autofac;
// using PhoenixFramework.Core;
// using PhoenixFramework.Identity;
// using Serilog;
// using Serilog.Events;
// using Serilog.Sinks.SystemConsole.Themes;
// using UserManagement.Persistence;
//
// Log.Logger = new LoggerConfiguration()
//     .MinimumLevel.Debug()
//     .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
//     .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
//     .MinimumLevel.Override("System", LogEventLevel.Warning)
//     .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
//     .Enrich.FromLogContext()
//     // uncomment to write to Azure diagnostics stream
//     //.WriteTo.File(
//     //    @"D:\home\LogFiles\Application\identityserver.txt",
//     //    fileSizeLimitBytes: 1_000_000,
//     //    rollOnFileSizeLimit: true,
//     //    shared: true,
//     //    flushToDiskInterval: TimeSpan.FromSeconds(1))
//     .WriteTo.Console(
//         outputTemplate:
//         "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
//         theme: AnsiConsoleTheme.Code)
//     .CreateLogger();
//
// try
// {
//     var builder = WebApplication.CreateBuilder(args);
//
//     builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
//
//     builder.Services.AddControllersWithViews();
//
//     var idsBuilder = builder.Services.AddIdentityServer(options =>
//         {
//             options.Events.RaiseErrorEvents = true;
//             options.Events.RaiseInformationEvents = true;
//             options.Events.RaiseFailureEvents = true;
//             options.Events.RaiseSuccessEvents = true;
//             options.EmitStaticAudienceClaim = true;
//         })
//         .AddCustomTokenRequestValidator<CustomTokenRequestValidator>();
//
//     var tokenExpiryTime = int.Parse(builder.Configuration["TokenExpiryTime"]) * 60;
//     var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
//
//     idsBuilder.AddInMemoryIdentityResources(IdentityServiceConfiguration.IdentityResources());
//     idsBuilder.AddInMemoryApiScopes(IdentityServiceConfiguration.ApiScopes());
//     idsBuilder.AddInMemoryClients(IdentityServiceConfiguration.Clients(tokenExpiryTime, allowedOrigins));
//     idsBuilder.AddDeveloperSigningCredential();
//
//     builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();
//     builder.Services.AddTransient<IPasswordValidator, PasswordValidator>();
//
//     var connectionString = builder.Configuration.GetConnectionString("Application");
//     builder.Services.AddDbContext<UserManagementCommandContext>(builder =>
//         builder.UseSqlServer(connectionString), ServiceLifetime.Transient);
//
//     builder.Services.AddAuthentication();
//
//     builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
//     {
//         containerBuilder.RegisterModule<PhoenixFrameworkModule>();
//     });
//     
//
//     var app = builder.Build();
//     var autofacContainer = app.Services.GetAutofacRoot();
//     ServiceLocator.SetCurrent(new AutofacServiceLocator(autofacContainer));
//     
//     app.UseStaticFiles();
//
//     app.UseRouting();
//     app.UseIdentityServer();
//     app.UseAuthorization();
//     app.MapControllers();
//     app.MapDefaultControllerRoute();
//
//     app.Run();
//
//     return 0;
// }
// catch (Exception ex)
// {
//     Log.Fatal(ex, "Host terminated unexpectedly.");
//     return 1;
// }
// finally
// {
//     Log.Information("finally");
//     Log.CloseAndFlush();
//
// }

using System;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Phoenix.SSO;

public class Program
{
    public static int Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console(
                outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                theme: AnsiConsoleTheme.Code)
            .CreateLogger();
        try
        {
            Log.Information("Starting host...");
            CreateHostBuilder(args).Build().Run();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly.");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .UseSerilog()
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
}