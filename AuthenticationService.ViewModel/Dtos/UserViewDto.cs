﻿using System.Text.Json.Serialization;

namespace AuthenticationService.ViewModel.Dtos;

public class UserViewDto
{
    /// <summary>
    /// User Id
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }
    /// <summary>
    /// Namee
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }
    /// <summary>
    /// Email
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; }
    /// <summary>
    /// Photo
    /// </summary>
    [JsonPropertyName("photo")]
    public string Photo { get; set; }
    /// <summary>
    /// Photo
    /// </summary>
    [JsonPropertyName("phone")]
    public string Phone { get; set; }
    /// <summary>
    /// List role
    /// </summary>
    [JsonPropertyName("roles")]
    public IList<string> Roles { get; set; }

    /// <summary>
    /// Photo
    /// </summary>
    [JsonPropertyName("accessToken")]
    public string? AccessToken { get; set; }

    /// <summary>
    /// Photo
    /// </summary>
    [JsonPropertyName("refreshToken")]
    public string? RefreshToken { get; set; }
}
