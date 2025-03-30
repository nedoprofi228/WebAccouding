using accoudingWeb.DataBase;
using accoudingWeb.Entities;
using accoudingWeb.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder();
CreateFirstAdmin();

builder.Services.AddDbContext<ApplicationContext>();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => //CookieAuthenticationOptions
    {
        options.LoginPath = new PathString("/auth/login");
        options.AccessDeniedPath = new PathString("/auth/accessDenied");
    });;
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<AccoudingService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<ManagerService>();
builder.Services.AddScoped<TicketService>();

var app = builder.Build();
app.MapControllers();
app.UseRouting();
app.UseAuthentication();    // аутентификация
app.UseAuthorization();     // авторизация
app.UseSwagger();
app.MapSwagger().RequireAuthorization();
app.UseSwaggerUI();

app.MapGet("/", (context) => context.Response.SendFileAsync("./Views/Home/Index.html"));
CreateFirstAdmin();
app.Run();

void CreateFirstAdmin()
{
    var db = new ApplicationContext();
    if (!db.Users.Any())
        db.Users.Add(new Employee()
        {
            Name = "admin",
            Login = "admin",
            Password = "admin",
            Role = "Admin"
        });
    
    db.SaveChanges();
}

