using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.ViewModel.Dtos.User;

public class RenewPasswordDto
{
    [EmailAddress]
    [Required]
    public string Email { get; set; }
    [Required]
    [RegularExpression(@"^.*(?=.{10,})(?=.*\d)(?=.*[a-zA-Z])(?=.*[@#$%^&+=]).*$")]
    public string Password { get; set; }
    [Required]
    public string Token { get; set; }
}
