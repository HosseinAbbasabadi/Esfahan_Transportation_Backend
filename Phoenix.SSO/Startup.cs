using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Phoenix.SSO.IdentitySettings;
using Phoenix.SSO.Validators;
using PhoenixFramework.Autofac;
using PhoenixFramework.Core;
using PhoenixFramework.Identity;
using UserManagement.Persistence;

namespace Phoenix.SSO
{
    public class Startup
    {
        private IWebHostEnvironment Environment { get; }
        private IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            var idsBuilder = services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                    options.EmitStaticAudienceClaim = true;
                })
                .AddCustomTokenRequestValidator<CustomTokenRequestValidator>();

            var tokenExpiryTime = int.Parse(Configuration["TokenExpiryTime"]) * 60;
            var allowedOrigins = Configuration.GetSection("AllowedOrigins").Get<string[]>();

            idsBuilder.AddInMemoryIdentityResources(IdentityServiceConfiguration.IdentityResources());
            idsBuilder.AddInMemoryApiScopes(IdentityServiceConfiguration.ApiScopes());
            idsBuilder.AddInMemoryClients(IdentityServiceConfiguration.Clients(tokenExpiryTime, allowedOrigins));
            idsBuilder.AddDeveloperSigningCredential();

            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<IPasswordValidator, PasswordValidator>();

            var connectionString = Configuration.GetConnectionString("Application");
            services.AddDbContext<UserManagementCommandContext>(builder =>
                builder.UseSqlServer(connectionString), ServiceLifetime.Transient);

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
            });

            services.AddAuthentication();
        }
        
        public void ConfigureContainer(ContainerBuilder container)
        {
            container.RegisterModule<PhoenixFrameworkModule>();
        }
        
        public void Configure(IApplicationBuilder app)
        {
            var autofacContainer = app.ApplicationServices.GetAutofacRoot();
            ServiceLocator.SetCurrent(new AutofacServiceLocator(autofacContainer));
    
            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            
            app.UseCookiePolicy();

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
