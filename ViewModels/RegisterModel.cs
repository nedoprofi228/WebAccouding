using System.ComponentModel.DataAnnotations;

namespace accoudingWeb.ViewModels;

public class RegisterModel
{
    [Required(ErrorMessage = "Не указано имя")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Не указан логин")]
    public string Login { get; set; }

    [Required(ErrorMessage = "Не указан пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string ConfirmPassword { get; set; }
}
