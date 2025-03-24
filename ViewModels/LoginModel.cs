using System.ComponentModel.DataAnnotations;

namespace accoudingWeb.ViewModels;

public class LoginModel
{
    [Required(ErrorMessage = "Не указан Login")]
    public string Login { get; set; }
         
    [Required(ErrorMessage = "Не указан пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}