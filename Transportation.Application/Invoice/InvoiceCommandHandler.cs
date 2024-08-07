using PhoenixFramework.Application;
using PhoenixFramework.Application.Command;
using PhoenixFramework.Identity;
using Transport.Domain.InvoiceAgg;

namespace Transportation.Application.Invoice;

public class InvoiceCommandHandler :
    ICommandHandlerAsync<CreateInvoice, Guid>,
    ICommandHandlerAsync<EditInvoice>,
    ICommandHandlerAsync<DeleteInvoice>
{
    private readonly IClaimHelper _claimHelper;
    private readonly IInvoiceRepository _invoiceRepository;

    public InvoiceCommandHandler(IInvoiceRepository invoiceRepository, IClaimHelper claimHelper)
    {
        _invoiceRepository = invoiceRepository;
        _claimHelper = claimHelper;
    }

    public async Task<Guid> Handle(CreateInvoice command)
    {
        var currentUserGuid = _claimHelper.GetCurrentUserGuid();
        var date = command.Date.ToGeorgianDateTime();

        var invoice = new Transport.Domain.InvoiceAgg.Invoice(currentUserGuid, command.No, date, command.CustomerId,
            command.Description);

        var details = command.Details.Select(x => new InvoiceDetail(currentUserGuid, x.ProductId, x.Count,
            x.UnitPrice, x.Price, x.Discount, x.FinalPrice, x.Description)).ToList();

        invoice.SetDetails(details);

        await _invoiceRepository.CreateAsync(invoice);

        return invoice.Guid;
    }

    public async Task Handle(EditInvoice command)
    {
        var currentUserGuid = _claimHelper.GetCurrentUserGuid();
        var invoice = await _invoiceRepository.LoadAsync(command.Guid, "Details");

        foreach (var incomingDetail in command.Details)
        {
            if (incomingDetail.IsDeleted)
            {
                var detail = invoice.Details.First(x => x.Id == incomingDetail.Id);
                invoice.Details.Remove(detail);
            }
            else
            {
                if (incomingDetail.Id > 0)
                {
                    var detail = invoice.Details.First(x => x.Id == incomingDetail.Id);
                    detail.Edit(incomingDetail.ProductId, incomingDetail.Count, incomingDetail.UnitPrice,
                        incomingDetail.Price, incomingDetail.Discount, incomingDetail.FinalPrice,
                        incomingDetail.Description);
                }
                else
                {
                    var detail = new InvoiceDetail(currentUserGuid, incomingDetail.ProductId, incomingDetail.Count,
                        incomingDetail.UnitPrice,
                        incomingDetail.Price, incomingDetail.Discount, incomingDetail.FinalPrice,
                        incomingDetail.Description);
                    invoice.Details.Add(detail);
                }
            }
        }
    }

    public async Task Handle(DeleteInvoice command)
    {
        var invoice = await _invoiceRepository.LoadAsync(command.Guid, "Details");
        _invoiceRepository.Delete(invoice);
    }
}