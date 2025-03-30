using System.Text.Json;
using accoudingWeb.DataBase;
using Microsoft.AspNetCore.Mvc;

namespace accoudingWeb.Controllers;

[ApiController]
[Route("items")]
public class ItemsController(ApplicationContext dbContext): ControllerBase
{
    
    /// <summary>
    /// метод для получения предметов нужного типа
    /// </summary>
    /// <param name="type"> тип нужных предметов</param>
    [HttpGet("{type}")]
    public async Task GetItemsAsync([FromRoute] string type)
    {
        Response.ContentType = "application/json";
        Response.StatusCode = 400;
        
        switch (type)
        {
            case "equipment":
                Response.StatusCode = 200;
                await Response.WriteAsync(JsonSerializer.Serialize(dbContext.Equipments.ToList()));
                break;
            
            case "officeequipment":
                Response.StatusCode = 200;
                await Response.WriteAsync(JsonSerializer.Serialize(dbContext.OfficeEquipments.ToList()));
                break;
            
            case "preciousmetals":
                Response.StatusCode = 200;
                await Response.WriteAsync(JsonSerializer.Serialize(dbContext.PreciousMetals.ToList()));
                break;
        }
    }

    /*[HttpGet("have/{type}")]
    public async Task GetHaveItems([FromRoute] string type)
    {
        Response.ContentType = "application/json";
        Response.StatusCode = 400;
        
        switch (type)
        {
            case "equipment":
                Response.StatusCode = 200;
                await Response.WriteAsync(JsonSerializer.Serialize(dbContext.Equipments
                    .Where(e => e.Count > 0).ToList()));
                break;
            
            case "officeEquipment":
                Response.StatusCode = 200;
                await Response.WriteAsync(JsonSerializer.Serialize(dbContext.OfficeEquipments
                    .Where(e => e.Count > 0).ToList()));
                break;
            
            case "preciousMetals":
                Response.StatusCode = 200;
                await Response.WriteAsync(JsonSerializer.Serialize(dbContext.PreciousMetals
                    .Where(e => e.Count > 0).ToList()));
                break;
        }
    }*/
}