using accoudingWeb.DataBase;
using accoudingWeb.Entities;
using accoudingWeb.ViewModels;

namespace accoudingWeb.Services;

public class TicketService(ApplicationContext dbContext)
{
    public Ticket<T> CreateTicket<T>(Employee employee, TicketModel<T> ticketModel) where T : BaseItem<T>
    {
        var ticket = new Ticket<T>
        {
            Employee = employee,
            Name = ticketModel.Name,
            TicketStatus = Ticketstatus.Pending.ToString(),
            ItemStatus = ticketModel.ItemStatus.ToString(),
            Items = dbContext.Set<T>().Where(i => ticketModel.ItemsId.Contains(i.Id)).ToList(),
            TotalPrice = ticketModel.TotalPrice
        };
        
        return ticket;
    }
}