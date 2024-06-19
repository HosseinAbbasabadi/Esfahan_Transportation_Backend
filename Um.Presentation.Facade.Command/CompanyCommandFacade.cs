using PhoenixFramework.Application.Command;
using Um.Presentation.Facade.Contract.Company;
using UserManagement.Application.Contracts.Company;

namespace Um.Presentation.Facade.Command;

public class CompanyCommandFacade : ICompanyCommandFacade
{
    private readonly ICommandBus _commandBus;
    private readonly IResponsiveCommandBus _responsiveCommandBus;

    public CompanyCommandFacade(ICommandBus commandBus, IResponsiveCommandBus responsiveCommandBus)
    {
        _commandBus = commandBus;
        _responsiveCommandBus = responsiveCommandBus;
    }

    public long Create(CreateCompany command)
    {
        return _responsiveCommandBus.Dispatch<CreateCompany, long>(command);
    }

    public void Edit(EditCompany command)
    {
        _commandBus.Dispatch(command);
    }

    public void Delete(Guid guid)
    {
        var command = new DeleteCompany(guid);
        _commandBus.Dispatch(command);
    }

    public void Deactivate(Guid guid)
    {
        var command = new DeactivateCompany(guid);
        _commandBus.Dispatch(command);
    }

    public void Activate(Guid guid)
    {
        var command = new ActivateCompany(guid);
        _commandBus.Dispatch(command);
    }
}
