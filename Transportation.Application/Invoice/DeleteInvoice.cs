using PhoenixFramework.Application.Command;

namespace Transportation.Application.Invoice;

public class DeleteInvoice : ICommand
{
    public DeleteInvoice(Guid guid)
    {
        Guid = guid;
    }

    public Guid Guid { get; set; }
}