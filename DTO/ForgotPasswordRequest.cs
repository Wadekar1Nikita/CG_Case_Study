using System.ComponentModel.DataAnnotations;

namespace forgot
{
    public class ForgotPasswordRequest
{
    [Required]
    public string Email { get; set; }
}

}