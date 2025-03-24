using System.Security.Claims;
using accoudingWeb.DataBase;
using accoudingWeb.Entities;
using accoudingWeb.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace accoudingWeb.Controllers;

[Route("api/Auth")]
public class AuthController(ApplicationContext dbContext) : Controller
{

    [HttpGet("/login")]
    public async Task Login(HttpContext context)
    {
        await context.Response.SendFileAsync("..//Views//Auth//Login.html");
    }

    [HttpPost("/login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            Employee? user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Login == model.Login && u.Password == model.Password);
            if (user != null)
            {
                await Authenticate(user.Login, user.Role); // аутентификация

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Некорректные логин и(или) пароль");
        }

        return BadRequest(model);
    }

    [HttpGet("/register")]
    public async Task Register(HttpContext context)
    {
        await context.Response.SendFileAsync("..//Views//Auth//Register.html");
    }

    [HttpPost("/register")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            Employee? user = await dbContext.Users.FirstOrDefaultAsync(u => u.Login == model.Login);
            if (user == null)
            {
                // добавляем пользователя в бд
                user = new Employee { Login = model.Login, Password = model.Password, Role = Role.Employee };
                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();

                await Authenticate(user.Login, user.Role); // аутентификация

                return RedirectToAction("Index", "Home");
            }
            else
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
        }

        return BadRequest(model);
    }

    private async Task Authenticate(string login, Role role)
    {
        // создаем один claim - данные о пользователе
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, login),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, role.ToString())
        };
        
        // создаем объект ClaimsIdentity 
        ClaimsIdentity identity = new ClaimsIdentity(claims, "ApplicationCookie",
            ClaimsIdentity.DefaultNameClaimType,    
            ClaimsIdentity.DefaultRoleClaimType);
        
        // установка аутентификационных куки
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Auth");
    }
}
