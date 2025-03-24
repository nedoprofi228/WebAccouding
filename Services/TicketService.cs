using accoudingWeb.DataBase;
using accoudingWeb.Entities;
using accoudingWeb.ViewModels;

namespace accoudingWeb.Services;

public class TicketService(ApplicationContext dbContext)
{
    public Ticket<T> CreateTicket<T>(Employee employee, TicketModel<T> ticketModel)
    {
        var ticket = new Ticket<T>
        {
            Employee = employee,
            Name = ticketModel.Name,
            Ticketstatus = Ticketstatus.Pending,
            Items = ticketModel.Items,
            Organization = employee.Organization,
        };
        
        return ticket;
    }
}