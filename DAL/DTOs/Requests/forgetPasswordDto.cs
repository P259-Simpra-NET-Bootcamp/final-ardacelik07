using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.Requests
{
    public class forgetPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    
        public string ClientURI { get; set; }

        public string Code { get; set; }

        public string DomainLink { get; set; }

    }
}
