using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AuthenticationService.ViewModel.Dtos.Address;

public class AddressDto
{
    /// <summary>
    /// User name is phoneNumber or Email
    /// </summary>
    [JsonPropertyName("addressId")]
    [Required]
    public string AddressId { get; set; }
    /// <summary>
    /// Addtional information
    /// </summary>
    [JsonPropertyName("information")]
    [Required]
    public string Information;
    /// <summary>
    /// Address of place
    /// </summary>
    [JsonPropertyName("address")]
    [Required]
    public string Address;
    /// <summary>
    /// Address Type
    /// </summary>
    [JsonPropertyName("addressType")]
    [Required]
    public string AddressType;
    /// <summary>
    /// Latitude
    /// </summary>
    [JsonPropertyName("lat")]
    [Required]
    public double Lat;
    /// <summary>
    /// Longitude
    /// </summary>
    [JsonPropertyName("lng")]
    [Required]
    public double Lng;
}
