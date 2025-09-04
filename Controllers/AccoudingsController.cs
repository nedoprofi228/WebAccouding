using System.Text.Json;
using accoudingWeb.DataBase;
using accoudingWeb.DTO;
using accoudingWeb.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace accoudingWeb.Controllers;

[ApiController]
[Route("accoudings")]
public class AccoudingsController(ApplicationContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task GetView()
    {
        Response.ContentType = "text/html";

        await Response.SendFileAsync("./Views/Working/Accoudings.html");
    }

    /// <summary>
    /// метод отправляющий джсон с отчетами нужного премета
    /// </summary>
    /// <param name="context"></param>
    /// <param name="type">предмет</param>
    [HttpGet("{type}")]
    public async Task GetAccoudings([FromRoute] string type)
    {

        // переменная для джсона
        string accoudings = string.Empty;
        switch (type)
        {
            // если нужный предмет это оборудование
            case "equipment":

                // перевод массива объектов в джсон
                accoudings = await ConvertAccoudingToJsonAsync(dbContext.AccoudingsEquipment);
                break;

            case "officeEquipment":
                accoudings = await ConvertAccoudingToJsonAsync(dbContext.AccoudingsOfficeEquipment);
                break;

            case "preciousMetals":
                accoudings = await ConvertAccoudingToJsonAsync(dbContext.AccoudingsPreciousMetals);
                break;
        }

        // тип ответа это джсон
        Response.ContentType = "application/json";

        // отправка ответа
        await Response.WriteAsync(accoudings);
    }

    /// <summary>
    /// метод для удаления отчета 
    /// </summary>
    /// <param name="type">тип отчета</param>
    /// <param name="id">id отчета</param>
    /// <returns></returns>
    [HttpDelete("{type}/{id}")]
    public async Task<IActionResult> DeleteAccoudings([FromRoute] string type, long id)
    {
        bool result = type switch
        {
            "equipment" => await DeleteAccouding(id, dbContext.AccoudingsEquipment),
            "officeEquipment" => await DeleteAccouding(id, dbContext.AccoudingsOfficeEquipment),
            "preciousMetals" => await DeleteAccouding(id, dbContext.AccoudingsPreciousMetals)
        };

        if (!result)
            return BadRequest();

        return Ok();
    }

    // метод удаляющий отчет
    private async Task<bool> DeleteAccouding<T>(long id, DbSet<Accouding<T>> dbSet)
    {
        // находит отчет
        var accouding = await dbSet.FirstOrDefaultAsync(a => a.Id == id);

        if (accouding == null)
            return false;

        // удаляет и сохраняет это
        dbSet.Remove(accouding);
        await dbContext.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// обобщенный метод для конвертации отчета в джсон
    /// </summary>
    /// <param name="dbSet">таблица с предметом</param>
    /// <typeparam name="T">тип предмета</typeparam>
    /// <returns></returns>
    private async Task<string> ConvertAccoudingToJsonAsync<T>(DbSet<Accouding<T>> dbSet)
    {
        // перевод массива объектов в джсон
        return JsonSerializer.Serialize(dbSet
                // подтягивает зависимые данные
            .Include(a => a.Ticket)
            .ThenInclude(t => t.Items)
            .Include(a => a.Manager)
                // создает другой класс для передачи данных на фронт с помощью DTO
            .Select(a => new AccoudingDto<T>()
            {
                Id = a.Id,
                Date = a.Date,
                Manager = a.Manager,
                Ticket = new TicketDto<T>()
                {
                    Id = a.Ticket.Id,
                    Name = a.Ticket.Name,
                    ItemStatus = a.Ticket.ItemStatus,
                    TicketStatus = a.Ticket.TicketStatus,
                    TotalPrice = a.Ticket.TotalPrice,
                    EmployeeName = a.Ticket.Employee.Name,
                    Items = a.Ticket.Items
                }
            })
            .ToList());
    }
}