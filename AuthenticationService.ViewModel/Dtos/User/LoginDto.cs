using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AuthenticationService.ViewModel.Dtos.User;

/// <summary>
/// Login DTO
/// </summary>
public class LoginDto
{
    /// <summary>
    /// User name is phoneNumber or Email
    /// </summary>
    [JsonPropertyName("userName")]
    [EmailAddress]
    [Required]
    public string UserName { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    [JsonPropertyName("password")]
    [RegularExpression(@"^.*(?=.{10,})(?=.*\d)(?=.*[a-zA-Z])(?=.*[@#$%^&+=]).*$")]
    [Required]
    public string Password { get; set; }
}
