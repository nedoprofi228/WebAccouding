using System.Security.Claims;
using accoudingWeb.DataBase;
using accoudingWeb.Entities;
using accoudingWeb.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace accoudingWeb.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(ApplicationContext dbContext) : ControllerBase
{

    [HttpGet("login")]
    public async Task Login()
    {
        Response.ContentType = "text/html";
        await Response.SendFileAsync("./Views/Auth/Login.html");
    }

    /// <summary>
    /// метод для авторизации
    /// </summary>
    /// <param name="model">данные пользователя</param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("error", "Некорректные логин и(или) пароль");

            return BadRequest(ModelState);
        }

        Employee? user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Login == model.Login && u.Password == model.Password);

        if (user == null)
        {
            ModelState.AddModelError("error", "user not exist");
            return BadRequest(ModelState);
        }

        // если пользователь есть, то авторизует его
        await Authenticate(user.Login, user.Role); // аутентификация

        Response.Redirect("/", true);
        return Ok(user);
    }


    [HttpGet("register")]
    public async Task Register()
    {
        await Response.SendFileAsync("./Views/Auth/Register.html");
    }

    /// <summary>
    /// метод для регистрации пользователя
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("error", "Некорректные логин и(или) пароль");
            return BadRequest(ModelState);
        }

        Employee? user = await dbContext.Users.FirstOrDefaultAsync(u => u.Login == model.Login);
        if (user != null)
        {
            ModelState.AddModelError("error", "user already exist");
            return BadRequest(ModelState);
        }

        // добавляем пользователя в бд
        user = new Employee { Login = model.Login, Password = model.Password, Role = Role.Employee.ToString() };
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        // авторизует нового пользователя
        await Authenticate(user.Login, user.Role); // аутентификация

        Response.Redirect("/", true);
        return Ok(user);
    }

    [HttpGet("accessDenied")]
    public async Task AccessDenied()
    {
        Response.ContentType = "text/html";
        
        await Response.SendFileAsync("./Views/Errors/AccessDenied.html");
    }


    /// <summary>
    /// метод для авторизации
    /// </summary>
    /// <param name="login">логин пользователя</param>
    /// <param name="role">роль пользователя</param>
    private async Task Authenticate(string login, string role)
    {
        // создаем один claim - данные о пользователе
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, login),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
        };
        
        // создаем объект ClaimsIdentity 
        ClaimsIdentity identity = new ClaimsIdentity(claims, "ApplicationCookie",
            ClaimsIdentity.DefaultNameClaimType,    
            ClaimsIdentity.DefaultRoleClaimType);
        
        // установка аутентификационных куки
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
    }

    /// <summary>
    /// метод для удаления пользователя из авторизованных пользователей
    /// </summary>
    /// <returns></returns>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok();
    }
}
