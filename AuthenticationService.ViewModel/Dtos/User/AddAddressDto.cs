using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AuthenticationService.ViewModel.Dtos.User;

public class AddAddressDto
{
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
