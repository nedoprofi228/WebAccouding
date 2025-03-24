using accoudingWeb.Entities;
using accoudingWeb.Services;
using accoudingWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace accoudingWeb.Controllers;

[Route("api/tickets")]
public class EmployeeController(EmployeeService employeeService): Controller
{
    [HttpGet]
    public async Task Get(HttpContext context)
    {
        await context.Response.SendFileAsync("");
    }

    [HttpPost("/{}")]
    public async Task<IActionResult> PostEquipment(TicketModel<Equipment> equipmentAccouding)
    {
        if (await employeeService.CreateTicket<Equipment>(new Employee(), equipmentAccouding))
        {
            return Ok();
        }
        
        return BadRequest(equipmentAccouding);
    }

    [HttpPut("/equipment/{id}")]
    public async Task<IActionResult> PutEquipment(long id, TicketModel<Equipment> newTicketModel)
    {
        if (await employeeService.UpdateTicket<Equipment>(new Employee(),id, newTicketModel))
            return Ok();
        
        return BadRequest(newTicketModel);
    }

    [HttpDelete("/equipment/{id}")]
    public async Task<IActionResult> DeleteEquipment(long id)
    {
        if (await employeeService.DeleteTicket<Equipment>(new Employee(), id))
            return Ok();
        
        return BadRequest(id);
    }
    
    [HttpPost("/officeEquipment")]
    public async Task<IActionResult> PostOfficeEquipment(TicketModel<OfficeEquipment> equipmentAccouding)
    {
        if (await employeeService.CreateTicket<OfficeEquipment>(new Employee(), equipmentAccouding))
        {
            return Ok();
        }
        
        return BadRequest(equipmentAccouding);
    }

    [HttpPut("/officeEquipment/{id}")]
    public async Task<IActionResult> PutOfficeEquipment(long id, TicketModel<OfficeEquipment> newTicketModel)
    {
        if (await employeeService.UpdateTicket<OfficeEquipment>(new Employee(),id, newTicketModel))
            return Ok();
        
        return BadRequest(newTicketModel);
    }

    [HttpDelete("/officeEquipment/{id}")]
    public async Task<IActionResult> DeleteOfficeEquipment(long id)
    {
        if (await employeeService.DeleteTicket<OfficeEquipment>(new Employee(), id))
            return Ok();
        
        return BadRequest(id);
    }
    
    [HttpPost("/PreciousMetals")]
    public async Task<IActionResult> PostPreciousMetals(TicketModel<PreciousMetals> equipmentAccouding)
    {
        if (await employeeService.CreateTicket<PreciousMetals>(new Employee(), equipmentAccouding))
        {
            return Ok();
        }
        
        return BadRequest(equipmentAccouding);
    }

    [HttpPut("/PreciousMetals/{id}")]
    public async Task<IActionResult> PutPreciousMetals(long id, TicketModel<PreciousMetals> newTicketModel)
    {
        if (await employeeService.UpdateTicket<PreciousMetals>(new Employee(),id, newTicketModel))
            return Ok();
        
        return BadRequest(newTicketModel);
    }

    [HttpDelete("/PreciousMetals/{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        if (await employeeService.DeleteTicket<PreciousMetals>(new Employee(), id))
            return Ok();
        
        return BadRequest(id);
    }
    
}