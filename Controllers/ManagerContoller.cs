
using System.Text.Json;
using accoudingWeb.DataBase;
using accoudingWeb.Entities;
using accoudingWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace accoudingWeb.Controllers;

[Route("api/manager")]
public class ManagerContoller(ApplicationContext dbContext, ManagerService managerService) : Controller
{

    [HttpGet("/accaudings/{type}")]
    public async Task GetAccoudingEquiment(HttpContext context, string type)
    {
        string accoudings = string.Empty;
        switch (type)
        {
            case "equiment":
                accoudings = JsonSerializer.Serialize(dbContext.AccoudingsEquipment.ToList());
                break;
            case "officeEquipment":
                accoudings = JsonSerializer.Serialize(dbContext.AccoudingsOfficeEquipment.ToList());
                break;
            case "preciousMetals":
                accoudings = JsonSerializer.Serialize(dbContext.AccoudingsPreciousMetals.ToList());
                break;
        }
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(accoudings);
    }
    
    [HttpPost("/tickets/{type}/{ticketId}/{action}")]
    public async Task<IActionResult> AcceptPreciousMetalsTicket(HttpContext context,string type, long ticketId, string action)
    {
        var manager = new Employee();
        if(action == "accept")
            switch (type)
        {
            case "equipment":
                if(action == "accept")
                {
                    if (!await managerService.AcceptTicketAsync<Equipment>(manager, ticketId))
                        return BadRequest();
                }
                else if(action == "decline")
                {
                    if (!await managerService.DeclineTicketAsync<Equipment>(manager, ticketId))
                        return BadRequest();
                }
                
                break;
            case "officeEquipment":
                if(action == "accept")
                {
                    if (!await managerService.AcceptTicketAsync<OfficeEquipment>(manager, ticketId))
                        return BadRequest();
                }
                else if(action == "decline")
                {
                    if (!await managerService.DeclineTicketAsync<OfficeEquipment>(manager, ticketId))
                        return BadRequest();
                }
                break;
            case "preciousMetals":
                if(action == "accept")
                {
                    if (!await managerService.AcceptTicketAsync<PreciousMetals>(manager, ticketId))
                        return BadRequest();
                }
                else if(action == "decline")
                {
                    if (!await managerService.DeclineTicketAsync<PreciousMetals>(manager, ticketId))
                        return BadRequest();
                }
                break;
        }
        
        return Ok();
    }
}