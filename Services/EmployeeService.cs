using accoudingWeb.DataBase;
using accoudingWeb.Entities;
using accoudingWeb.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace accoudingWeb.Services;

public class EmployeeService(TicketService ticketService, ApplicationContext dbContext)
{
    
    public async Task<bool> CreateTicket<T>(Employee employee, TicketModel<T> ticketModel) where T : BaseItem<T>
    {
        var ticket = ticketService.CreateTicket(employee, ticketModel);
        
        await dbContext.Set<Ticket<T>>().AddAsync(ticket);
        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteTicket<T>(Employee employee, long id) 
    {
        var ticket = await dbContext.Set<Ticket<T>>()
            .FirstOrDefaultAsync(a => a.Id == id);
        
        if(ticket == null)
            return false;
        
        dbContext.Set<Ticket<T>>().Remove(ticket);
        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateTicket<T>(Employee employee,long id, TicketModel<T> newTicketModel) where T : BaseItem<T>
    {
        var ticket = dbContext.Set<Ticket<T>>()
            .Include(t => t.Items)
            .FirstOrDefault(a => a.Id == id);
        
        if(ticket == null)
            return false;

        ticket.Name = newTicketModel.Name;
        ticket.Items = dbContext.Set<T>().Where(i => newTicketModel.ItemsId.Contains(i.Id)).ToList();
        ticket.TotalPrice = newTicketModel.TotalPrice;
        ticket.ItemStatus = newTicketModel.ItemStatus;
        dbContext.Update(ticket);
        await dbContext.SaveChangesAsync();
        return true;
    }
}