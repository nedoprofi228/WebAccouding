using accoudingWeb.DataBase;
using accoudingWeb.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace accoudingWeb.Services;

public class ManagerService(ApplicationContext dbContext, AccoudingService accoudingService)
{
    public async Task<bool> DeclineTicketAsync<T>(Employee manager, long accoudingId)
    {
        if(await CloseTicketAsync<T>(manager, accoudingId, Ticketstatus.Declined) == null)
            return false;
        
        // логика после успешного закрытия тикета
        // ...
        
        return true;
    }
    
    public async Task<bool> AcceptTicketAsync<T>(Employee manager, long ticketId)
    {
        if(await CloseTicketAsync<T>(manager, ticketId, Ticketstatus.Accepted) == null)
            return false;
        
        // логика после успешного закрытия тикета
        // ...
        
        return true;
    }

    private async Task<Accouding<T>?> CloseTicketAsync<T>(Employee manager, long ticketId, Ticketstatus ticketstatus)
    {
        Console.WriteLine(ticketstatus.ToString());
        var ticket = await dbContext.Set<Ticket<T>>().FirstOrDefaultAsync(x => x.Id == ticketId);
        Console.WriteLine(ticket == null ? "Ticket not found" : ticket.Name);
        
        if(ticket == null || ticket.TicketStatus != Ticketstatus.Pending.ToString())
            return null;

        ticket.TicketStatus = ticketstatus.ToString();
        var accouding = accoudingService.CreateAccouding<T>(manager, ticket);
        
        await dbContext.Set<Accouding<T>>().AddAsync(accouding);
        await dbContext.SaveChangesAsync();
        return accouding;
    }
}