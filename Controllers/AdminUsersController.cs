using System.Text.Json;
using accoudingWeb.DataBase;
using accoudingWeb.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace accoudingWeb.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("admin/users")]
public class AdminUsersController(ApplicationContext dbContext): ControllerBase
{
    [HttpGet]
    public async Task GetPage()
    {
        Response.ContentType = "text/html";

        await Response.SendFileAsync("./Views/Working/AdminUsers.html");
    }
    
    [HttpGet("list")]
    public async Task GetUsers()
    {
        Response.ContentType = "application/json";
        
        await Response.WriteAsync(JsonSerializer.Serialize(dbContext.Users.ToList()));
    }
    
    /// <summary>
    /// метод для управления ролями пользователя
    /// </summary>
    /// <param name="login"></param>
    /// <param name="action1"></param>
    /// <returns></returns>
    [HttpPost("{login}/{action1}")]
    public async Task<IActionResult> Login([FromRoute] string login, [FromRoute] string action1)
    {
        // находит пользователя в бд
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Login == login);
        Role? role;
        if (user == null)
            return BadRequest();

        // проверяет действие
        switch (action1)
        {
            case "up":
            {
                role = Enum.Parse<Role>(user.Role);
                if (role != Role.Admin)
                    role++;

                user.Role = role.ToString();
                await dbContext.SaveChangesAsync();
                return Ok();
            }
            case "down":
                role = Enum.Parse<Role>(user.Role);
                if (role != Role.Employee)
                    role--;

                user.Role = role.ToString();
                await dbContext.SaveChangesAsync();
                return Ok();
        }

        return BadRequest(action1);
    }
}