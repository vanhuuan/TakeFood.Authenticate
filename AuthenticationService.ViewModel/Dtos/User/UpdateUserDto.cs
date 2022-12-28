using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AuthenticationService.ViewModel.Dtos.User;

public class UpdateUserDto
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
    /// Số điện thoại
    /// </summary>
    [JsonPropertyName("phoneNumber")]
    [Phone]
    [Required]
    public string PhoneNumber { get; set; }
    /// <summary>
    /// Avatar
    /// </summary>
    [JsonPropertyName("avatar")]
    [Required]
    public string Avatar { get; set; }
}
