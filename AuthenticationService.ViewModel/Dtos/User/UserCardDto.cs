using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.ViewModel.Dtos.User;

public class UserCardDto
{
    /// <summary>
    /// Code/Name
    /// </summary>
    [Required]
    [FromQuery]
    public String UserId { get; set; }
    /// <summary>
    /// Code/Name
    /// </summary>
    [Required]
    [FromQuery]
    public String Name { get; set; }
    /// <summary>
    /// Code/Name
    /// </summary>
    [Required]
    [FromQuery]
    public String Email { get; set; }
    /// <summary>
    /// Code/Name
    /// </summary>
    [Required]
    [FromQuery]
    public String Address { get; set; }
    /// <summary>
    /// Code/Name
    /// </summary>
    [Required]
    [FromQuery]
    public String PhoneNumber { get; set; }
    /// <summary>
    /// Code/Name
    /// </summary>
    [Required]
    [FromQuery]
    public String Gender { get; set; }
    /// <summary>
    /// Code/Name
    /// </summary>
    [Required]
    [FromQuery]
    public String Status { get; set; }
}
