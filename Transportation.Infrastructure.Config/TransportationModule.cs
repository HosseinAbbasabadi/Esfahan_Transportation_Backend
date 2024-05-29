using Autofac;
using Autofac.Extras.DynamicProxy;
using Microsoft.EntityFrameworkCore;
using PhoenixFramework.Application.Command;
using PhoenixFramework.Application.Query;
using PhoenixFramework.Autofac;
using PhoenixFramework.Domain;
using Transportation.Application.VehicleType;
using Transportation.Infrastructure.Efcore;
using Transportation.Infrastructure.Efcore.Repository;
using Transportation.Infrastructure.Query.VehicleType;
using Transportation.Presentation.Facade.Command.VehicleType;
using Transportation.Presentation.Facade.Query.VehicleType;

namespace Transportation.Infrastructure.Config;

public class TransportationModule : Module
{
    private string ConnectionString { get; set; }
    private string SsoConnectionString { get; set; }

    public TransportationModule(string connectionString, string ssoConnectionString)
    {
        ConnectionString = connectionString;
        SsoConnectionString = ssoConnectionString;
    }

    protected override void Load(ContainerBuilder builder)
    {
        var commandHandlersAssembly = typeof(VehicleTypeCommandHandler).Assembly;
        builder.RegisterAssemblyTypes(commandHandlersAssembly)
            .AsClosedTypesOf(typeof(ICommandHandler<>))
            .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(commandHandlersAssembly)
            .AsClosedTypesOf(typeof(ICommandHandlerAsync<>))
            .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(commandHandlersAssembly)
            .AsClosedTypesOf(typeof(ICommandHandler<,>))
            .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(commandHandlersAssembly)
            .AsClosedTypesOf(typeof(ICommandHandlerAsync<,>))
            .InstancePerLifetimeScope();

        var queryHandlerAssembly = typeof(VehicleTypeQueryHandler).Assembly;
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
                var optionsBuilder = new DbContextOptionsBuilder<TransportationCommandContext>();
                optionsBuilder.UseSqlServer(ConnectionString);
                return new TransportationCommandContext(optionsBuilder.Options);
            })
            .As<DbContext>()
            .As<TransportationCommandContext>()
            .InstancePerLifetimeScope();

        builder.Register(_ =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<TransportationQueryContext>();
                optionsBuilder.UseSqlServer(ConnectionString);
                return new TransportationQueryContext(optionsBuilder.Options);
            })
            .As<TransportationQueryContext>()
            .InstancePerDependency();

        var repositoryAssembly = typeof(VehicleTypeRepository).Assembly;
        builder.RegisterAssemblyTypes(repositoryAssembly)
            .AsClosedTypesOf(typeof(IRepository<,>))
            .InstancePerLifetimeScope();

        //var domainServiceAssembly = typeof(VechicleTypeService).Assembly;
        //builder.RegisterAssemblyTypes(domainServiceAssembly)
        //    .Where(t => t.Name.EndsWith("Service"))
        //    .AsImplementedInterfaces()
        //    .InstancePerLifetimeScope();

        var facadeAssembly = typeof(VehicleTypeCommandFacade).Assembly;
        builder.RegisterAssemblyTypes(facadeAssembly)
            .Where(t => t.Name.EndsWith("CommandFacade"))
            .InstancePerLifetimeScope()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(SecurityInterceptor))
            .AsImplementedInterfaces();

        var facadeQueryAssembly = typeof(VehicleTypeQueryFacade).Assembly;
        builder.RegisterAssemblyTypes(facadeQueryAssembly)
            .Where(t => t.Name.EndsWith("QueryFacade"))
            .InstancePerLifetimeScope()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(SecurityInterceptor))
            .AsImplementedInterfaces();

        base.Load(builder);
    }
}