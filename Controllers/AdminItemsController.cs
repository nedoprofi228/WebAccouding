using accoudingWeb.DataBase;
using accoudingWeb.Entities;
using accoudingWeb.Services;
using accoudingWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace accoudingWeb.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("admin/items")]
public class AdminControllerv(ApplicationContext dbContext) : ControllerBase
{
    #region GEt
    [HttpGet("/admin")]
    public async Task GetAdminView()
    {
        Response.ContentType = "text/html";
        
        await Response.SendFileAsync("./Views/Working/Admin.html");
    }
    
    [HttpGet]
    public async Task GetView()
    {
        Response.ContentType = "text/html";
        
        await Response.SendFileAsync("./Views/Working/AdminItems.html");
    }
    #endregion

    #region POST
    /// <summary>
    /// метод для создания оборудования
    /// </summary>
    /// <param name="equipmentModel">необходимые данные</param>
    /// <returns></returns>
    [HttpPost("equipment")]
    public async Task<IActionResult> PostEquipmentsItems(EquipmentModel equipmentModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        Equipment equipment = new Equipment()
        {
            Name = equipmentModel.Name,
            Price = equipmentModel.Price,
            EquipmentType = equipmentModel.EquipmentType
        };
        
        await dbContext.Equipments.AddAsync(equipment);
        await dbContext.SaveChangesAsync();
        return Ok(equipment);
    }
    
    /// <summary>
    /// метод для создания оргтехники
    /// </summary>
    /// <param name="officeEquipmentModel"></param>
    /// <returns></returns>
    [HttpPost("officeEquipment")]
    public async Task<IActionResult> PostItems(OfficeEquipmentModel officeEquipmentModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        OfficeEquipment officeEquipment = new OfficeEquipment()
        {
            Name = officeEquipmentModel.Name,
            Price = officeEquipmentModel.Price,
            PrinterType = officeEquipmentModel.PrinterType,
        };
        
        await dbContext.OfficeEquipments.AddAsync(officeEquipment);
        await dbContext.SaveChangesAsync();
        return Ok(officeEquipment);
    }
    
    /// <summary>
    /// метод для создания драгметаллов
    /// </summary>
    /// <param name="preciousMetalsModel"></param>
    /// <returns></returns>
    [HttpPost("preciousMetals")]
    public async Task<IActionResult> PostItems(PreciousMetalsModel preciousMetalsModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        PreciousMetals preciousMetal = new PreciousMetals()
        {
            Name = preciousMetalsModel.Name,
            Price = preciousMetalsModel.Price,
            Weight = preciousMetalsModel.Weight,
            Precious = preciousMetalsModel.Precious
        };
        
        await dbContext.PreciousMetals.AddAsync(preciousMetal);
        await dbContext.SaveChangesAsync();
        return Ok(preciousMetal);
    }
    
    #endregion
    
    #region PUT
    /// <summary>
    ///  метод для изменения оборудования
    /// </summary>
    /// <param name="id"></param>
    /// <param name="equipmentModel"></param>
    /// <returns></returns>
    [HttpPut("equipment")]
    public async Task<IActionResult> PutquipmentsItems(long id, EquipmentModel equipmentModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var equipment = await dbContext.Equipments.FindAsync(id);
        
        if(equipment == null)
        {
            ModelState.AddModelError("error", "Equipment not found");
            return BadRequest(ModelState);
        }
        
        equipment.Name = equipmentModel.Name;
        equipment.Price = equipmentModel.Price;
        equipment.EquipmentType = equipmentModel.EquipmentType;
        
        await dbContext.SaveChangesAsync();
        return Ok(equipment);
    }
    
    /// <summary>
    /// метод для изменения оргтехники
    /// </summary>
    /// <param name="id"></param>
    /// <param name="officeEquipmentModel"></param>
    /// <returns></returns>
    [HttpPut("officeEquipment")]
    public async Task<IActionResult> PutItem(long id,OfficeEquipmentModel officeEquipmentModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var officeEquipment = await dbContext.OfficeEquipments.FindAsync(id);
        
        if(officeEquipment == null)
        {
            ModelState.AddModelError("error", "Equipment not found");
            return BadRequest(ModelState);
        }
        
        officeEquipmentModel.Name = officeEquipment.Name;
        officeEquipment.Price = officeEquipmentModel.Price;
        officeEquipment.PrinterType = officeEquipmentModel.PrinterType;
        
        await dbContext.SaveChangesAsync();
        return Ok(officeEquipment);
    }
    
    /// <summary>
    /// метод для изменения драг металлов
    /// </summary>
    /// <param name="id"></param>
    /// <param name="newPreciousMetalsModel"></param>
    /// <returns></returns>
    [HttpPut("preciousMetals")]
    public async Task<IActionResult> PutItem(long id, PreciousMetalsModel newPreciousMetalsModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var preciousMetal = await dbContext.PreciousMetals.FindAsync(id);
        
        if(preciousMetal == null)
        {
            ModelState.AddModelError("error", "Equipment not found");
            return BadRequest(ModelState);
        }
        
        preciousMetal.Name = newPreciousMetalsModel.Name;
        preciousMetal.Price = newPreciousMetalsModel.Price;
        preciousMetal.Weight = newPreciousMetalsModel.Weight;
        preciousMetal.Precious = newPreciousMetalsModel.Precious;
        
        await dbContext.SaveChangesAsync();
        return Ok(preciousMetal);
    }
    
    #endregion
    
    /// <summary>
    /// метод для удаления предметов
    /// </summary>
    /// <param name="type">тип предмета/param>
    /// <param name="id">id предмета</param>
    /// <returns></returns>
    [HttpDelete("{type}/{id}")]
    public async Task<IActionResult> DeleteTickets(string type, long id)
    {
        switch (type)
        {
            case "equipment":
                var equipment = await dbContext.Equipments.FindAsync(id);
                if (equipment == null)
                    return BadRequest(id);
                
                dbContext.Equipments.Remove(equipment);
                await dbContext.SaveChangesAsync();
                return Ok();

            case "officeEquipment":
                var officeEquipment = await dbContext.OfficeEquipments.FindAsync(id);
                if (officeEquipment == null)
                    return BadRequest(id);
                
                dbContext.OfficeEquipments.Remove(officeEquipment);
                await dbContext.SaveChangesAsync();
                return Ok();
            
            case "preciousMetals":
                var preciousMetal = await dbContext.PreciousMetals.FindAsync(id);
                if (preciousMetal == null)
                    return BadRequest(id);
                
                dbContext.PreciousMetals.Remove(preciousMetal);
                await dbContext.SaveChangesAsync();
                return Ok();
        }

        return BadRequest(id);
    }
}