using PhoenixFramework.Application;
using PhoenixFramework.Application.Command;
using PhoenixFramework.Identity;
using UserManagement.Application.Contracts.Company;
using UserManagement.Domain.CompanyAgg;
using UserManagement.Domain.CompanyAgg.Service;

namespace UserManagement.Application;

public class CompanyCommandHandler :
    ICommandHandler<CreateCompany, long>,
    ICommandHandler<EditCompany>,
    ICommandHandler<DeleteCompany>,
    ICommandHandler<DeactivateCompany>,
    ICommandHandler<ActivateCompany>
{
    private readonly IClaimHelper _claimHelper;
    private readonly ICompanyRepository _companyRepository;
    private readonly ICompanyService _companyService;

    public CompanyCommandHandler(ICompanyRepository companyRepository, ICompanyService companyService,
        IClaimHelper claimHelper)
    {
        _companyRepository = companyRepository;
        _companyService = companyService;
        _claimHelper = claimHelper;
    }

    public long Handle(CreateCompany command)
    {
        var creator = _claimHelper.GetCurrentUserGuid();

        byte[] logo = null;
        if (command.Logo != null)
            logo = Tools.ConvertFileToArray(command.Logo);

        //var registerDate = command.RegisterDate.ToGeorgianDateTime();
        long parentId = 0;
        if (command.ParentGuid != null)
            parentId = _companyRepository.GetIdBy(command.ParentGuid.Value);

        var company = new Company(creator, command.Title, parentId, command.Address,
            logo, command.Description, _companyService);

        _companyRepository.Create(company);

        return company.Id;
    }

    public void Handle(EditCompany command)
    {
        var company = _companyRepository.Load(command.Guid);

        var creator = _claimHelper.GetCurrentUserGuid();

        byte[] logo = null;
        if (command.Logo != null)
            logo = Tools.ConvertFileToArray(command.Logo);

        long parentId = 0;
        if (command.ParentGuid != null)
            parentId = _companyRepository.GetIdBy(command.ParentGuid.Value);

        company.Edit(creator, command.Title, parentId, command.Address,
            logo, command.Description, _companyService);

        _companyRepository.Update(company);
    }

    public void Handle(DeleteCompany command)
    {
        var company = _companyRepository.Load(command.Guid);

        _companyService.CheckIfCompanyCanBeDeleted(company);

        _companyRepository.Delete(company);
    }

    public void Handle(ActivateCompany command)
    {
        var company = _companyRepository.Load(command.Id);

        company.Activate();

        _companyRepository.Update(company);
    }

    public void Handle(DeactivateCompany command)
    {
        var company = _companyRepository.Load(command.Id);

        company.Deactivate();

        _companyRepository.Update(company);
    }
}