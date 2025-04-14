using System.ComponentModel.DataAnnotations;

namespace FinaData.Core.Requests.Account;

public class RegisterRequest : Request
{
    [Required(ErrorMessage = "E-mail")]
    [EmailAddress(ErrorMessage = "E-mail inválido")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Senha Inválida")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirmação de senha é obrigatória")]
    public string ConfirmPassword { get; set; } = string.Empty;
}