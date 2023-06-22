using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.DTOs.Requests
{
    public class UserRegistrationRequest
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
