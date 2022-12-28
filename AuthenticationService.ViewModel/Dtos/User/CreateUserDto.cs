using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AuthenticationService.ViewModel.Dtos.User;

public class CreateUserDto
{
    /// <summary>
    /// Display Name
    /// </summary>
    [JsonPropertyName("name")]
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    [JsonPropertyName("email")]
    [EmailAddress]
    [Required]
    public string Email { get; set; }

    /// <summary>
    /// Username
    /// </summary>
    [JsonPropertyName("password")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^.*(?=.{10,})(?=.*\d)(?=.*[a-zA-Z])(?=.*[@#$%^&+=]).*$")]
    [Required]
    public string Password { get; set; }

    /// <summary>
    /// Số điện thoại
    /// </summary>
    [JsonPropertyName("phoneNumber")]
    [Phone]
    [Required]
    public string PhoneNumber { get; set; }
}
