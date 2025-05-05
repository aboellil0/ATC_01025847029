using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Core.DTOs.Auth
{
    public class LoginReq
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DeviceId { get; set; }
    }
}
