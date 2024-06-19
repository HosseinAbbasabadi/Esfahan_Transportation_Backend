using PhoenixFramework.Application.Command;
using PhoenixFramework.Identity;
using UserManagement.Application.Contracts.OrganizationChart;
using UserManagement.Domain.CompanyAgg;
using UserManagement.Domain.OrganizationChartAgg;
using UserManagement.Domain.OrganizationChartAgg.Services;

namespace UserManagement.Application;

public class OrganizationChartCommandHandler :
    ICommandHandler<EditOrganizationChart, long>,
    ICommandHandler<ChangeOrganizationChartParent>,
    ICommandHandler<DeleteOrganizationChart>
{
    private readonly IClaimHelper _claimHelper;
    private readonly IOrganizationChartService _organizationChartService;
    private readonly IOrganizationChartRepository _organizationChartRepository;
    private readonly ICompanyRepository _companyRepository;

    public OrganizationChartCommandHandler(IOrganizationChartRepository organizationChartRepository,
        IClaimHelper claimHelper, IOrganizationChartService organizationChartService,
        ICompanyRepository companyRepository)
    {
        _organizationChartRepository = organizationChartRepository;
        _claimHelper = claimHelper;
        _organizationChartService = organizationChartService;
        _companyRepository = companyRepository;
    }

    public long Handle(EditOrganizationChart command)
    {
        var currentUserId = _claimHelper.GetCurrentUserGuid();

        OrganizationChart? organizationChart;
        var parentId = _organizationChartRepository.GetIdBy(command.ParentGuid);
        var companyId = _companyRepository.GetIdBy(command.CompanyGuid);

        if (command.Id == 0)
        {
            organizationChart = new OrganizationChart(currentUserId, command.Guid, companyId, command.Title, parentId,
                _organizationChartService);
        }
        else
        {
            organizationChart = _organizationChartRepository.Load(command.Guid);
            organizationChart.Edit(currentUserId, command.Title);
        }

        _organizationChartRepository.SaveOrUpdate(organizationChart);

        return organizationChart.Id;
    }

    public void Handle(ChangeOrganizationChartParent command)
    {
        var currentUserId = _claimHelper.GetCurrentUserGuid();

        var organizationChart = _organizationChartRepository.Load(command.Guid);
        var parentId = _organizationChartRepository.GetIdBy(command.ParentGuid);

        organizationChart.ChangeParent(currentUserId, parentId);

        _organizationChartRepository.Create(organizationChart);
    }

    public void Handle(DeleteOrganizationChart command)
    {
        var organizationChart = _organizationChartRepository.Load(command.Guid);
        _organizationChartRepository.Delete(organizationChart);
    }
}