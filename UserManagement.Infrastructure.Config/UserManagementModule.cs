using Autofac;
using Autofac.Extras.DynamicProxy;
using Microsoft.EntityFrameworkCore;
using PhoenixFramework.Application.Command;
using PhoenixFramework.Application.Query;
using PhoenixFramework.Autofac;
using PhoenixFramework.Domain;
using PhoenixFramework.Identity;
using Um.Presentation.Facade.Command;
using Um.Presentation.Facade.Query;
using UserManagement.Acl.Role;
using UserManagement.Acl.User;
using UserManagement.Application;
using UserManagement.Application.Contracts.Contracts;
using UserManagement.Domain.RoleAgg.Services;
using UserManagement.Domain.UserAgg.Services;
using UserManagement.Persistence;
using UserManagement.Persistence.Repository;
using UserManagement.Query;

namespace UserManagement.Infrastructure.Config;

public class UserManagementModule : Module
{
    private string ConnectionString { get; set; }

    public UserManagementModule(string connectionString)
    {
        ConnectionString = connectionString;
    }
    
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<UserApplication>().As<IUserApplication>();
        builder.RegisterType<RoleApplication>().As<IRoleApplication>();
        builder.RegisterType<RoleValidatorService>().As<IRoleValidatorService>();
        builder.RegisterType<FeatureApplication>().As<IFeatureApplication>();
        builder.RegisterType<RoleAcl>().As<IRoleAcl>();
        builder.RegisterType<UserAcl>().As<IUserAcl>();
        builder.RegisterType<PasswordHasher>().As<IPasswordHasher>();

        var repositoryAssembly = typeof(UserRepository).Assembly;
        builder.RegisterAssemblyTypes(repositoryAssembly)
            .AsClosedTypesOf(typeof(IRepository<,>))
            .InstancePerLifetimeScope();

        var domainServiceAssembly = typeof(UserValidatorService).Assembly;
        builder.RegisterAssemblyTypes(domainServiceAssembly)
            .Where(t => t.Name.EndsWith("Service"))
            .AsImplementedInterfaces();

        var commandHandlersAssembly = typeof(CompanyCommandHandler).Assembly;
        builder.RegisterAssemblyTypes(commandHandlersAssembly)
            .AsClosedTypesOf(typeof(ICommandHandler<>))
            .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(commandHandlersAssembly)
            .AsClosedTypesOf(typeof(ICommandHandler<,>))
            .InstancePerLifetimeScope();

        var queryHandlerAssembly = typeof(RoleQueryHandler).Assembly;
        builder.RegisterAssemblyTypes(queryHandlerAssembly)
            .AsClosedTypesOf(typeof(IQueryHandler<>))
            .InstancePerDependency();

        builder.RegisterAssemblyTypes(queryHandlerAssembly)
            .AsClosedTypesOf(typeof(IQueryHandlerAsync<>))
            .InstancePerDependency();

        builder.RegisterAssemblyTypes(queryHandlerAssembly)
            .AsClosedTypesOf(typeof(IQueryHandler<,>))
            .InstancePerDependency();

        builder.RegisterAssemblyTypes(queryHandlerAssembly)
            .AsClosedTypesOf(typeof(IQueryHandlerAsync<,>))
            .InstancePerDependency();

        builder.Register(_ =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<UserManagementCommandContext>();
                optionsBuilder.UseSqlServer(ConnectionString);
                return new UserManagementCommandContext(optionsBuilder.Options);
            })
            .As<DbContext>()
            .As<UserManagementCommandContext>()
            .InstancePerLifetimeScope();

        builder.Register(_ =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<UserManagementQueryContext>();
                optionsBuilder.UseSqlServer(ConnectionString);
                return new UserManagementQueryContext(optionsBuilder.Options);
            })
            .As<UserManagementQueryContext>()
            .InstancePerDependency();

        var facadeAssembly = typeof(CompanyCommandFacade).Assembly;
        builder.RegisterAssemblyTypes(facadeAssembly)
            .Where(t => t.Name.EndsWith("CommandFacade"))
            .InstancePerLifetimeScope()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(SecurityInterceptor))
            .AsImplementedInterfaces();

        var facadeQueryAssembly = typeof(CompanyQueryFacade).Assembly;
        builder.RegisterAssemblyTypes(facadeQueryAssembly)
            .Where(t => t.Name.EndsWith("QueryFacade"))
            .InstancePerLifetimeScope()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(SecurityInterceptor))
            .AsImplementedInterfaces();
        base.Load(builder);
    }
}