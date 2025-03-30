
using System.Text.Json;
using accoudingWeb.DataBase;
using accoudingWeb.Entities;
using accoudingWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace accoudingWeb.Controllers;

// требуется авторизация, а роли должный быть Админ или Менеджер
// маршрутизация по запросу manager
[ApiController]
[Authorize(Roles = "Admin, Manager")]
[Route("manager")]
public class ManagerController(ApplicationContext dbContext, ManagerService managerService) : ControllerBase
{
    
    /// <summary>
    /// метод отправляющий html документ
    /// </summary>
    /// <param name="context"></param>
    [HttpGet]
    public async Task GetView()
    {
        await Response.SendFileAsync("./Views/Working/Manager.html");
    }
    
    /// <summary>
    /// метод закрытия тикета
    /// </summary>
    /// <param name="context"></param>
    /// <param name="type">Тип объекта тикета</param>
    /// <param name="ticketId">id нужного тикета</param>
    /// <param name="action1"> Действие над тикетом, принять или отклонить</param>
    /// <returns></returns>
    [HttpPost("tickets/{type}/{ticketId}/{action1}")]
    public async Task<IActionResult> CloseTicket([FromRoute] string type, [FromRoute] long ticketId, [FromRoute] string action1)
    {
        var manager = dbContext.Users
            .FirstOrDefault(u => u.Login == HttpContext.User.Identity.Name);

        return type switch
        {
            "equipment"       => 
                await HandleAction<Equipment>(action1, ticketId, manager),
            "officeEquipment" => 
                await HandleAction<OfficeEquipment>(action1, ticketId, manager),
            "preciousMetals"  => 
                await HandleAction<PreciousMetals>(action1, ticketId, manager),
            _                 => 
                BadRequest(ModelState)
        };
    }

    /// <summary>
    ///  обощенный вспомогаеющий метод для закрытия тикета
    /// </summary>
    /// <param name="action"></param>
    /// <param name="ticketId"></param>
    /// <param name="manager"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private async Task<IActionResult> HandleAction<T>(string action, long ticketId, Employee manager)
        where T : class
    {
        var result = action switch
        {
            "accept"  => await managerService.AcceptTicketAsync<T>(manager, ticketId),
            "decline" => await managerService.DeclineTicketAsync<T>(manager, ticketId),
            _         => false
        };

        if (!result) 
            return BadRequest(ModelState);
        
        return Ok();
    }
}