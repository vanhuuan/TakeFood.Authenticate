using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.ViewModel.Dtos.User
{
    public class DetailsUserDto:ShowUserDto
    {
        public string avatar { get; set; }
        public DateTime? createdDate { get; set; }
    }
}
