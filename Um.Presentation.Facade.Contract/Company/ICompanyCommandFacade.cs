using PhoenixFramework.Core;
using UserManagement.Application.Contracts.Company;

namespace Um.Presentation.Facade.Contract.Company;

public interface ICompanyCommandFacade : IFacadeService
{
    //[HasPermission("CreateCompany")]
    long Create(CreateCompany command);

    //[HasPermission("EditCompany")]
    void Edit(EditCompany command);
    void Delete(Guid guid);
    void Deactivate(Guid guid);
    void Activate(Guid guid);
}