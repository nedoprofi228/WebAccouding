using System.Net.Mime;
using System.Text.Json;
using accoudingWeb.DataBase;
using accoudingWeb.Entities;
using accoudingWeb.Services;
using accoudingWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace accoudingWeb.Controllers;

// требуется авторизация, а роли должный быть Админ или Работник
// маршрутизация по запросу api/tickets
[ApiController]
[Authorize(Roles = "Admin, Employee")]
[Route("tickets")]
public class TicketController(EmployeeService employeeService, ApplicationContext dbContext): ControllerBase
{
    
    /// <summary>
    ///  метод для получения страницы html
    /// </summary>
    /// <param name="context"></param>
    [HttpGet("/employee")]
    public async Task Get()
    {
        // отправка html документа
        Response.ContentType = "text/html";
        
        await Response.SendFileAsync("./Views/Working/Employee.html");
    }

    /// <summary>
    /// обощенный метод для получения тикетов с предметами какого-то типа
    /// </summary>
    /// <param name="itemsType">тип предметов</param>
    [HttpGet("{itemsType}")]
    public async Task GetItems([FromRoute] string itemsType)
    {
        Response.ContentType = "application/json";
        
        var user = await dbContext.Users
            .FirstOrDefaultAsync( u => u.Login == HttpContext.User.Identity.Name);
        
        switch (itemsType)
        {
            case "equipment":
                // отправляет сериализованный в джсон список тикетов с оборудованием
                await Response.WriteAsync(JsonSerializer.Serialize(dbContext.TicketsEquipment
                    .Where(e => e.EmployeeId == user.Id).ToList()));
                return;
            
            case "officeEquipment":
                await Response.WriteAsync(JsonSerializer.Serialize(dbContext.TicketsOfficeEquipment
                    .Where(e => e.EmployeeId == user.Id).ToList()));
                return;
            
            case "preciousMetals":
                await Response.WriteAsync(JsonSerializer.Serialize(dbContext.TicketsPreciousMetals
                    .Where(e => e.EmployeeId == user.Id).ToList()));
                return;
        }
    }
    
    [HttpGet("{itemsType}/{id}")]
    public async Task GetItemById([FromRoute] string itemsType, [FromRoute] long id)
    {
        Response.ContentType = "application/json";
        
        var user = await dbContext.Users
            .FirstOrDefaultAsync( u => u.Login == HttpContext.User.Identity.Name);
        
        switch (itemsType)
        {
            case "equipment":
                await Response.WriteAsync(JsonSerializer.Serialize(await dbContext.TicketsEquipment
                    .Where(e => e.EmployeeId == user.Id && e.Id == id).FirstOrDefaultAsync()));
                return;
            
            case "officeEquipment":
                await Response.WriteAsync(JsonSerializer.Serialize(dbContext.TicketsOfficeEquipment
                    .Where(e => e.EmployeeId == user.Id && e.Id == id).FirstOrDefaultAsync()));
                return;
            
            case "preciousMetals":
                await Response.WriteAsync(JsonSerializer.Serialize(dbContext.TicketsPreciousMetals
                    .Where(e => e.EmployeeId == user.Id&& e.Id == id).FirstOrDefaultAsync()));
                return;
        }
    }
    
    [HttpGet("pending/{itemsType}")]
    public async Task GetItemsPendingByUser([FromRoute] string itemsType)
    {
        Response.ContentType = "application/json";
        
        var user = await dbContext.Users
            .FirstOrDefaultAsync( u => u.Login == HttpContext.User.Identity.Name);
        
        switch (itemsType)
        {
            case "equipment":
                await Response.WriteAsync(JsonSerializer.Serialize(dbContext.TicketsEquipment
                    .Where(e => e.EmployeeId == user.Id 
                                && e.TicketStatus == Ticketstatus.Pending.ToString()).ToList()));
                return;
            
            case "officeEquipment":
                await Response.WriteAsync(JsonSerializer.Serialize(dbContext.TicketsOfficeEquipment
                    .Where(e => e.EmployeeId == user.Id
                                && e.TicketStatus == Ticketstatus.Pending.ToString()).ToList()));
                return;
            
            case "preciousMetals":
                await Response.WriteAsync(JsonSerializer.Serialize(dbContext.TicketsPreciousMetals
                    .Where(e => e.EmployeeId == user.Id
                                && e.TicketStatus == Ticketstatus.Pending.ToString()).ToList()));
                return;
        }
    }
    
    /// <summary>
    ///  метод для отправки заявки с оборудованием
    /// </summary>
    /// <param name="context"></param>
    /// <param name="equipmentAccouding"> данные об тикете</param>
    /// <returns></returns>
    [HttpPost("equipment")]
    public async Task<IActionResult> PostEquipment(TicketModel<Equipment> ticketModel)
    {
        // получения работника который отправляет заявку
        Employee employee = await dbContext.Users.FirstOrDefaultAsync(u => u.Login == HttpContext.User.Identity.Name);
        
        // если тикет оборудования создан успешно
        if (await employeeService.CreateTicket<Equipment>(employee, ticketModel))
        {
            // отправка положительного ответа
            return Ok();
        }
        
        // иначе отправка отрицательного ответа
        return BadRequest(ticketModel);
    }

    /// <summary>
    /// метод для исправления заявки c предметами офисной техники
    /// </summary>
    /// <param name="id"> id нужной заявки</param>
    /// <param name="ticketModel"> исправленная заявка</param>
    /// <returns></returns>
    [HttpPut("equipment/{id}")]
    public async Task<IActionResult> PutEquipment([FromRoute] long id, TicketModel<Equipment> ticketModel)
    {
        Employee employee = await dbContext.Users.FirstOrDefaultAsync(u => u.Login == HttpContext.User.Identity.Name);
        
        // также с обновлением ответа
        if (await employeeService.UpdateTicket<Equipment>(employee,id, ticketModel))
            return Ok();
        
        return BadRequest(ticketModel);
    }
    
    /// <summary>
    /// метод для отправки заявки c предметами офисной техники
    /// </summary>
    /// <param name="ticketModel"></param>
    /// <returns></returns>
    [HttpPost("officeEquipment")]
    public async Task<IActionResult> PostOfficeEquipment(TicketModel<OfficeEquipment> ticketModel)
    {
        Employee employee = await dbContext.Users.FirstOrDefaultAsync(u => u.Login == HttpContext.User.Identity.Name);
        if (await employeeService.CreateTicket<OfficeEquipment>(employee, ticketModel))
        {
            return Ok();
        }
        
        return BadRequest(ticketModel);
    }

    /// <summary>
    /// метод для исправления заявки с офисной техникой
    /// </summary>
    /// <param name="id"></param>
    /// <param name="newTicketModel"></param>
    /// <returns></returns>
    [HttpPut("officeEquipment/{id}")]
    public async Task<IActionResult> PutOfficeEquipment(long id, TicketModel<OfficeEquipment> ticketModel)
    {
        Employee employee = await dbContext.Users.FirstOrDefaultAsync(u => u.Login == HttpContext.User.Identity.Name);
        if (await employeeService.UpdateTicket<OfficeEquipment>(employee,id, ticketModel))
            return Ok();
        
        return BadRequest(ticketModel);
    }

    
    /// <summary>
    /// метод для добавления новой заявки с драгоценными металлами
    /// </summary>
    /// <param name="equipmentAccouding"></param>
    /// <returns></returns>
    [HttpPost("preciousMetals")]
    public async Task<IActionResult> PostPreciousMetals(TicketModel<PreciousMetals> ticketModel)
    {
        Employee employee = await dbContext.Users.FirstOrDefaultAsync(u => u.Login == HttpContext.User.Identity.Name);
        if (await employeeService.CreateTicket<PreciousMetals>(employee, ticketModel))
        {
            return Ok();
        }
        
        return BadRequest(ticketModel);
    }

    
    /// <summary>
    /// метод для исправления заявки с драгоценными металлами
    /// </summary>
    /// <param name="id"></param>
    /// <param name="newTicketModel"></param>
    /// <returns></returns>
    [HttpPut("preciousMetals/{id}")]
    public async Task<IActionResult> PutPreciousMetals(long id, TicketModel<PreciousMetals> newTicketModel)
    {
        Employee employee = await dbContext.Users.FirstOrDefaultAsync(u => u.Login == HttpContext.User.Identity.Name);
        if (await employeeService.UpdateTicket<PreciousMetals>(employee,id, newTicketModel))
            return Ok();
        
        return BadRequest(newTicketModel);
    }
    
    
    /// <summary>
    /// метод для удаления предмета
    /// </summary>
    /// <param name="type">тип предмета</param>
    /// <param name="id">id нужного предмета</param>
    /// <returns></returns>
    [HttpDelete("{type}/{id}")]
    public async Task<IActionResult> Delete(string type, long id)
    {
        Employee employee = await dbContext.Users.FirstOrDefaultAsync(u => u.Login == HttpContext.User.Identity.Name);
        switch (type)
        {
            case "equipment":
                if(await employeeService.DeleteTicket<Equipment>(employee, id))
                    return Ok();
                break;
            case "officeequipment":
                if (await employeeService.DeleteTicket<OfficeEquipment>(employee, id))
                    return Ok();
                break;
            case "preciousmetals":
                if (await employeeService.DeleteTicket<PreciousMetals>(employee, id))
                    return Ok();
                break;
        }

        return BadRequest(id);
    }
    
}