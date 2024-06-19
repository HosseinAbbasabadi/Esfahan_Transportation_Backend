using PhoenixFramework.Application.Command;
using Um.Presentation.Facade.Contract.OrganizationChart;
using UserManagement.Application.Contracts.OrganizationChart;

namespace Um.Presentation.Facade.Command;

public class OrganizationChartCommandFacade : IOrganizationChartCommandFacade
{
    private readonly ICommandBus _commandBus;
    private readonly IResponsiveCommandBus _responsiveCommandBus;

    public OrganizationChartCommandFacade(ICommandBus commandBus, IResponsiveCommandBus responsiveCommandBus)
    {
        _commandBus = commandBus;
        _responsiveCommandBus = responsiveCommandBus;
    }

    public void Create(ChangeOrganizationChartParent command)
    {
        _commandBus.Dispatch(command);
    }

    public long Edit(EditOrganizationChart command)
    {
        return _responsiveCommandBus.Dispatch<EditOrganizationChart, long>(command);
    }

    public void Delete(Guid guid)
    {
        var command = new DeleteOrganizationChart(guid);
        _commandBus.Dispatch(command);
    }
}