using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AuthenticationService.ViewModel.Dtos.User
{
    public class NewsUserDto
    {
        [JsonPropertyName("name")]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [JsonPropertyName("ID")]
        [Required]
        public string UserID { get; set; }
    }
}
