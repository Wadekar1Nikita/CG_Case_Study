using System.ComponentModel.DataAnnotations;

namespace reset
{
    public class ResetPasswordRequest
{
    public string Email{get;set;}
    public string Token { get; set; }

     [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", 
    ErrorMessage = "Password must be at least 8 characters long, include one uppercase letter, one lowercase letter, one number, and one special character.")]       

    public string NewPassword { get; set; }
}

}