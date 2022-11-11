using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AuthenticationService.ViewModel.Dtos.User
{
    public class ShowUserDto
    {
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
        [JsonPropertyName("address")]
        public List<string> Address { get; set; }
        [JsonPropertyName("Phone")]
        public string Phone { get; set; }
        [JsonPropertyName("gender")]
        public string Gender { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
