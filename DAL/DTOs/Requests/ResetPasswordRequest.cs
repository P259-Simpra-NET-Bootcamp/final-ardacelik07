using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.Requests
{
    public class ResetPasswordRequest
    {
      

        [Required]
        public string Password { get; set; }


        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
