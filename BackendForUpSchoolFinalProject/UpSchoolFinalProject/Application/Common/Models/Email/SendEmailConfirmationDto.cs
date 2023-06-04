using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models.Email
{
    public class SendEmailConfirmationDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string Link { get; set; }
    }
}
