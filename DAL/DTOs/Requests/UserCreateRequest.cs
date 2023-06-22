using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.Requests
{
    public class UserCreateRequest
    {

        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }

        public int balance { get; set; }
   
        public string PhoneNumber { get; set; }
        [Required]
        public string email { get; set; }

        public int point { get; set; }
        public string Address { get; set; }
        [Required]
        public string Password { get; set; }

        

    }
}

